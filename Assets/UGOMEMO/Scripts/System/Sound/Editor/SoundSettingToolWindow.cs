using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

/// <summary>
/// サウンドデータの設定Windowクラス
/// </summary>
public class SoundSettingToolWindow : EditorWindow
{
    //====================================================================
    // constant
    //====================================================================

    private readonly Vector2 WINDOW_SIZE = new Vector2(300, 150);

    //====================================================================
    // private method
    //====================================================================

    /// <summary>
    /// Unity側から開く際の処理
    /// </summary>
    [MenuItem("Tools/Sound/Setting")]
    private static void Open()
    {
        var window = GetWindow<SoundSettingToolWindow>();
        window.titleContent = new GUIContent(nameof(SoundSettingToolWindow));
        window.Show();
    }

    /// <summary>
    /// サウンドデータアセットの生成、破棄用Windowの生成
    /// </summary>
    private void CreateDataAssetManageWindow()
    {
        var generate_button = new Button(() => GenerateSoundDataAssetWindow.Open());
        generate_button.text = "Generate Data Asset Window";

        var delete_button = new Button(() => DeleteSoundDataAssetWindow.Open());
        delete_button.text = "Delete Data Asset Window";

        rootVisualElement.Add(new Label(" Manage\n"));
        rootVisualElement.Add(generate_button);
        rootVisualElement.Add(delete_button);
    }

    /// <summary>
    /// サウンドデータアセットの編集用Windowの生成
    /// </summary>
    private void CreateDataAssetEditWindow()
    {
        var edit_button = new Button(() => EditSoundDataAssetWindow.Open());
        edit_button.text = "Edit Data Asset Window";
        
        rootVisualElement.Add(new Label(" Edit\n"));
        rootVisualElement.Add(edit_button);
    }

    //====================================================================
    // unity method
    //====================================================================

    private void OnEnable()
    {
        minSize = WINDOW_SIZE;
        maxSize = WINDOW_SIZE;

        CreateDataAssetManageWindow();
        rootVisualElement.Add(new Label("=========================================="));
        CreateDataAssetEditWindow();
    }
}
