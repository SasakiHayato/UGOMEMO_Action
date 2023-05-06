using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

/// <summary>
/// サウンドデータアセットの編集Windowクラス
/// </summary>
public class EditSoundDataAssetWindow : EditorWindow
{
    //==========================================================================
    // private class
    //==========================================================================

    /// <summary>
    /// Window表示中データクラス
    /// </summary>
    private class Data
    {
        public string GUID { get; }
        public string FileName { get; }

        public Data(string guid, string file_name)
        {
            GUID = guid;
            FileName = file_name;
        }
    }

    //==========================================================================
    // variable
    //==========================================================================

    private bool _canUpdate;

    private Editor _editor;
    private SoundDataAsset _asset;
    private List<Data> _dataList;

    //==========================================================================
    // internal mathod
    //==========================================================================

    /// <summary>
    /// 開く際の処理
    /// 
    /// </summary>
    internal static void Open()
    {
        var window = GetWindow<EditSoundDataAssetWindow>();
        window.titleContent = new GUIContent(nameof(EditSoundDataAssetWindow));
        window.Show();
    }

    //==========================================================================
    // private method
    //==========================================================================

    /// <summary>
    /// 編集するサウンドデータアセットの立ち上げ
    /// </summary>
    /// <param name="guid"></param>
    private void SetupEditor(string guid)
    {
        _canUpdate = false;

        var path = AssetDatabase.GUIDToAssetPath(guid);
        _asset = AssetDatabase.LoadAssetAtPath<SoundDataAsset>(path);

        if (EditorGUI.EndChangeCheck()) _editor = Editor.CreateEditor(_asset);
        if (_editor == null) return;

        _canUpdate = true;
    }

    /// <summary>
    /// 保存処理
    /// </summary>
    private void Save()
    {
        if (_asset == null) return;

        EditorUtility.SetDirty(_asset);
        _asset.Save();
        AssetDatabase.SaveAssets();
    }

    //==========================================================================
    // unity method
    //==========================================================================

    private void OnEnable()
    {
        _dataList = new List<Data>();

        var toolbar = new Toolbar();

        var data_asset_text_file_path = AssetDatabase.GUIDToAssetPath(EditorDefine.Sound.DATA_ASSET_TEXT_FILE_GUID);
        var data_asset_text_file = AssetDatabase.LoadAssetAtPath<TextAsset>(data_asset_text_file_path);

        if (data_asset_text_file.text != null)
        {
            data_asset_text_file.text
            .Split(",\n")
            .Where(data => data != "")
            .ToList()
            .ForEach(data_asset =>
            {
                var data = data_asset.Split(':').Where(data => data != "").ToArray();
                _dataList.Add(new Data(data[0], data[1]));
            });
        }

        var option_popup = new ToolbarMenu { text = "Option", variant = ToolbarMenu.Variant.Popup };
        var data_asset_popup = new ToolbarMenu { text = "Sound Data Asset", variant = ToolbarMenu.Variant.Popup };

        option_popup.menu.AppendAction("Save", callback => Save(), status => DropdownMenuAction.Status.Normal);

        _dataList.ForEach(data => 
        {
            data_asset_popup.menu.AppendAction(data.FileName, callbak => SetupEditor(data.GUID), status => DropdownMenuAction.Status.Normal);
        });

        toolbar.Add(data_asset_popup);
        toolbar.Add(option_popup);

        rootVisualElement.Add(toolbar);
    }

    private void OnGUI()
    {
        if (!_canUpdate) return;

        EditorGUI.BeginChangeCheck();
        _editor.OnInspectorGUI();
    }
}
