using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�`�[�u�����g�f�[�^�N���X
/// </summary>
public class AchievementDataAsset : ScriptableObject
{
    //====================================================================
    // private class
    //====================================================================

    /// <summary>
    /// �f�[�^�N���X
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
        /// ������
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
    /// �f�[�^�̒ǉ�
    /// Editor�p�Ȃ̂ŁA������͌Ă΂Ȃ�
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
