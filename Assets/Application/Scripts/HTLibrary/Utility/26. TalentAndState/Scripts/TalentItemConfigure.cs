using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 天赋单位集合
    /// </summary>
    public class TalentItemConfigure : ScriptableObject
    {
        [ReorderableList]
        public List<TalentItem> talentItems = new List<TalentItem>();
    }

}
