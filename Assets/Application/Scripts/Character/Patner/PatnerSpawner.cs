using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 孵化一个未激活的伙伴
    /// </summary>
    public class PatnerSpawner :Spawner
    {
        public GameObject[] patners;

        private void Start()
        {
            InitialSpawner();
        }

        /// <summary>
        /// 初始化伙伴
        /// </summary>
        void InitialSpawner()
        {
            int targetIndex = Random.Range(0, patners.Length);

            SpawnAI(patners[targetIndex]);
        }
    }

}
