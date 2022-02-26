using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Utility
{
    [Serializable]
    /// <summary>
    /// 天赋学习消耗
    /// </summary>
    public struct TalentCost
    {
        public int level;
        public int cost;
    }

    public enum TalentType
    {
            None,
            AddAttack,
            AddAttackSpeed,
            AddCritRank,
            Furious,
            AddHP,
            AddDefence,
            AddDodgeRank,
            SuckBlood,
            AddExperienceGet,
            AddMoveSpeed,
            AddMoneyGet,
            AddSpirtualSourceRank,
            Parry
    }

    /// <summary>
    /// 天赋属性
    /// </summary>

    [Serializable]
    public class TalentItem
    {
        public string Name;

        public int ID;

        public int[] BindIDs;

        public Sprite TalentIcon;

        public List<TalentCost> talenCosts;

        public TalentType talentType;

        public string[] Descritions;

        [Header("是否直接解锁")]
        public bool IsInitial = false;

        [Header("能不能生成在游戏中")]
        public bool canPicked = false;

        [Header("这个天赋/符文之力用于远程还是近战")]
        public DashDirectionEnum directionEnum;

         
    }

}
