using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Utility
{
    public class fps_EnemyShoot : MonoBehaviour
    {

        public float maximumDamage = 120;
        public float minimumDamage = 45;
        public float flashIntensity = 3f;
        public float fadeSpeed = 10f;

        private Animator anim;
        private HashIDs hash;
        private LineRenderer laserShotLine;
        private SphereCollider col;
        private fps_PlayerHealth playerHealth;
        private bool shooting;
        private float scaledDamage;
        private Light laserShotLight;
        private Transform player;
        public fps_EnemyAI aiController;

        private void OnEnable()
        {
            aiController.shootHanlde += Shoot;
        }
        private void OnDisable()
        {
            aiController.shootHanlde -= Shoot;
        }
        /// <summary>
        /// 初始化
        /// 1. 组件
        /// 2. 伤害
        /// </summary>
        private void Start()
        {
            anim = this.GetComponent<Animator>();
            laserShotLine = this.GetComponentInChildren<LineRenderer>();
            laserShotLight = laserShotLine.GetComponent<Light>();

            col = GetComponentInChildren<SphereCollider>();
            player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
            playerHealth = player.GetComponent<fps_PlayerHealth>();
            hash = HashIDs.Instance;

            laserShotLine.enabled = false;
            laserShotLight.intensity = 0;

            scaledDamage = maximumDamage - minimumDamage;


        }

        /// <summary>
        /// IK逻辑
        /// </summary>
        /// <param name="laterIndex"></param>
        void OnAnimatorIK(int laterIndex)
        {
            //TODO 设置抬起权值
            float aimWeight = anim.GetFloat(0);
            anim.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * 1.5f);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);

        }

        /// <summary>
        /// 射击
        /// 1. 距离越近伤害越高
        /// </summary>
        private void Shoot()
        {
            shooting = true;
            float fractionalDistance = ((col.radius - Vector3.Distance(transform.position, player.position)) / col.radius);
            float damage = scaledDamage * fractionalDistance + minimumDamage;

            playerHealth.TakeDamage(damage);

            ShotEffects();

        }

        /// <summary>
        /// 显示特效
        /// </summary>
        private void ShotEffects()
        {
            laserShotLine.SetPosition(0, laserShotLine.transform.position);
            laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f);
            laserShotLight.enabled = true;
            laserShotLight.intensity = flashIntensity;

        }

        /// <summary>
        /// 监听射击
        /// </summary>
        private void Update()
        {
            //TODO 射击的动画权值
            float shot = anim.GetFloat(0);

            if (shot > 0.05f && !shooting)
            {
                Shoot();
            }
            if (shot < 0.05f)
            {
                shooting = false;
                laserShotLine.enabled = false;
            }

        }

    }
}
