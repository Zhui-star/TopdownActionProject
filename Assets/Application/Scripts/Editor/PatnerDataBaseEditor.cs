using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Application;
using UnityEditor;
namespace HTLibrary.Editor
{
    /// <summary>
    /// 生成伙伴信息表
    /// </summary>
    public class PatnerDataBaseEditor 
    {
        [MenuItem("HTLibrary/Creat Patner Data Table",false,-25)]
        static void CreatePatnerDataTable()
        {
            string path = "Assets/PatnerDataTable" + ".asset";
            var Instance= ScriptableObject.CreateInstance<PatnerDataConfigure>();
            AssetDatabase.CreateAsset(Instance, path);
            AssetDatabase.SaveAssets();
        }
    }

}
