using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using HTLibrary.Framework;
using HTLibrary.Application;
using MoreMountains.Feedbacks;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 物体孵化器
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        public Transform parent;
        [Header("是否使用对象池")]
        public bool IsUseObjectPool = false;

        [MMInformation("随机位置区分同一位置的敌人",MMInformationAttribute.InformationType.Info,false)]
        public int minRandom = -5;
        public int maxRandom = 5;

        public MMFeedbacks _spawnFeedBacks;

        private void Awake()
        {
            _spawnFeedBacks?.Initialization(this.gameObject);
        }

        /// <summary>
        /// 孵化物体
        /// </summary>
        /// <param name="go"></param>
        public void SpawnAI(GameObject go)
        { 
            if(IsUseObjectPool)
            {
                //TODO
            }
            else
            {
                _spawnFeedBacks?.PlayFeedbacks();
                GameObject tempGo = GameObject.Instantiate(go, parent);
                tempGo.transform.position = this.transform.position + new Vector3(Random.Range(minRandom, maxRandom + 1),
                    0, Random.Range(minRandom, maxRandom + 1));
                tempGo.transform.rotation = transform.rotation;

            }
        }
    }

}
