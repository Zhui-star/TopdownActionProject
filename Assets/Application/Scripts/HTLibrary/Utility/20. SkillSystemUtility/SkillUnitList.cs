using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace HTLibrary.Utility
{
    public class SkillUnitList :ScriptableObject
    {
        [ReorderableList]
        public List<SkillUnit> skillList;
    }

}
