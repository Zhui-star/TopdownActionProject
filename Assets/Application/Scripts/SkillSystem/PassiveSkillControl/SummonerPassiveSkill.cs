using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    /// <summary>
    /// 召唤师被动技能
    /// </summary>
    public class SummonerPassiveSkill : PassiveSkill
    {
        [Range(0, 1)]
        public float percentTrigger = 0.1f;

        public float _cooldown = 2.0f;
        private float _lastTriggerTime;

        private void OnEnable()
        {
            EventTypeManager.AddListener<Vector3>(HTEventType.SummonerPassiveSkill, TriggerPassiveSkill);
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener<Vector3>(HTEventType.SummonerPassiveSkill, TriggerPassiveSkill);
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        void TriggerPassiveSkill(Vector3 direction)
        {
            if (MathUtility.Percent((int)(percentTrigger * 100)))
            {
                if (Time.time - _lastTriggerTime > _cooldown)   //元素法师/召唤师被动 冷却 防止连续发射
                {
                    _skillReleseTrigger.SpawnDirection = direction;
                    _skillReleseTrigger.TriggerSkill(_index);
                    _lastTriggerTime = Time.time;
                }

            }
        }
    }

}
