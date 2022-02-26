using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    public class TrapControl : MonoBehaviour
    {
        public GameObject trappingParticleSystem;
        public GameObject trappedParticleSystem;
        public BoxCollider boxCollider;
        public AudioClip trapAudioClip;
        public AudioClip trappedAudioClip;

        public float startTrapTime; //陷阱生效的时间
        public float startExplosionTime; //陷阱爆炸的时间
        public float trapDisappearTime; //陷阱失效时间
        public string ExplosionName;
        private bool isExplosion; //是否产生爆炸；
        

        private void OnEnable()
        {
            CancelInvoke("trapDisappear");
            trappingParticleSystem.SetActive(false);
            trappedParticleSystem.SetActive(false);
            this.gameObject.SetActive(true);
            boxCollider.enabled = false;
            isExplosion = false;
            Invoke("StartTrap", startTrapTime);
            Invoke("trapDisappear", trapDisappearTime);
            SoundManager.Instance.PlaySound(trapAudioClip, this.transform.position, false);
        }
        private void OnDisable()
        {
            if (isExplosion == false)
            {
                StartExplosion();
            }

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                SoundManager.Instance.PlaySound(trappedAudioClip, this.transform.position, false);
                trappingParticleSystem.SetActive(false);
                trappedParticleSystem.SetActive(true);
                isExplosion = true;
                Invoke("StopAttack", 0.1f);
                Invoke("StartExplosion", startExplosionTime);
                CancelInvoke("trapDisappear");
                Invoke("trapDisappear", trapDisappearTime);
            }
        }
        
        /// <summary>
        /// 陷阱生效
        /// </summary>
        private void StartTrap()
        {
            boxCollider.enabled = true;
            trappingParticleSystem.SetActive(true);
        }

        /// <summary>
        /// 停止对敌人造成陷阱伤害
        /// </summary>
        private void StopAttack()
        {
            boxCollider.enabled = false;
        }

        /// <summary>
        /// 陷阱失效
        /// </summary>
        private void trapDisappear()
        {
            this.gameObject.SetActive(false);
        }
        /// <summary>
        /// 产生爆炸
        /// </summary>
        private void StartExplosion()
        {
            if (ExplosionName != "")
            {
                GameObject castEff = PoolManagerV2.Instance.GetInst(ExplosionName);
                castEff.transform.position = this.transform.position;
                //selfParticleSystem.SetActive(false);
                trapDisappear();
            }
            
        }
    }
}

