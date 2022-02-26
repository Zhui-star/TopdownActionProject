using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace HTLibrary.Utility
{
    public class ItemList : ScriptableObject
    {
        /// <summary>
        /// 配置文件列表
        /// </summary>
        [ReorderableList]
        public List<Item> itemList;
    }

}
