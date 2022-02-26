using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HTLibrary.Utility;
namespace HTLibrary.Editor
{
    /// <summary>
    /// 配方容器生成器
    /// </summary>
    public class FormularEditor 
    {
        [MenuItem("HTLibrary/Make formular list",false,55)]
        static void MakeFormularList()
        {
            FormularConfigure _object= ScriptableObject.CreateInstance<FormularConfigure>();

            string path = "Assets/" + "FormularList.asset";

            AssetDatabase.CreateAsset(_object, path);

            AssetDatabase.SaveAssets();

        }
    }

}
