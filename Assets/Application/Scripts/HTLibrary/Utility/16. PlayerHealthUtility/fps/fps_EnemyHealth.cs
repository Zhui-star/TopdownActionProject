using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 控制敌人血量状态
    /// </summary>
    public class fps_EnemyHealth : MonoBehaviour
    {
        public float hp = 100;
        public Animator anim;
        private HashIDs hash;

        private void Start()
        {

            hash = HashIDs.Instance;

        }

        /// <summary>
        /// 受伤
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(float damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
               //TODO 死亡之后的逻辑

            }
        }

       //TODO 播放敌人死亡动画
    }
}

