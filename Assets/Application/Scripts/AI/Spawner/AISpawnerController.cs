using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using NaughtyAttributes;
using System.Linq;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    /// <summary>
    /// AI主动孵化
    /// </summary>
    public class AISpawnerController : MonoBehaviour
    {
        List<Spawner> spawners = new List<Spawner>();
        [ReorderableList]
        public List<AIS> AIGroup = new List<AIS>();

        int index;

        [SerializeField]
        private float _spawnerInterval;
        private void Awake()
        {
            spawners = FindObjectsOfType<Spawner>().ToList();


            for (int i = 0; i < spawners.Count; i++)
            {
                if (spawners[i].parent.gameObject.name != "AI")
                {
                    spawners.RemoveAt(i);
                }
            }
        }



        /// <summary>
        /// 随机孵化AI （随机位置）
        /// </summary>
        public void RandomSpanwerAI()
        {
            if (index >= AIGroup.Count - 1) return;

            StartCoroutine(IRandowmSpawnerAI());

        }

        IEnumerator IRandowmSpawnerAI()
        {
            AIS AIs = AIGroup[index];
            foreach (var tempAI in AIs.AIs)
            {
                int randomSpawner = UnityEngine.Random.Range(0, spawners.Count);
                spawners[randomSpawner].SpawnAI(tempAI);

                if (EventTypeManager.ContainHTEventType(HTEventType.CountAI))
                {
                    EventTypeManager.Broadcast<int>(HTEventType.CountAI, 1);
                }
                yield return new WaitForSeconds(_spawnerInterval);
            }

            index++;
        }
    }

}
