using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using HTLibrary.Utility;

namespace HTLibrary.Framework
{ /// <summary>
  /// 资源池
  /// </summary>
    [Serializable]
    public class GameObjectPool
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public GameObject prefab;
        [SerializeField]
        public int maxAmount;

        [NonSerialized]
        private List<GameObject> goList = new List<GameObject>();

        Transform parent;

        /// <summary>
        /// 表示从资源池中获取一个实例
        /// </summary>
        public GameObject GetInst()
        {
            foreach (GameObject go in goList)
            {
                if (go.activeInHierarchy == false)
                {
                    go.SetActive(true);
                    return go;
                }
            }

            if (goList.Count >= maxAmount)
            {
                GameObject.Destroy(goList[0]);
                goList.RemoveAt(0);
            }

            if (parent == null)
            {
                parent = GameObject.Find(Consts.PoolParent).transform;
            }

            GameObject temp = GameObject.Instantiate(prefab, parent) as GameObject;
            goList.Add(temp);
            return temp;
        }

        public void ClearPool()
        {
            if (goList.Count > 0)
            {
                goList.Clear();
            }

        }


    }
}
