using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HTLibrary.Application
{
    public class GameSettingConfigureEditor : MonoBehaviour
    {
        [MenuItem("HTLibrary/GameSetting Config")]
        static void CreateGameSetDB()
        {
            GameSettingConfigure gsConfig = ScriptableObject.CreateInstance<GameSettingConfigure>();
            string Path = "Assets/" + "GameSettingConfig.asset";
            AssetDatabase.CreateAsset(gsConfig, Path);
            AssetDatabase.SaveAssets();
        }
    }
}
