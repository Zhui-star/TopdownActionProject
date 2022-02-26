using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using UnityEngine.UI;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 回合制战斗玩家行为
    /// </summary>
    public class RoundCharacter : MonoBehaviour
    {

        [Header("组件")]
        public ActionBar actionBar;
        public Transform actionBarParent;

        [Header("角色类型")]
        public ActionBarRoundState characterState;
        public CharacterType_Round characterType;

        public GameObject menue;

        [Header("属性")]
        public float speed;//Test

        /// <summary>
        /// 事件
        /// </summary>
        private void OnEnable()
        {

            actionBar.InitRoundEvent += InitRound;
            actionBar.EndRoundEvent += EndRound;
            actionBar.ExcuteRoundEvent += ExcuteRound;
        }

        /// <summary>
        /// 事件注销
        /// </summary>
        private void OnDisable()
        {
            actionBar.InitRoundEvent -= InitRound;
            actionBar.EndRoundEvent -= EndRound;
            actionBar.ExcuteRoundEvent -= ExcuteRound;
        }

        /// <summary>
        /// 初始化
        /// 1. 组件
        /// </summary>
        private void Awake()
        {
            GameObject go = GameObject.Instantiate(actionBar.gameObject, actionBarParent);
            actionBar = go.GetComponent<ActionBar>();
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        private void Start()
        {
            SetSpeed(speed);
        }

        /// <summary>
        /// 初始化回合
        /// </summary>
        void InitRound()
        {
            ActionBarRoundManager.Instance.SetState(this.characterState);
            ActionBarRoundManager.Instance.SetPlayerType(this.characterType);
            if(!menue.activeInHierarchy)
            {
                menue.SetActive(true);
            }
        }

        /// <summary>
        /// 结束回合
        /// </summary>
        void EndRound()
        {
            menue.SetActive(false);
        }

        /// <summary>
        /// 执行回合
        /// </summary>
        void ExcuteRound()
        {

        }

        public void SetSpeed(float speed)
        {
            actionBar.Speed = speed;
        }
    }

}
