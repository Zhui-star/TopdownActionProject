using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 合成材料表生成器
    /// </summary>
    public class FormularConfigure : ScriptableObject
    {
        [ReorderableList]
        public List<Formular> formulars = new List<Formular>();
    }

}
