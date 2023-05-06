using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

public class DeleteSoundDataAssetWindow : EditorWindow
{
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

    private readonly Vector2 WINDOW_SIZE = new Vector2(300, 150);

    private string _fileName;

    List<Data> _dataList;

    internal static void Open()
    {
        var window = GetWindow<DeleteSoundDataAssetWindow>();
        window.titleContent = new GUIContent(nameof(DeleteSoundDataAssetWindow));
        window.Show();
    }

    private void DeleteDataAsset(string guid)
    {
        var path = AssetDatabase.GUIDToAssetPath(guid);
        
        AssetDatabase.DeleteAsset(path);
    }

    private void ApplyDataAsset(string guid)
    {
        var data_asset_text_file_path = AssetDatabase.GUIDToAssetPath(Editor.Sound.DATA_ASSET_TEXT_FILE_GUID);

        var string_builder = new System.Text.StringBuilder();
        _dataList.ForEach(data => 
        {
            if (data.GUID != guid) string_builder.Append($"{data.GUID}:{data.FileName},\n");
        });

        var encode = System.Text.Encoding.GetEncoding("UTF-8");
        using (var stream_writer = new StreamWriter(data_asset_text_file_path, false, encode))
        {
            stream_writer.Write(string_builder.ToString());
        }

        AssetDatabase.Refresh();
    }

    private void OnEnable()
    {
        _dataList = new List<Data>();

        minSize = WINDOW_SIZE;
        maxSize = WINDOW_SIZE;

        var data_asset_text_file_path = AssetDatabase.GUIDToAssetPath(Editor.Sound.DATA_ASSET_TEXT_FILE_GUID);
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

        var label = new Label("Sound Data Asset Name");
        
        var file_name_list = new List<string>();
        _dataList.ForEach(data => file_name_list.Add(data.FileName));

        if (file_name_list.Count == 0)
        {
            file_name_list.Add("");
        }

        _fileName = file_name_list[0]; 
        var popup = new PopupField<string>(file_name_list, _fileName);
        popup.RegisterCallback<ChangeEvent<string>>(callback => _fileName = callback.newValue);

        var button = new Button(() => 
        {
            try
            {
                var guid = _dataList.Find(data => data.FileName == _fileName).GUID;
                DeleteDataAsset(guid);
                ApplyDataAsset(guid);
            }
            catch
            {
                Debug.Log("FileNameを指定している場合ここでエラーが起きるのはおかしいです。作業を初期化してください");
            }
        });
        button.text = "Delete Sound Data Asset";

        rootVisualElement.Add(label);
        rootVisualElement.Add(popup);
        rootVisualElement.Add(new Label("\n\n\n\n\n"));
        rootVisualElement.Add(button);
    }
}
