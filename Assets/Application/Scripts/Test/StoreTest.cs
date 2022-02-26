using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using HTLibrary.Application;

namespace HTLibrary.Test
{
    public class StoreTest : MonoBehaviour
    {
        [HideInInspector]
        public List<int> goodsIDList = new List<int>();

        public static StoreTest _instance;

        public static StoreTest Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<StoreTest>();
                }
                return _instance;
            }
        }

        public int StoreTotalItemNum()
        {
            return goodsIDList.Count;
        }

        private void Start()
        {
            goodsIDList = new List<int> { 22, 21, 15, 10, 6, 1};
        }

        public void GenerateItemClick()
        {
            GetItemData(goodsIDList);
            gameObject.SetActive(false);
            //Knapsack.Instance.StoreItem(Random.Range(1, 22),true);
        }

        private void GetItemData(List<int> dataList)
        {
            foreach (int a in dataList)
            {
                //Knapsack.Instance.StoreItem(a, false);
                //StorePanel.Instance.goodsIDList.Add(a);//TODO
                StorePanel.Instance.StoreItem(a, false);//TEST
            }
        }
    }
}
