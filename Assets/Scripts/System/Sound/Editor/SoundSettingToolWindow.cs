using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class SoundSettingToolWindow : EditorWindow
{
    private readonly Vector2 WINDOW_SIZE = new Vector2(300, 150);

    [MenuItem("Tools/Sound/Setting")]
    private static void Open()
    {
        var window = GetWindow<SoundSettingToolWindow>();
        window.titleContent = new GUIContent(nameof(SoundSettingToolWindow));
        window.Show();
    }

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

    private void CreateDataAssetEditWindow()
    {
        var edit_button = new Button(() => EditSoundDataAssetWindow.Open());
        edit_button.text = "Edit Data Asset Window";
        
        rootVisualElement.Add(new Label(" Edit\n"));
        rootVisualElement.Add(edit_button);
    }

    private void OnEnable()
    {
        minSize = WINDOW_SIZE;
        maxSize = WINDOW_SIZE;

        CreateDataAssetManageWindow();
        rootVisualElement.Add(new Label("=========================================="));
        CreateDataAssetEditWindow();
    }
}
