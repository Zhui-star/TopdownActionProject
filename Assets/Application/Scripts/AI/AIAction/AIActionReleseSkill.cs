using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    /// <summary>
    /// AI 释放技能的脚本
    /// </summary>
    public class AIActionReleseSkill : AIAction
    {
        private SkillReleaseTrigger skillRelesTrigger;
        [Header("参考HTCharacterSkill 技能ID")]
        public int skillId;
        CharacterOrientation3D oritentation3D;
        [Header("旋转")]
        public bool IsOpenLookAt = true;
        public bool smoothRotate = false;
        public float rotateSpeed = 10;

        [Header("延迟")]
        public float _delayTime = 0;
        private float _timer = 0;
        /// <summary>
        /// 释放技能
        /// </summary>
        public override void PerformAction()
        {
            if(_delayTime>0&&_timer<_delayTime)
            {
                _timer += Time.deltaTime;
                return;
            }

            if(IsOpenLookAt&&smoothRotate)
            {
                oritentation3D.HTForceRotation(_brain.Target,smoothRotate,rotateSpeed);
            }

            if(!skillRelesTrigger.OnAttacking&&IsOpenLookAt)
            {
                if(!smoothRotate)
                {
                     oritentation3D.HTForceRotation(_brain.Target);
                }

                skillRelesTrigger.TriggerSkill(skillId);
            }

            if(!IsOpenLookAt)
            {
                skillRelesTrigger.TriggerSkill(skillId);
            }
        }

        protected override void Initialization()
        {
            base.Initialization();
            skillRelesTrigger = GetComponent<SkillReleaseTrigger>();
            oritentation3D = GetComponent<CharacterOrientation3D>();
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            if(IsOpenLookAt)
            {
                oritentation3D.CharacterRotationAuthorized = false;

            }

            _timer = 0;

        }

        public override void OnExitState()
        {
            base.OnExitState();
            oritentation3D.CharacterRotationAuthorized = true;
            
            skillRelesTrigger.StopSkillRelese();
            
        }
    }

}
