using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

/// <summary>
/// サウンドアドレスの発行、破棄Windowクラス
/// </summary>
public class SoundAddressToolWindow : EditorWindow
{
    //==========================================================================
    // constant
    //==========================================================================

    private readonly string GENERATE_BUTTON_NAME = "Generate Address";
    private readonly string DELETE_BUTTON_NAME = "Delete Address";

    //==========================================================================
    // private method
    //==========================================================================

    /// <summary>
    /// Unity側から開く際の処理
    /// </summary>
    [MenuItem("Tools/Sound/AddressTool")]
    private static void Open()
    {
        var window = GetWindow<SoundAddressToolWindow>();
        window.titleContent = new GUIContent(nameof(SoundAddressToolWindow));
        window.Show();
    }

    /// <summary>
    /// アドレスの発行
    /// </summary>
    private void GenerateAddress()
    {
        var address_text_asset_path = AssetDatabase.GUIDToAssetPath(SoundEditorDefine.ADDRESS_TEXT_FILE_GUID);
        var address_text_asset = AssetDatabase.LoadAssetAtPath<TextAsset>(address_text_asset_path);

        var address_list = address_text_asset.text.Split(",\n").Where(address => address != "");

        var clip_directory_path = AssetDatabase.GUIDToAssetPath(SoundEditorDefine.CLIP_DIRECTORY_GUID);
        var string_builder = new StringBuilder();
        var clip_address_list = Directory
            .GetFiles(clip_directory_path, "*", SearchOption.AllDirectories)
            .Where(clip => !clip.Contains(".meta"))
            .Select(clip => Path.GetFileNameWithoutExtension(clip))
            .Where(clip => !address_list.Any(address => address == clip))
            .ToList();

        var encode = Encoding.GetEncoding("UTF-8");
        using (var stream_writer = new StreamWriter(address_text_asset_path, true, encode))
        {
            clip_address_list.ForEach(clip => 
            {
                string_builder.Append($"{clip},\n");
                rootVisualElement.Add(new Label($"Add               {clip}"));
            });
            stream_writer.Write(string_builder.ToString());
        }
            

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// アドレスの破棄
    /// </summary>
    private void DeleteAddress()
    {
        var address_text_asset_path = AssetDatabase.GUIDToAssetPath(SoundEditorDefine.ADDRESS_TEXT_FILE_GUID);
        var address_text_asset = AssetDatabase.LoadAssetAtPath<TextAsset>(address_text_asset_path);

        var address_list = address_text_asset.text.Split(",\n")
            .Where(address => address != "")
            .ToList();

        address_list.ForEach(address => rootVisualElement.Add(new Label($"Delete         {address}")));

        var encode = Encoding.GetEncoding("UTF-8");
        using (var stream_writer = new StreamWriter(address_text_asset_path, false, encode))
            stream_writer.Write("");

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Editorの更新
    /// </summary>
    private void ApplyAddress()
    {
        var address_text_file_path = AssetDatabase.GUIDToAssetPath(SoundEditorDefine.ADDRESS_TEXT_FILE_GUID);
        var address_text_file = AssetDatabase.LoadAssetAtPath<TextAsset>(address_text_file_path);
        var address_text_fli_list = address_text_file.text.Split(",\n").Where(address => address != "").ToList();

        var address_csharp_path = AssetDatabase.GUIDToAssetPath(SoundEditorDefine.ADDRESS_CSHARP_GUID);

        var encode = Encoding.GetEncoding("UTF-8"); 
        using (var stream_writer = new StreamWriter(address_csharp_path, false, encode))
        {
            var string_builder = new StringBuilder();
            
            string_builder.Append("//===================================================================\n");
            string_builder.Append("// 自動生成されたスクリプト\n");
            string_builder.Append("//===================================================================\n");

            string_builder.Append("\n");

            string_builder.Append("/// <summary>\n");
            string_builder.Append("/// サウンドデータのアドレス\n");
            string_builder.Append("/// <summary>\n");

            string_builder.Append("public enum SoundAddress\n");
            string_builder.Append("{\n");

            if (address_text_fli_list.Count > 0)
            {
                address_text_fli_list.ForEach(address => string_builder.Append($"    {address},\n"));
            }

            string_builder.Append("    None,\n");
            string_builder.Append("\n");
            string_builder.Append("    Length\n");

            string_builder.Append("}\n");

            stream_writer.Write(string_builder.ToString());
        }

        AssetDatabase.Refresh();
    }

    //==========================================================================
    // unity method
    //==========================================================================

    private void OnEnable()
    {
        var generate_address_button = new Button(() => 
        {
            GenerateAddress();
            ApplyAddress();
        });
        generate_address_button.text = GENERATE_BUTTON_NAME;

        var delete_address_button = new Button(() =>
        {
            DeleteAddress();
            ApplyAddress();
        });
        delete_address_button.text = DELETE_BUTTON_NAME;

        rootVisualElement.Add(generate_address_button);
        rootVisualElement.Add(delete_address_button);
    }
}
