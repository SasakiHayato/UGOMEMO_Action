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
/// アチーブメントWindow
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
    /// ツールバーから開く際の処理
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
    /// アセット生成ボタンの生成
    /// </summary>
    /// <returns></returns>
    private VisualElement CreateAssetButton()
    {
        var create_asset_button = new Button();
        create_asset_button.text = "アセットの生成";

        create_asset_button.clicked += () => 
        {
            Debug.Log("アセットを生成します");
            if (Directory.Exists(DATA_ASSET_DIRECTORY_PATH))
            {
                if (AssetDatabase.FindAssets(DATA_ASSET_FILE_PATH).Length > 0) return;
                CreateAsset();
                Debug.Log("アセットを生成しました");
                return;
            }
            else
            {
                Directory.CreateDirectory(DATA_ASSET_DIRECTORY_PATH);
                CreateAsset();
                Debug.Log("アセットを生成しました");
                return;
            }
        };

        return create_asset_button;
    }

    /// <summary>
    /// 更新ボタンの生成
    /// </summary>
    /// <returns></returns>
    private VisualElement UpdateAssetButton()
    {
        var update_asset_button = new Button();
        update_asset_button.text = "アセットの更新";

        update_asset_button.clicked += () => 
        {
            Debug.Log("アセットを更新します");
            _updateType = UpdateType.DataAsset;
            var requester = new GSSWebRequester(this, GSS_Sheet.Achievement);
            requester.Request();
        };

        return update_asset_button;
    }

    private VisualElement UpdateAchievementEnumButton()
    {
        var update_achievement_button = new Button();
        update_achievement_button.text = "アチーブメントEnumの更新";
        update_achievement_button.clicked += () => 
        {
            Debug.Log("Enum を更新します");
            _updateType = UpdateType.Enum;
            var requester = new GSSWebRequester(this, GSS_Sheet.Achievement);
            requester.Request();
        };

        return update_achievement_button;
    }

    /// <summary>
    /// アセットの作成
    /// </summary>
    private void CreateAsset()
    {
        var asset = CreateInstance<AchievementDataAsset>();
        AssetDatabase.CreateAsset(asset, Path.Combine(DATA_ASSET_DIRECTORY_PATH, DATA_ASSET_FILE_PATH + ".asset"));
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// データの反映
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

        Debug.Log("アセットを反映しました");
    }

    /// <summary>
    /// Enum の更新
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
    /// Enum の反映
    /// </summary>
    /// <param name="list"></param>
    private void ApplyAchivementEnum(List<string> list)
    {
        var path = AssetDatabase.GUIDToAssetPath(AHIVEMENT_ENUM_FILE_GUID);

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
        {
            var string_builder = new StringBuilder();
            string_builder.Append("//===================================================================\n");
            string_builder.Append("// 自動生成されたスクリプト\n");
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
    /// GSS から受信した際の処理
    /// </summary>
    /// <param name="download">受け取ったデータ</param>
    void IGSSWebReceiver.Receive(DownloadHandler download)
    {
        Debug.Log("GSS からデータ受信しました");
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
