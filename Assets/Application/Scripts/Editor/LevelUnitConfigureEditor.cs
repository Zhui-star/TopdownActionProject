using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HTLibrary.Application;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Editor
{
    public class LevelUnitConfigureEditor : MonoBehaviour
    {
        [MenuItem("HTLibrary/LevelUnitList")]
        static void CreateLevelUnitList()
        {
           LevelUnitConfigure  levelunit = ScriptableObject.CreateInstance<LevelUnitConfigure>();

            string path = "Assets/" + "LevelUnitConfigure.asset";

            AssetDatabase.CreateAsset(levelunit, path);

            AssetDatabase.SaveAssets();
        }
    }

}
