using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.Feedbacks;
namespace HTLibrary.Application
{
    /// <summary>
    /// 战斗状态
    /// </summary>
    public enum BattleState
    {
        Normal,
        Battle
    }

    /// <summary>
    /// 角色战斗控制
    /// </summary>
    public class CharacterBattleController : MonoBehaviour
    {
        public MMStateMachine<BattleState> BattleState { get; set; }
        private LevelEvent levelEvent;
        bool countDown = false;
        private float lastBattleTime;
        [SerializeField]
        private float delayDropBattleState;
        private Animator animator;

        public MMFeedbacks startBattleFeedbacks;
        public MMFeedbacks stopBattleFeedbacks;

        private void Start()
        {
            Initialization();
        }

        /// <summary>
        /// 初始化状态
        /// </summary>
        void Initialization()
        {
            BattleState = new MMStateMachine<BattleState>(this.gameObject, false);
            levelEvent = FindObjectOfType<LevelEvent>();
            animator = GetComponent<Character>()._animator;
            stopBattleFeedbacks?.Initialization();
            startBattleFeedbacks?.Initialization();

            levelEvent.VictoryEvent += StopBattleState;

            if(levelEvent.AIParent==null)
            {
                StopBattleState();
            }
            else if(levelEvent.GetAICount()<=0)
            {
                StopBattleState();
            }
            else
            {
                StartBattleState();
            }

        }

        /// <summary>
        /// 处理各个状态
        /// </summary>

        void ProcessState()
        {
            switch(BattleState.CurrentState)
            {
                case Application.BattleState.Normal:
                    CaseNormalState();
                    break;
                case Application.BattleState.Battle:
                    CaseBattleState();
                    break;
            }
        }

        private void Update()
        {
            ProcessState();
        }

        /// <summary>
        /// 处理Normal 非战斗状态
        /// </summary>
        void CaseNormalState()
        {
            animator.SetFloat("IdleBlend", 0,0.2f,Time.deltaTime);
        }

        /// <summary>
        /// 处理战斗状态
        /// </summary>

        void CaseBattleState()
        {
            animator.SetFloat("IdleBlend", 1,0.2f,Time.deltaTime);

            if(countDown)
            {
               if(Time.time-lastBattleTime>=delayDropBattleState)
                {
                    BattleState.ChangeState(Application.BattleState.Normal);
                    countDown = false;
                    stopBattleFeedbacks?.PlayFeedbacks();
                }
            }
        }

        /// <summary>
        /// 停止战斗状态
        /// </summary>

        public void StopBattleState()
        {
            if(levelEvent.GetAICount()<=0)
            {
                countDown = true;
                lastBattleTime = Time.time;
            }
        }

        /// <summary>
        /// 开启战斗状态
        /// </summary>
        public void StartBattleState()
        {
            BattleState.ChangeState(Application.BattleState.Battle);
            countDown = false;

            startBattleFeedbacks?.PlayFeedbacks();
        }

        private void OnDestroy()
        {
            levelEvent.VictoryEvent -= StopBattleState;
        }


    }

}
