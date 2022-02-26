using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace HTLibrary.Application
{
    public class CharacterConfigEditor : MonoBehaviour
    {
        [MenuItem("HTLibrary/Chararcter Config")]
        static void CreateItemList()
        {
            CharacterConfig characterConfig = ScriptableObject.CreateInstance<CharacterConfig>();

            string path = "Assets/" + "CharacterConfig.asset";

            AssetDatabase.CreateAsset(characterConfig, path);

            AssetDatabase.SaveAssets();

        }
    }

}
