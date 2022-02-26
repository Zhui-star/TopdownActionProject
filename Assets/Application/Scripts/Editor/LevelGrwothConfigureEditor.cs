using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HTLibrary.Framework;
using HTLibrary.Application;
using HTLibrary.Utility;
namespace HTLibrary.Editor
{
    /// <summary>
    /// 成长配置表生成器
    /// </summary>
    public class LevelGrwothConfigureEditor:MonoBehaviour
    {
        [MenuItem("HTLibrary/Level Growth Configure")]
      static void CreatLevelGrowthConfigure()
        {
            LevelGrowthConfigure configure = ScriptableObject.CreateInstance<LevelGrowthConfigure>();
            string path = "Assets/" + "LevelGrowthConfigure.asset";

            AssetDatabase.CreateAsset(configure, path);

            AssetDatabase.SaveAssets();
        }
    }

}
