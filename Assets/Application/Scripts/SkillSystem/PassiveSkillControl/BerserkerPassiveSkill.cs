using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Application
{
    public class BerserkerPassiveSkill : MonoBehaviour
    {
        public float additiveAttackSpeed; //增加的攻速
        public float totalTime; //印记持续时间；
        private float timer; //印记持续时间计时器；
        public ParticleSystem berserkerParticleSystem;

        public CharacterConfig characterConfig;

        // Start is called before the first frame update
        void Start()
        {
            timer = 0;
            berserkerParticleSystem = GetComponent<ParticleSystem>();

        }
        private void Update()
        {
            CreateRageMark();
        }
        //产生狂暴印记
        public void CreateRageMark()
        {
            if (berserkerParticleSystem.isPlaying)
            {
                if (timer == 0)
                {
                    characterConfig.characterAddtiveAttackSpeed += additiveAttackSpeed;
                    Debug.Log("Berserker Passive Skill Release!! The attackSpeed is" + characterConfig.characterAddtiveAttackSpeed);
                }
                timer += Time.deltaTime;
                if (timer > totalTime)
                    berserkerParticleSystem.Stop();
            }
            if (timer > totalTime && berserkerParticleSystem.isStopped)
            {
                characterConfig.characterAddtiveAttackSpeed -= additiveAttackSpeed;
                Debug.Log("Return!! The attackSpeed is" + characterConfig.characterAddtiveAttackSpeed);
                timer = 0;
            }
        }


        private void OnDestroy()
        {
            
            if (timer >totalTime)
            {
                characterConfig.characterAddtiveAttackSpeed -= additiveAttackSpeed;
            }else if(timer<totalTime&&timer!=0)
            {
                characterConfig.characterAddtiveAttackSpeed -= additiveAttackSpeed;
            }

        }

    }


}
