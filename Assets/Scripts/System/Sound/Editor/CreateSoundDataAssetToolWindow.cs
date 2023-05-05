using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

/// <summary>
/// サウンドデータ用ScriptableObjectの生成用Window
/// </summary>
public class CreateSoundDataAssetToolWindow : EditorWindow
{
    //==========================================================
    // private class
    //==========================================================

    /// <summary>
    /// サウンドデータのデータ
    /// </summary>
    private class CreateData
    {
        public AudioClip Clip { get; set; }
        public SoundAddress Address { get; set; } = SoundAddress.None;
    }

    //==========================================================
    // constant
    //==========================================================

    private readonly Vector2 WINDOW_SIZE = new Vector2(700, 450);

    //==========================================================
    // variable
    //==========================================================

    private static bool s_IsOpen;

    private string _name;
    private Define.Sound.Type _soundType;
    private List<VisualElement> _soundDataList;
    private List<CreateData> _createSoundDataList;

    //==========================================================
    // private method
    //==========================================================

    /// <summary>
    /// Windowを開く際の処理
    /// </summary>
    [MenuItem("Tools/Sound/CreateDataAsset")]
    private static void Open()
    {
        if (s_IsOpen)
        {
            Debug.LogWarning($"すでに{nameof(CreateSoundDataAssetToolWindow)}を開いています。");
            return;
        }
        s_IsOpen = true;

        var window = GetWindow<CreateSoundDataAssetToolWindow>();
        window.titleContent = new GUIContent("SoundCreateDataTool");
        window.Show();
    }

    /// <summary>
    /// データの生成
    /// </summary>
    private void CreateDataAsset()
    {
        var asset = CreateInstance<SoundDataAsset>();
        
        if (_createSoundDataList != null) 
            _createSoundDataList.ForEach(data => asset.CreateData(data.Clip, data.Address));
        
        if (_name == "") _name = "New_Sound_Data_Asset";
        
        var path = "Assets/Data/Sound";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        AssetDatabase.CreateAsset(asset, Path.Combine(path, $"{_name}.asset"));
    }

    /// <summary>
    /// データ生成用リクエストボタンの作成
    /// </summary>
    /// <returns></returns>
    private VisualElement CreateButton()
    {
        var button_field = new Button(() => CreateDataAsset());
        button_field.text = "Create Asset";

        return button_field;
    }

    /// <summary>
    /// ファイルネーム作成用のTextFieldを作成
    /// </summary>
    /// <returns></returns>
    private VisualElement CreateText()
    {
        var text_field = new TextField("Sound Data Name");
        text_field.RegisterCallback<ChangeEvent<string>>(change => _name = change.newValue);

        return text_field;
    }

    /// <summary>
    /// サウンドタイプ設定用のポップアップを作成
    /// </summary>
    /// <returns></returns>
    private VisualElement CreateSoundTypePopup()
    {
        List<Define.Sound.Type> sound_type_popup_list = new List<Define.Sound.Type>();
        for (int index = 0; index < (int)Define.Sound.Type.Length; index++)
        {
            sound_type_popup_list.Add((Define.Sound.Type)index);
        }

        var sound_type_popup = new PopupField<Define.Sound.Type>("Sound Type", sound_type_popup_list, 0);
        sound_type_popup.RegisterCallback<ChangeEvent<Define.Sound.Type>>(change => _soundType = change.newValue);

        return sound_type_popup;
    }

    /// <summary>
    /// データ作成時に初期で登録するサウンドデータ用リストの作成
    /// </summary>
    /// <returns></returns>
    private VisualElement CreateSoundDataList()
    {
        var clip_count_field = new IntegerField("Clip Count");
        clip_count_field.RegisterCallback<ChangeEvent<int>>(change =>
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;

            var value = change.newValue <= 20 ? change.newValue : 20;

            if (_soundDataList != null)
            {
                _soundDataList.ForEach(data => rootVisualElement.Remove(data));
                _soundDataList = null;
                _createSoundDataList = null;
            }

            List<SoundAddress> sound_address_popup_list = new List<SoundAddress>();
            for (int index = 0; index < (int)SoundAddress.Length; index++)
            {
                sound_address_popup_list.Add((SoundAddress)index);
            }

            var list = new List<VisualElement>();
            _createSoundDataList = new List<CreateData>();
            for (int index = 0; index < value; index++)
            {
                var element = new VisualElement();
                element.style.flexDirection = FlexDirection.Row;
                var data = new CreateData();

                var sound_address_popup = new PopupField<SoundAddress>("Sound Address", sound_address_popup_list, 0);
                sound_address_popup.style.flexGrow = 1;
                sound_address_popup.RegisterCallback<ChangeEvent<SoundAddress>>(change => data.Address = change.newValue);

                var sound_clip = new ObjectField($"Clip_{index + 1}");
                sound_clip.objectType = typeof(AudioClip);
                sound_clip.style.flexGrow = 1;
                sound_clip.RegisterCallback<ChangeEvent<AudioClip>>(change => data.Clip = change.newValue);

                element.Add(sound_address_popup);
                element.Add(sound_clip);

                rootVisualElement.Add(element);
                list.Add(element);
                _createSoundDataList.Add(data);
            }

            _soundDataList = list;
        });

        return clip_count_field;
    }

    //==========================================================
    // unity method
    //==========================================================

    private void OnEnable()
    {
        minSize = WINDOW_SIZE;
        maxSize = WINDOW_SIZE;

        rootVisualElement.Add(CreateButton());
        rootVisualElement.Add(CreateText());
        rootVisualElement.Add(CreateSoundTypePopup());
        rootVisualElement.Add(CreateSoundDataList());
    }

    private void OnDestroy()
    {
        s_IsOpen = false;
    }
}
