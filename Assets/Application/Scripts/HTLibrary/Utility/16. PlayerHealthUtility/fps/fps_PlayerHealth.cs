using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 角色状态控制
    /// 1. 受伤
    /// 2. 禁止交互
    /// 3. 死亡
    /// </summary>
    public class fps_PlayerHealth : MonoBehaviour
    {
        public bool isDead;
        public float resetAfterDeathTime = 5;
        public float maxHp = 100;
        public float hp = 100;
        public float recoverSpeed = 1;

        private float timer = 0;
        public FadeInOutUtility fader;

        private void Start()
        {
            hp = maxHp;
            //不显示血红
        }

        /// <summary>
        /// 受伤
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage)
        {
            if (isDead)
            {
                return;
            }

            hp -= damage;
        }

        /// <summary>
        /// 禁止交互
        /// </summary>
        public void DisableInput()
        {
            //TODO 
        }

        /// <summary>
        /// 角色死亡
        /// </summary>
        public void PlayerDead()
        {
            isDead = true;
            DisableInput();
        }

        /// <summary>
        /// 关卡重置
        /// </summary>
        public void LevelReset()
        {
            timer += Time.deltaTime;
            if (timer >= resetAfterDeathTime)
            {
                fader.EndScen();
            }
        }

        /// <summary>
        /// 监听角色状态
        /// </summary>
        private void Update()
        {
            if (!isDead)
            {
                hp += recoverSpeed * Time.deltaTime;
                if (hp > maxHp)
                {
                    hp = maxHp;
                }
                if (hp < 0)
                {
                    if (!isDead)
                    {
                        PlayerDead();
                    }
                    else
                    {
                        LevelReset();
                    }
                }
            }
        }

    }

}
