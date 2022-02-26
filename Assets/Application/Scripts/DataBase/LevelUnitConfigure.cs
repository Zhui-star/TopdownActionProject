using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Application;
using HTLibrary.Framework;
using NaughtyAttributes;
namespace HTLibrary.Utility
{
    public class LevelUnitConfigure :ScriptableObject
    {
        /// <summary>
        /// 关卡管理
        /// </summary>
        [ReorderableList]
        public List<LevelUnit> LevelUnitList = new List<LevelUnit>();
        
    }

}
