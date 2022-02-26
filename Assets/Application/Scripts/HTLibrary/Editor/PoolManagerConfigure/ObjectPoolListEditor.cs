using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HTLibrary.Framework;

namespace HTLibrary.Editor
{
    public class ObjectPoolListEditor
    {
        [MenuItem("HTLibrary/ObjectPoolList Config")]
        static void CreateItemList()
        {
            GameObjectPoolList ObjectList = ScriptableObject.CreateInstance<GameObjectPoolList>();

            string path = "Assets/" + "ObjectPoolList.asset";

            AssetDatabase.CreateAsset(ObjectList, path);

            AssetDatabase.SaveAssets();

        }
    }

}
