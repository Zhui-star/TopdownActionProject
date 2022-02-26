using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Application;
using HTLibrary.Framework;
using System;
namespace HTLibrary.Utility {

    /// <summary>
    /// 关卡单位
    /// </summary>
    [Serializable]
    public class LevelUnit
    {
        public string scenesName;
        /// <summary>
        /// 当前关卡索引
        /// </summary>
        public int index;

        /// <summary>
        /// 下一个关卡集合
        /// </summary>
        public List<int> nextIndexs;

        /// <summary>
        /// 之前关卡索引
        /// </summary>
        public int beforeIndex;

        /// <summary>
        /// 关卡强度
        /// </summary>
        public int levelStrenth;
    }

}
