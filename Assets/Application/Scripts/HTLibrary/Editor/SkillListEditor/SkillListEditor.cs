using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using HTLibrary.Framework;
using HTLibrary.Utility;
using UnityEngine;
namespace HTLibrary.Editor
{
    public class SkillListEditor 
    {
        [MenuItem("HTLibrary/SkillList Config")]
        static void CreateItemList()
        {
            SkillUnitList skillList = ScriptableObject.CreateInstance<SkillUnitList>();

            string path = "Assets/" + "SkillUnitList.asset";

            AssetDatabase.CreateAsset(skillList, path);

            AssetDatabase.SaveAssets();

        }
    }

}
