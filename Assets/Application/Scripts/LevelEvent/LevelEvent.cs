using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System;
using MoreMountains.Feedbacks;

namespace HTLibrary.Application
{
    /// <summary>
    /// 场景事件管理
    /// </summary>
    public class LevelEvent : MonoBehaviour
    {
        [Header("全部AI的父物体")]
        public Transform AIParent;

        int AICount = 0;

        [Header("当前场景的传送门")]
        public GameObject portal;
        public event Action VictoryEvent;

        [Header("宝箱出现位置")]
        public GameObject chestPos;

        [SerializeField]
        [Header("是否开局先暂停BGM")]
        private bool pauseBGM = false;

        public bool PauseBGM
        {
            get
            {
                return pauseBGM;
            }
        }

        public MMFeedbacks victoryFeedBacks;

        [Header("Final battle")]  
        [MMInformation("Is this scene final battle ?",MMInformationAttribute.InformationType.Info,false)]  
        public bool _isLastLevel;

        private void Awake()
        {
            victoryFeedBacks?.Initialization();
        }

        private void Start()
        {
            InitialAICount();
            InitialPortalState();
        }

        void InitialAICount()
        {
            if (AIParent == null) return;
            AICount = AIParent.childCount;
        }

        void InitialPortalState()
        {
            if (AICount <= 0)
            {
                OpenPortal();
            }
        }

        /// <summary>
        /// Number可以是负数记得
        /// </summary>
        /// <param name="number"></param>

        void CountAI(int number)
        {
            AICount += number;

            if (AICount<=0)
            {
                if(SpawnerManager.Instance.GetNumberOfAI()<=0)
                {
                    if(_isLastLevel)
                    {
                        StartCoroutine(ProcessLastBattleVictory());
                    }
                    else
                    {
                        LevelVictory();
                    }

                    VictoryEvent?.Invoke();
                }
                else
                {
                    AICount += SpawnerManager.Instance.AddAINumber();
                    SpawnerManager.Instance.RandomSpanwerAI();
                   
                }            
            }
        }

        /// <summary>
        /// 场景获胜奖励
        /// </summary>
        void LevelVictory()
        {
            victoryFeedBacks?.PlayFeedbacks();

            OpenPortal();
            SpawnAwardChest();
        }

        void OpenPortal()
        {
            if (portal != null)
            {
                portal.GetComponent<Animator>().SetBool("IsOpened", true);
                //portal.SetActive(true);
            }
        }

        void SpawnAwardChest()
        {
            if (chestPos != null)
            {
                GameObject castEff = PoolManagerV2.Instance.GetInst("TalentAwardChest");
                castEff.transform.position = chestPos.transform.position;
            }
            
            //GameObject castEff = PoolManagerV2.Instance.GetInst("TalentAwardChest");
            //Vector3 playerPos = GameObject.FindGameObjectWithTag(Tags.Player).transform.position;
            //Vector3 currentPos = playerPos;
            //currentPos.x = 0;
            //currentPos.y = 16;
            //castEff.transform.position = currentPos;
        }

        /// <summary>
        /// 得到当前一个AI数量
        /// </summary>
        /// <returns></returns>
        public int GetAICount()
        {
            return AICount;
        }

        private void OnEnable()
        {
            EventTypeManager.AddListener<int>(HTEventType.CountAI, CountAI);
        }
        /// <summary>
        /// 重置
        /// </summary>
        private void OnDisable()
        {
            EventTypeManager.RemoveListener<int>(HTEventType.CountAI, CountAI);
        }

        /// <summary>
        /// Process last battle victory logic
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProcessLastBattleVictory()
        {
            yield return new WaitForSeconds(2.0f);
            UIManager.Instance.PushPanel(UIPanelType.VictoryPanel);
        }
    }

}
