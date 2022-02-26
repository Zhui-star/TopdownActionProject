using System.Collections;
using System.Collections.Generic;
using HTLibrary.Utility;
using HTLibrary.Framework;
using UnityEditor;
using UnityEngine;
namespace HTLibrary.Editor
{
    public class ItemListEditor
    {
        [MenuItem("HTLibrary/ItemList Config")]
      static void CreateItemList()
        {
            ItemList itemList = ScriptableObject.CreateInstance<ItemList>();

            string path ="Assets/"+"ItemList.asset";

            AssetDatabase.CreateAsset(itemList, path);

            AssetDatabase.SaveAssets();

        }
    }

}
