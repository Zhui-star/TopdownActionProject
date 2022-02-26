using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
namespace HTLibrary.Framework
{
    public class PoolManagerV2 : MonoSingleton<PoolManagerV2>
    {
        public GameObjectPoolList poolList;
        private Dictionary<string, GameObjectPool> poolDict;
        // Start is called before the first frame update
        void Start()
        {
            poolDict = new Dictionary<string, GameObjectPool>();

            foreach (GameObjectPool pool in poolList.poolList)
            {
                poolDict.Add(pool.name, pool);
            }
        }


        public GameObject GetInst(string poolName)
        {
            GameObjectPool pool;
            if (poolDict.TryGetValue(poolName, out pool))
            {
                return pool.GetInst();
            }
            Debug.LogWarning("Pool : " + poolName + " is not exits!!!");
            return null;
        }


        private void OnDestroy()
        {
            Reset();
        }

        /// <summary>
        /// 重置对象池
        /// </summary>
        private void Reset()
        {
            if (poolDict == null) return;

            foreach(var temp in poolDict.Keys)
            {
                GameObjectPool pool;
               if( poolDict.TryGetValue(temp, out pool))
                {
                    pool.ClearPool();
                }
            }
        }

    }

}
