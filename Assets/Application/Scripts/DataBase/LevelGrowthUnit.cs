using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HTLibrary.Application
{
    /// <summary>
    /// 成长属性
    /// </summary>
    [Serializable]
    public class LevelGrowthUnit
    {
        public string unitName;

        public int Level;

        public float HP;

        public float Attack;

        public float Defence;

        public float CritRank;

        public float CritMultiple;

        public float Dodge;

        public float MoveSpeed;

        public float AttackSpeed;
    }

}
