using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using HTLibrary.Application;

namespace HTLibrary.Test
{
    public class InventoryTest : MonoBehaviour
    {
        public List<int> itemIDList = new List<int>();

        public static InventoryTest _instance;

        public static InventoryTest Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InventoryTest>();
                }
                return _instance;
            }
        }

        public int InventoryTotalItemNum()
        {
            return itemIDList.Count;
        }


        public void GenerateItemClick()
        {
            int random =  Random.Range(0, itemIDList.Count);
            Knapsack.Instance.StoreItem(itemIDList[random], true);
            HTDBManager.Instance.AddItemKnapsack(itemIDList[random]);
        }

      
    }

}
