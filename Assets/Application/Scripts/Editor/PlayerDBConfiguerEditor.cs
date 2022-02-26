using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace HTLibrary.Application
{
    public class PlayerDBConfiguerEditor : MonoBehaviour
    {
        [MenuItem("HTLibrary/Player DB")]
        static void CreatePlayerDB()
        {
            PlayerDBConfiguer playerConfigure= ScriptableObject.CreateInstance<PlayerDBConfiguer>();
            string Path = "Assets/" + "PlayerDBConfiguer.asset";
            AssetDatabase.CreateAsset(playerConfigure, Path);
            AssetDatabase.SaveAssets();
        }
    }

}
