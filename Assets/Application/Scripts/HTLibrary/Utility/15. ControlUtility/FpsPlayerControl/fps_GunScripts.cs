using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 控制角色枪械逻辑
    /// 1. 装载
    /// 2. 开火
    /// </summary>
    public class fps_GunScripts : MonoBehaviour
    {
        public delegate void PlayerShoot();
        public static event PlayerShoot PlayerShootEvent;
        public float fireRate = 0.1f;
        public float damage = 40;
        public float reloadTime = 1.5f;
        public float flashRate = 0.2f;
        public GameObject explosion;
        public int bulletCount = 30;
        public int chargerBulletCount = 60;
        //public Text bulletText;

        public Animation anim;
        public float nextFireTime = 0.0f;
        public MeshRenderer flash;
        public int currentBullet;
        public int currentChargeBullet;
        public fps_PlayerParameter parameter;
        public fps_PlayerControl playerControl;

        /// <summary>
        /// 初始化
        /// 1.子弹数
        /// </summary>
        private void Start()
        {
            flash.enabled = false;
            currentBullet = bulletCount;
            currentChargeBullet = chargerBulletCount;
            //TODO 初始化子弹数
        }

        /// <summary>
        /// 监听
        /// 1. 装载子弹
        /// 2. 开火
        /// 3. 显示动画
        /// </summary>
        void Update()
        {
            if (parameter.inputReload && currentBullet < bulletCount)
            {
                Reload();

            }
            if (parameter.inputFire && true)//TODO 当前是否没有装载子弹
            {
                Fire();
            }
            else if (true)//TODO 当前是否没有装载子弹
            {
                StateAnim(playerControl.State);
            }
        }

        /// <summary>
        /// 装载子弹动画
        /// </summary>
        private void ReloadAnim()
        {
            //TODO 装载子弹
        }

        /// <summary>
        /// 装载子弹完成
        /// </summary>
        /// <returns></returns>
        private IEnumerator ReloadFinish()
        {
            yield return new WaitForSeconds(reloadTime);
            if (currentChargeBullet >= bulletCount - currentBullet)
            {
                currentChargeBullet -= (bulletCount - currentBullet);
                currentBullet = bulletCount;
            }
            else
            {
                currentBullet += currentChargeBullet;
                currentChargeBullet = 0;
            }
        }

        /// <summary>
        /// 装载子弹
        /// </summary>
        private void Reload()
        {
            if (true)//TODO 条件判断
            {
                if (currentChargeBullet > 0)
                {
                    StartCoroutine(ReloadFinish());

                }
                else
                {
                  //TODO 开火动画;
                  //TODO 播放音效
                    return;
                }
                //TODO 装载子弹的音效
                ReloadAnim();
            }
        }
        /// <summary>
        /// 火花表现
        /// </summary>
        /// <returns></returns>
        private IEnumerator Flash()
        {

            flash.enabled = true;
            Debug.Log("flash" + flash.enabled);
            yield return new WaitForSeconds(flashRate);
            flash.enabled = false;

        }

        /// <summary>
        /// 开火
        /// 1. 开火cd
        /// 2. 子弹数目更新
        /// </summary>
        private void Fire()
        {
            if (Time.time > nextFireTime)
            {
                if (currentBullet <= 0)
                {
                    Reload();
                    nextFireTime = Time.time + fireRate;

                }
                currentBullet--;
                //TODO Update子弹数目

                DamageEnemy();
                if (PlayerShootEvent != null)
                {
                    PlayerShootEvent();


                }
                //TODO 播放开火音效
                nextFireTime = Time.time + fireRate;
                //TODO 播放开火动画
                StartCoroutine(Flash());
                Debug.Log("flash");
            }
        }

        private void DamageEnemy()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
            
                //TODO 是否打到敌人
                //TODO 播放音效
                //TODO 初始化特效
                //TODO 敌人受伤
            }
        }

        /// <summary>
        /// 播放当前状态动画
        /// </summary>
        /// <param name="animName"></param>
        private void PlayerStateAnim(string animName)
        {
            //Todo 播放当前状态动画
        }
        private void StateAnim(PlayerState state)
        {
            switch (state)
            {

                case PlayerState.Idle:
                     //TODO 播放Idle动画
                    break;
                case PlayerState.Walk:
                    //TODO 播放Walk动画
                    break;
                case PlayerState.Crouch:
                    //TODO 播放Crouch动画
                    break;
                case PlayerState.Run:
                    //TODO 播放Run动画
                    break;
            }
        }
    }

}
