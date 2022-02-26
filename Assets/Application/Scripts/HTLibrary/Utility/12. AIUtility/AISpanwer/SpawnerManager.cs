using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using System.Linq;
using System;
using NaughtyAttributes;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;

namespace HTLibrary.Utility
{
    [Serializable]
    public struct AIS
    {
        public List<GameObject> AIs;
    }
    public class SpawnerManager : MonoSingleton<SpawnerManager>
    {
        private List<Spawner> spawners = new List<Spawner>();

        [ReorderableList]
        public List<AIS> AIGroup = new List<AIS>();

        public int numberOfAI;
        private int index;
        public AudioClip audioClip;
        public MMFeedbacks feedBacks;
        [SerializeField] private float _spawnerInterval = 0.5f;
        private void Awake()
        {
            spawners = FindObjectsOfType<Spawner>().ToList();
            

            for(int i=0;i<spawners.Count;i++)
            {
                if(spawners[i].parent.gameObject.name!= "AI") 
                {
                    spawners.RemoveAt(i);
                }
            }
        }

        private void Start()
        {
            while (numberOfAI < AIGroup.Count)
            {
                int deletIndex = UnityEngine.Random.Range(0, AIGroup.Count);
                AIGroup.RemoveAt(deletIndex);
            }
            feedBacks?.Initialization(this.gameObject);
        }

        /// <summary>
        /// 随机孵化AI （随机位置）
        /// </summary>
        public void RandomSpanwerAI()
        {
            if (numberOfAI <= 0) return;
            StartCoroutine(IRandomSpanwerAI());
        }

        IEnumerator IRandomSpanwerAI()
        {           
            SoundManager.Instance.PlaySound(audioClip, this.transform.position);
            feedBacks?.PlayFeedbacks();

            AIS AIs = AIGroup[index];
            foreach (var tempAI in AIs.AIs)
            {
                int randomSpawner = UnityEngine.Random.Range(0, spawners.Count);
                spawners[randomSpawner].SpawnAI(tempAI);
                yield return new WaitForSeconds(_spawnerInterval);
            }          

            index++;
            numberOfAI--;
        }

        /// <summary>
        /// 得到剩余AI波数
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfAI()
        {
            return numberOfAI;
        }

        /// <summary>
        /// 新添AI数
        /// </summary>
        /// <returns></returns>
        public int AddAINumber()
        {
            return AIGroup[index].AIs.Count;
        }
    }

}
