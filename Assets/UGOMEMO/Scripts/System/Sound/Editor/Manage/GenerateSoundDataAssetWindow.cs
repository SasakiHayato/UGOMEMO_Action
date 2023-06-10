using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;


/// <summary>
/// サウンドデータアセットの生成Windowクラス
/// </summary>
public class GenerateSoundDataAssetWindow : EditorWindow
{
    //====================================================================
    // constant
    //====================================================================

    private readonly Vector2 WINDOW_SIZE = new Vector2(300, 150);

    //====================================================================
    // variable
    //====================================================================

    private string _fileName;
    private SoundType _soundType;

    //====================================================================
    // internal method
    //====================================================================

    /// <summary>
    /// 開く際の処理
    /// SoundSettingToolWindowから呼ばれるので他からは呼ばない
    /// </summary>
    internal static void Open()
    {
        var window = GetWindow<GenerateSoundDataAssetWindow>();
        window.titleContent = new GUIContent(nameof(GenerateSoundDataAssetWindow));
        window.Show();
    }

    //====================================================================
    // private method
    //====================================================================

    /// <summary>
    /// サウンドデータアセットの生成
    /// </summary>
    /// <param name="path"></param>
    private void CreateDataAsset(string path)
    {
        var asset = CreateInstance<SoundDataAsset>();
        AssetDatabase.CreateAsset(asset, path);
    }

    /// <summary>
    /// Editorの更新
    /// </summary>
    /// <param name="path"></param>
    /// <param name="file_name"></param>
    private void ApplyDataAsset(string path, string file_name)
    {
        var asset = AssetDatabase.LoadAssetAtPath<SoundDataAsset>(path);
        var guid = AssetDatabase.AssetPathToGUID(path);
        
        asset.SetGUID(guid);
        asset.SetSoundType(_soundType);

        var data_asset_file_path = AssetDatabase.GUIDToAssetPath(SoundEditorDefine.DATA_ASSET_TEXT_FILE_GUID);
        var encode = System.Text.Encoding.GetEncoding("UTF-8");
        using (var stream_writer = new StreamWriter(data_asset_file_path, true, encode))
        {
            stream_writer.Write($"{guid}:{file_name},\n");
        }

        AssetDatabase.Refresh();
    }

    //====================================================================
    // unity method
    //====================================================================

    private void OnEnable()
    {
        minSize = WINDOW_SIZE;
        maxSize = WINDOW_SIZE;



        var file_name_element = new VisualElement();
        file_name_element.style.flexDirection = FlexDirection.Row;

        var file_name_label = new Label("Data Asset File Name");
        
        var file_name_text_field = new TextField();
        file_name_text_field.RegisterCallback<ChangeEvent<string>>(callback => _fileName = callback.newValue);

        file_name_element.Add(file_name_label);
        file_name_element.Add(file_name_text_field);



        var sound_type_element = new VisualElement();
        sound_type_element.style.flexDirection = FlexDirection.Row;

        var sound_type_label = new Label("Sound Type");
        sound_type_label.style.flexGrow = 1;

        var sound_type_list = new List<SoundType>();
        for (int index = 0; index < (int)SoundType.Length; index++)
            sound_type_list.Add((SoundType)index);

        _soundType = SoundType.SE;
        var sound_type_popup = new PopupField<SoundType>(sound_type_list, _soundType);
        sound_type_popup.style.flexGrow = 1;
        sound_type_popup.RegisterCallback<ChangeEvent<SoundType>>(callback => _soundType = callback.newValue);

        sound_type_element.Add(sound_type_label);
        sound_type_element.Add(sound_type_popup);



        var generate_button = new Button(() => 
        {
            var file_name = _fileName != "" ? _fileName : "New Sound Data Asset";
            var path = SoundEditorDefine.PATH + $"{file_name}.asset";

            CreateDataAsset(path);
            ApplyDataAsset(path, file_name);
        });
        generate_button.text = "Generate Sound Data Asset";



        rootVisualElement.Add(file_name_label);
        rootVisualElement.Add(file_name_text_field);
        rootVisualElement.Add(new Label(""));
        rootVisualElement.Add(sound_type_element);
        rootVisualElement.Add(new Label("\n\n\n"));
        rootVisualElement.Add(generate_button);
    }
}
