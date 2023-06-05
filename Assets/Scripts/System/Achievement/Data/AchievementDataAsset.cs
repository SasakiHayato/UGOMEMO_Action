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

    /// <summary>
    /// データクラス
    /// </summary>
    [System.Serializable]
    private class AchievementData
    {
        //====================================================================
        // variable - AchivementData
        //====================================================================

        [SerializeField, ReadOnly]
        private Achievement_Enum _achievementType;

        [SerializeField, ReadOnly]
        private int _rank;

        [SerializeField, ReadOnly]
        private string _text;

        [SerializeField, HideInInspector]
        private int _value;

        [SerializeReference, HideInInspector]
        private readonly IAchievement _achievement;

        //====================================================================
        // constructor
        //====================================================================

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="achievement_enum"></param>
        /// <param name="rank"></param>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <param name="achievement"></param>
        public AchievementData(Achievement_Enum achievement_enum, int rank, string text, int value, IAchievement achievement)
        {
            _achievementType = achievement_enum;
            _rank = rank;
            _text = text;
            _value = value;
            _achievement = achievement;
        }
    }

    //====================================================================
    // variable
    //====================================================================

    [SerializeField]
    private List<AchievementData> _dataList;

#if UNITY_EDITOR
    /// <summary>
    /// データの追加
    /// Editor用なので、他からは呼ばない
    /// </summary>
    /// <param name="achievement_enum"></param>
    /// <param name="rank"></param>
    /// <param name="text"></param>
    /// <param name="value"></param>
    /// <param name="achievement"></param>
    public void AddData(Achievement_Enum achievement_enum, int rank, string text, int value, IAchievement achievement)
    {
        if (_dataList == null) _dataList = new List<AchievementData>();
        _dataList.Add(new AchievementData(achievement_enum, rank, text, value, achievement));
    }
# endif
}
