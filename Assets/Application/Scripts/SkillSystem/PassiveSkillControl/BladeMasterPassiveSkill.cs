using System.Collections;
using System.Collections.Generic;
using HTLibrary.Utility;
using MoreMountains.FeedbacksForThirdParty;
using UnityEngine;

namespace HTLibrary.Application
{
    public class BladeMasterPassiveSkill : MonoBehaviour
    {
        public int skillID1; //Q技能 0
        public int skillID2; //E技能 1
        private float probabilityMark; //判断是否被动技能释放概率的标识

        private SkillReleaseTrigger skillReleaseTrigger;
        MMAutoFocus focus;
        public static BladeMasterPassiveSkill _instance;

        [Header("被动技能实现概率（0--1）")]
        public float probability;


        // Start is called before the first frame update
        void Start()
        {
            probabilityMark = Random.value;
            _instance = this;
            focus = FindObjectOfType<MMAutoFocus>();
            skillReleaseTrigger = FindObjectOfType<SkillReleaseTrigger>();
           // Debug.Log(skillReleaseTrigger.recoderCooldown.TryGet<int,float>(skillID1));
        }

        //被动技能释放方法
        public void BladeMasterPassiveSkillRelease()
        {
            probabilityMark = Random.value;
            
            if (probabilityMark > (1 - probability))
            {
                Debug.Log("Blade Master Passive Release!!!");
                skillReleaseTrigger.RefreshSkillCooldown(skillID1);
                skillReleaseTrigger.RefreshSkillCooldown(skillID2);
                BladeMasterPassiveSkillParticleControl._instance.BladeMasterParticleSystem.Play();
 
            }
            else
            {
                Debug.Log("Can not Release");
            }
        }

    }
}
