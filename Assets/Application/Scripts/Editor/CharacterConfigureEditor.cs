using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HTLibrary.Application
{
    public class CharacterConfigureEditor : MonoBehaviour
    {
        [MenuItem("HTLibrary/Chararcter Configure")]
        static void CreateItemList()
        {
            CharacterConfigure characterConfig = ScriptableObject.CreateInstance<CharacterConfigure>();

            string path = "Assets/" + "CharacterConfigure.asset";

            AssetDatabase.CreateAsset(characterConfig, path);

            AssetDatabase.SaveAssets();

        }
    }
}