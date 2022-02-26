using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    /// <summary>
    /// 伙伴孵化器
    /// </summary>
    public class PatnerSpawnManager : MonoBehaviour
    {
        public Spawner _spanwer;
        PatnerDataManager _patnerDataManager;

        
        private void Start()
        {
            _patnerDataManager = PatnerDataManager.Instance;

            InitialPatner();
        }

        void InitialPatner()
        {
            if (_spanwer == null) return;

           List<GameObject> patnerGoList=  _patnerDataManager.GetFollowPatners();

            foreach(var temp in patnerGoList)
            {
                _spanwer.SpawnAI(temp);

            }
        }
    }

}
