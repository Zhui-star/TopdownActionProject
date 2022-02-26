using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 管理动画参数
    /// </summary>
    public class HashIDs:MonoSingleton<HashIDs>
    {
        //TODO: 动画行为参数
        public int speedFloat;
        public int angularSpeedFloat;

        public int moveBool;
        public int attackTrigger;
        public int takeDamageTrigger;
        /// <summary>
        /// 参数初始化
        /// </summary>
        private void Awake()
        {
            speedFloat = Animator.StringToHash("Speed");
            angularSpeedFloat = Animator.StringToHash("AngularSpeed");

            moveBool = Animator.StringToHash("Move");
            attackTrigger = Animator.StringToHash("Attack");
            takeDamageTrigger = Animator.StringToHash("TakeDamage");
        }
    }

}
