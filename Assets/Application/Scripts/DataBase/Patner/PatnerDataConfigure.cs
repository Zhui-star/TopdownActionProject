using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace HTLibrary.Application
{
    /// <summary>
    /// 伙伴数据库
    /// </summary>
    public class PatnerDataConfigure :ScriptableObject
    {
        [ReorderableList]
        public List<PatnerUnit> patnerDatabase = new List<PatnerUnit>();
    }

}
