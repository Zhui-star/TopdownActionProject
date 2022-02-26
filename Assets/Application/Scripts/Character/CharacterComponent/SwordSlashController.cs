using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    /// <summary>
    /// 剑弧控制
    /// </summary>
    public class SwordSlashController : MonoBehaviour
    {
        private ParticleSystem slashParticle;

        private HTAttackSpeed attackSpeed;

        ParticleSystem.MainModule particle;

        [SerializeField]
        private float originLifeTime = 0.3f;

        [SerializeField]
        private float originStartDelay = 0.2f;

        private void Awake()
        {
            slashParticle = GetComponent<ParticleSystem>();
            attackSpeed = GetComponent<HTAttackSpeed>();
            particle = slashParticle.main;
        }

        /// <summary>
        /// 控制Slash特效的一个速度问题
        /// </summary>
        public void SwordSlashLifeTimeControl()
        {
            float targetLifeTime = originLifeTime * attackSpeed.AnimSpeedPercent();
            float targetStartDelay = originStartDelay * attackSpeed.AnimSpeedPercent();
            //TODO API将被启用
            particle.startLifetime = targetLifeTime;
            particle.startDelay = targetStartDelay;
        }
    }

}
