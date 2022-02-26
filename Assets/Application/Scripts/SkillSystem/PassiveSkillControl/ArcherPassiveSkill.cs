using System.Collections;
using System.Collections.Generic;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.TopDownEngine;
using UnityEngine;

namespace HTLibrary.Application
{
    public class ArcherPassiveSkill : MonoBehaviour
    {
        public static ArcherPassiveSkill _instance;
        public CharacterConfig characterConfig;
        public float AttackSpeed; //攻速
        public float ArcherPassiveSkillDuration; //弓箭手被动技能持续时间
        private float timer = 0; //计时器
        public bool ArcherPassiveSkillHappen; //判断弓箭手被动技能是否可以释放
        bool processPassive = false;

        private void Start()
        {
            _instance = this;
            timer = 0f;
            ArcherPassiveSkillHappen = false;
            //ArcherPassiveSkillParticleSystem.transform.SetParent(focus.FocusTargets[0]);
        }
        private void Update()
        {
            if (ArcherPassiveSkillHappen)
            {
                if (timer == 0)
                {
                    BoostAttackSpeed();

                }
                if (timer < ArcherPassiveSkillDuration)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    ReturnNormalSpeed();
                    timer = 0;
                    ArcherPassiveSkillHappen = false;

                }
            }


        }
        public virtual void BoostAttackSpeed() //提高攻速
        {
            if(!processPassive)
            {
                processPassive = true;
                characterConfig.characterAddtiveAttackSpeed+= AttackSpeed;
                ArcherPassiveSkillParticleControl._instance.ArcherParticleSystem.Play();

            }
        }

        public virtual void ReturnNormalSpeed() //恢复攻速
        {
            if(processPassive)
            {
                characterConfig.characterAddtiveAttackSpeed -= AttackSpeed;
                ArcherPassiveSkillParticleControl._instance.ArcherParticleSystem.Stop();
                processPassive = false;
            }
        }

        private void OnDisable()
        {
            ReturnNormalSpeed();
        }


    }
}
