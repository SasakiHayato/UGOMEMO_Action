using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アチーブメントデータクラス
/// </summary>
public class AchievementDataAsset : ScriptableObject
{
    //====================================================================
    // private class
    //====================================================================

    [System.Serializable]
    private class Data
    {
        [SerializeField, ReadOnly]
        private Achievement_Enum _achievementType;

        [SerializeField]
        private List<AchievementData> _achivementDataList;

        public Data(Achievement_Enum achievement_type, List<AchievementData> achievement_data_list)
        {
            _achievementType = achievement_type;
            _achivementDataList = achievement_data_list;
        }
    }

    [System.Serializable]
    private class AchievementData
    {
        [SerializeField, ReadOnly]
        private int _rank;

        [SerializeField, ReadOnly]
        private string _text;

        [SerializeReference, HideInInspector]
        private readonly IAchievement _achievement;

        public AchievementData(int rank, string text, IAchievement achievement)
        {
            _rank = rank;
            _text = text;
            _achievement = achievement;
        }
    }

    //====================================================================
    // variable
    //====================================================================

    [SerializeField]
    private List<Data> _dataList;
}
