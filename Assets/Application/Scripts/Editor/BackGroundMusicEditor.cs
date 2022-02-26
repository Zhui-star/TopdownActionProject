using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace HTLibrary.Application
{
    public class BackGroundMusicEditor : MonoBehaviour
    {
        [MenuItem("HTLibrary/BackGoundMusic Config")]
        static void CreateItemList()
        {
            BackGroundMusicConfigure backGroundMusicConfigure = ScriptableObject.CreateInstance<BackGroundMusicConfigure>();

            string path = "Assets/" + "BackGroundMusicConfigure.asset";

            AssetDatabase.CreateAsset(backGroundMusicConfigure, path);

            AssetDatabase.SaveAssets();

        }
    }

}
