using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

namespace HTLibrary.Application
{
    public class SwordmanPassiveSkill:MonoBehaviour
    {
        public static SwordmanPassiveSkill _instance;

        SkillReleaseTrigger skillReleaseTrigger;

        public int skillID = 0;
        public float reduceTime = 0.5f; //要减少的技能冷却时间
        //The probability of passive skill
        private float passiveProbability; 

        [Header("被动技能实现概率（0--1）")]
        public float probability; //实现减少技能冷却时间的概率

        private void Start()
        {
            skillReleaseTrigger = GetComponent<SkillReleaseTrigger>();
            passiveProbability = Random.value;
            //Debug.Log(passiveProbability);
            _instance = this;
        }
        
        public void PassiveSkillRelease()
        {
            passiveProbability = Random.value;
            //Debug.Log(passiveProbability);
            if (passiveProbability > (1 - probability))
            {
                skillReleaseTrigger.ReduceSkillCooldown(skillID, reduceTime);
                SwordManLevel1PassiveSkillParticleControl._instance.SwordmanParticleSystem.Play();
            }
            else
            {
            }
        }
    }
}

