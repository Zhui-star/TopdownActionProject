using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Application
{
    public class MagicianBeamPassiveSkill : MonoBehaviour
    {
        public bool isRelease; //判断技能是否释放
        private float passiveProbability; //概率标识
        private float originCritMultiple; //原始暴击倍率


        public CharacterConfig characterConfig;
        private WeaponCrit weaponCrit;
        private SpikeSkill spikeSkill;
        private SkillReleaseTrigger skillReleaseTrigger;

        [Header("被动技能实现概率（0--1）")]
        public float probability; //造成双倍的概率


        // Start is called before the first frame update
        void Start()
        {

            skillReleaseTrigger = GetComponent<SkillReleaseTrigger>();
            weaponCrit = FindObjectOfType<WeaponCrit>();
            spikeSkill = FindObjectOfType<SpikeSkill>();
            passiveProbability = Random.value;
            DoubleCrit();


        }

        // Update is called once per frame
        private void OnEnable()
        {
            originCritMultiple = characterConfig.characterCritMultiple;
        }
        private void OnDisable()
        {
            isRelease = false;
            weaponCrit.characterConfigure.additiveCritMultiple= originCritMultiple;
            MagicianPassiveSkillParticleControl._instance.MagicianParticleSystem.Stop();
        }


        public void DoubleCrit() //双倍暴击
        {
            if (isRelease == false)
            {
                passiveProbability = Random.value;
                Debug.Log(passiveProbability);
                if (passiveProbability > (1 - probability))
                {
                    MagicianPassiveSkillParticleControl._instance.MagicianParticleSystem.Play();
                    weaponCrit.characterConfigure.additiveCritMultiple *= 2;

                }
                isRelease = true;
            }
            //weaponCrit.CritMultiple = characterConfig.characterCritMultiple;
            //Debug.Log(characterConfig.characterCritMultiple);
        }
    }
}

