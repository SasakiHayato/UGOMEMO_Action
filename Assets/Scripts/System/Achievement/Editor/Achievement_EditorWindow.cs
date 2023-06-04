using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Text;

/// <summary>
/// �A�`�[�u�����gWindow
/// </summary>
public class Achievement_EditorWindow : EditorWindow, IGSSWebReceiver
{
    private enum UpdateType
    {
        DataAsset,
        Enum,
    }

    //==============================================================================
    // constant
    //==============================================================================

    private const string DATA_ASSET_DIRECTORY_PATH = "Assets/MasterData/Achievement/";
    private const string DATA_ASSET_FILE_PATH = "Achievement_DataAsset";

    private const string AHIVEMENT_ENUM_FILE_GUID = "043dd8dc64a84f544bf6def872ee9e3e";

    private UpdateType _updateType;

    //==============================================================================
    // static method - public
    //==============================================================================

    /// <summary>
    /// �c�[���o�[����J���ۂ̏���
    /// </summary>
    [MenuItem("Tools/Achievement")]
    public static void Open()
    {
        var window = GetWindow<Achievement_EditorWindow>();
        window.titleContent = new GUIContent(nameof(Achievement_EditorWindow));
        window.Show();
    }

    //==============================================================================
    // private method
    //==============================================================================

    /// <summary>
    /// �A�Z�b�g�����{�^���̐���
    /// </summary>
    /// <returns></returns>
    private VisualElement CreateAssetButton()
    {
        var create_asset_button = new Button();
        create_asset_button.text = "�A�Z�b�g�̐���";

        create_asset_button.clicked += () => 
        {
            Debug.Log("�A�Z�b�g�𐶐����܂�");
            if (Directory.Exists(DATA_ASSET_DIRECTORY_PATH))
            {
                if (AssetDatabase.FindAssets(DATA_ASSET_FILE_PATH).Length > 0) return;
                CreateAsset();
                Debug.Log("�A�Z�b�g�𐶐����܂���");
                return;
            }
            else
            {
                Directory.CreateDirectory(DATA_ASSET_DIRECTORY_PATH);
                CreateAsset();
                Debug.Log("�A�Z�b�g�𐶐����܂���");
                return;
            }
        };

        return create_asset_button;
    }

    /// <summary>
    /// �X�V�{�^���̐���
    /// </summary>
    /// <returns></returns>
    private VisualElement UpdateAssetButton()
    {
        var update_asset_button = new Button();
        update_asset_button.text = "�A�Z�b�g�̍X�V";

        update_asset_button.clicked += () => 
        {
            Debug.Log("�A�Z�b�g���X�V���܂�");
            _updateType = UpdateType.DataAsset;
            var requester = new GSSWebRequester(this, GSS_Sheet.Achievement);
            requester.Request();
        };

        return update_asset_button;
    }

    private VisualElement UpdateAchievementEnumButton()
    {
        var update_achievement_button = new Button();
        update_achievement_button.text = "�A�`�[�u�����gEnum�̍X�V";
        update_achievement_button.clicked += () => 
        {
            Debug.Log("Enum ���X�V���܂�");
            _updateType = UpdateType.Enum;
            var requester = new GSSWebRequester(this, GSS_Sheet.Achievement);
            requester.Request();
        };

        return update_achievement_button;
    }

    /// <summary>
    /// �A�Z�b�g�̍쐬
    /// </summary>
    private void CreateAsset()
    {
        var asset = CreateInstance<AchievementDataAsset>();
        AssetDatabase.CreateAsset(asset, Path.Combine(DATA_ASSET_DIRECTORY_PATH, DATA_ASSET_FILE_PATH + ".asset"));
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// �f�[�^�̔��f
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="model"></param>
    private void ApplyData(AchievementDataAsset asset, AchievementJsonModel model)
    {
        var achievement_list = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(assembly => assembly.GetInterface(nameof(IAchievement)) is not null)
            .Select(assembly => System.Activator.CreateInstance(assembly) as IAchievement)
            .Where(assembly => assembly is not null)
            .ToList();
        
        foreach (var data in model.Data)
        {
            var action = (Achievement_Enum)Enum.Parse(typeof(Achievement_Enum), data.Action);
            var achievement = achievement_list.Find(achievement => achievement.Type == action);
            asset.AddData(action, data.Rank, data.Text, data.Value, achievement);
        }

        Debug.Log("�A�Z�b�g�𔽉f���܂���");
    }

    /// <summary>
    /// Enum �̍X�V
    /// </summary>
    /// <param name="model"></param>
    private void UpdateAchivementEnum(AchievementJsonModel model)
    {
        var string_list = new List<string>();
        foreach (var data in model.Data) string_list.Add(data.Action);
        string_list = string_list.Distinct().ToList();

        ApplyAchivementEnum(string_list);

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Enum �̔��f
    /// </summary>
    /// <param name="list"></param>
    private void ApplyAchivementEnum(List<string> list)
    {
        var path = AssetDatabase.GUIDToAssetPath(AHIVEMENT_ENUM_FILE_GUID);

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
        {
            var string_builder = new StringBuilder();
            string_builder.Append("//===================================================================\n");
            string_builder.Append("// �����������ꂽ�X�N���v�g\n");
            string_builder.Append("//===================================================================\n");
            string_builder.Append("\n");
            string_builder.Append($"public enum {nameof(Achievement_Enum)}\n");
            string_builder.Append("{\n");
            list.ForEach(str => string_builder.Append($"    {str},\n"));
            string_builder.Append("}\n");

            writer.Write(string_builder.ToString());
        }
    }

    //==============================================================================
    // IGSSWebReceive interface
    //==============================================================================

    /// <summary>
    /// GSS �����M�����ۂ̏���
    /// </summary>
    /// <param name="download">�󂯎�����f�[�^</param>
    void IGSSWebReceiver.Receive(DownloadHandler download)
    {
        Debug.Log("GSS ����f�[�^��M���܂���");
        var assets = AssetDatabase.FindAssets(DATA_ASSET_FILE_PATH);
        if (assets.Length <= 0) return;
        string path = AssetDatabase.GUIDToAssetPath(assets[0]);
        var asset = AssetDatabase.LoadAssetAtPath<AchievementDataAsset>(path);

        var model = JsonUtility.FromJson<AchievementJsonModel>(download.text);

        switch (_updateType)
        {
            case UpdateType.DataAsset: ApplyData(asset, model); return;
            case UpdateType.Enum: UpdateAchivementEnum(model);  return;
        }
    }

    //==============================================================================
    // unity method
    //==============================================================================

    private void OnEnable()
    {
        rootVisualElement.Add(CreateAssetButton());
        rootVisualElement.Add(UpdateAssetButton());
        rootVisualElement.Add(UpdateAchievementEnumButton());
    }
}
