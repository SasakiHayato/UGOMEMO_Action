using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

/// <summary>
/// サウンドデータのアドレス管理Window
/// </summary>
public class CreateSoundAddressToolWindow : EditorWindow
{
    //==========================================================
    // constant
    //==========================================================

    private readonly Vector2 WINDOW_SIZE = new Vector2(300, 500);

    //==========================================================
    // variable
    //==========================================================

    private static bool s_isOpen;

    //==========================================================
    // ptivate method
    //==========================================================

    /// <summary>
    /// Windowを開く際の処理
    /// </summary>
    [MenuItem("Tools/Sound/CreateAddress")]
    private static void Open()
    {
        if (s_isOpen) return;
        s_isOpen = true;

        var window = GetWindow<CreateSoundAddressToolWindow>();
        window.titleContent = new GUIContent("SoundCreateAddressTool");
        window.Show();
    }

    /// <summary>
    /// アドレスの発行
    /// </summary>
    private void AddAddress()
    {
        string address_text_path = AssetDatabase.GUIDToAssetPath(Define.Sound.ADDRESS_TEXT_GUID);
        var address_asset = AssetDatabase.LoadAssetAtPath<TextAsset>(address_text_path);

        var address_asset_clip_list = address_asset.text
            .Split(",\n")
            .Where(clip => clip != "")
            .ToList();

        string clip_path = AssetDatabase.GUIDToAssetPath(Define.Sound.CLIP_DIRECTORY_GUID);
        var clip_file_append_list = Directory
            .GetFiles(clip_path, "*", SearchOption.AllDirectories)
            .Where(clip => !clip.Contains(".meta"))
            .Select(clip => Path.GetFileNameWithoutExtension(clip))
            .Where(clip => address_asset_clip_list.All(address_clip => clip != address_clip))
            .ToList();

        clip_file_append_list.ForEach(clip => rootVisualElement.Add(new Label($"Add                        {clip}")));

        using (var stream_writer = new StreamWriter(address_text_path, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            var string_builder = new System.Text.StringBuilder();
            address_asset_clip_list.ForEach(clip => string_builder.Append($"{clip},\n"));
            clip_file_append_list.ForEach(clip => string_builder.Append($"{clip},\n"));

            stream_writer.Write(string_builder.ToString());
        }

        AssetDatabase.Refresh();
        rootVisualElement.Add(new Label("Complete"));
        rootVisualElement.Add(CreateButton("Apply Csharp", () => ApplyAddress()));
    }

    /// <summary>
    /// アドレスの反映
    /// </summary>
    private void ApplyAddress()
    {
        string address_text_path = AssetDatabase.GUIDToAssetPath(Define.Sound.ADDRESS_TEXT_GUID);
        var address_asset = AssetDatabase.LoadAssetAtPath<TextAsset>(address_text_path);
        var address_list = address_asset.text
            .Split(",\n")
            .Where(address => address != "")
            .ToList();

        string sound_address_path = AssetDatabase.GUIDToAssetPath(Define.Sound.SOUND_ADDRESS_GUID);

        using (var stream_witer = new StreamWriter(sound_address_path, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            var string_builder = new System.Text.StringBuilder();
            string_builder.Append("//===================================================================\n");
            string_builder.Append("// 自動生成されたスクリプト\n");
            string_builder.Append("//===================================================================\n");
            string_builder.Append("\n");
            string_builder.Append("/// <summary>\n");
            string_builder.Append("/// サウンドデータのアドレス\n");
            string_builder.Append("/// </summary>\n");
            string_builder.Append("public enum SoundAddress\n");
            string_builder.Append("{\n");
            address_list.ForEach(address => string_builder.Append($"    {address},\n"));
            string_builder.Append("\n");
            string_builder.Append("    None,\n");
            string_builder.Append("\n");
            string_builder.Append("    Length\n");
            string_builder.Append("}\n");

            stream_witer.Write(string_builder.ToString());
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// アドレスの削除
    /// </summary>
    private void DeleteAddressText()
    {
        string address_text_path = AssetDatabase.GUIDToAssetPath(Define.Sound.ADDRESS_TEXT_GUID);

        using (var stream_writer = new StreamWriter(address_text_path, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            stream_writer.Write("");
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 反映されているアドレスの削除
    /// </summary>
    private void DeleteAddressCsharp()
    {
        string sound_address_path = AssetDatabase.GUIDToAssetPath(Define.Sound.SOUND_ADDRESS_GUID);

        using (var stream_witer = new StreamWriter(sound_address_path, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            var string_builder = new System.Text.StringBuilder();
            string_builder.Append("//===================================================================\n");
            string_builder.Append("// 自動生成されたスクリプト\n");
            string_builder.Append("//===================================================================\n");
            string_builder.Append("\n");
            string_builder.Append("/// <summary>\n");
            string_builder.Append("/// サウンドデータのアドレス\n");
            string_builder.Append("/// </summary>\n");
            string_builder.Append("public enum SoundAddress\n");
            string_builder.Append("{\n");
            string_builder.Append("    None,\n");
            string_builder.Append("\n");
            string_builder.Append("    Length\n");
            string_builder.Append("}\n");

            stream_witer.Write(string_builder.ToString());
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// ボタンの生成
    /// </summary>
    /// <param name="text"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private VisualElement CreateButton(string text, System.Action callback)
    {
        var button = new Button(callback);
        button.text = text;

        return button;
    }

    //==========================================================
    // unity method
    //==========================================================

    private void OnEnable()
    {
        minSize = WINDOW_SIZE;
        maxSize = WINDOW_SIZE;

        rootVisualElement.Add(CreateButton("Add Address", () => AddAddress()));
        rootVisualElement.Add(CreateButton("Delete Address Text", () => DeleteAddressText()));
        rootVisualElement.Add(CreateButton("Delete Address Csharp", () => DeleteAddressCsharp()));
    }

    private void OnDestroy()
    {
        s_isOpen = false;
    }
}
