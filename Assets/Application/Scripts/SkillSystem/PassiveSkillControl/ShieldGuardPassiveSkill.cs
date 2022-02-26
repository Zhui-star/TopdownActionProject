using System.Collections;
using System.Collections.Generic;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.TopDownEngine;
using UnityEngine;

namespace HTLibrary.Application
{
    public class ShieldGuardPassiveSkill : MonoBehaviour
    {
        public bool isProtected;
        private float timer; //护盾间隔生成计时器
        private float timerCreateShield; //护盾生成时间计时器
        public float totalTime; //间隔生成护盾的时间
        public float totalTimeShield; //护盾生成的时间
                                      //MMAutoFocus focus;

        public static ShieldGuardPassiveSkill _instance;
        public ParticleSystem shieldGuardPassiveSkillParticle;
        //Health health;

        private void Start()
        {
            timer = 0;
            timerCreateShield = 0;
            _instance = this;
            //shieldGuardPassiveSkillParticle.transform.SetParent(focus.FocusTargets[0].transform);
            //hieldGuardPassiveSkillParticle.Stop();
            //health = GetComponent<Health>();
        }
        private void Update()
        {
            CreateShiled();
        }

        public void CreateShiled()
        {
            if (timer < totalTime)
            {
                isProtected = false;
                timer += Time.deltaTime;
                shieldGuardPassiveSkillParticle.Stop();
            }
            else
            {
                if (timerCreateShield < totalTimeShield)
                {
                    isProtected = true;
                    timerCreateShield += Time.deltaTime;
                    shieldGuardPassiveSkillParticle.Play();

                }
                else
                {
                    timer = 0;
                    timerCreateShield = 0;

                }
            }
        }

    }
}