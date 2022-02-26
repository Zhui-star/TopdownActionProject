using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// RPG动画收集器
    /// </summary>
    public class RPGAnim : MonoBehaviour
    {
        public Animator anim;

       void Move()
        {
            anim.SetBool(HashIDs.Instance.moveBool, true);
        }

        void Attack()
        {
            anim.SetBool(HashIDs.Instance.moveBool, false);
            anim.SetTrigger(HashIDs.Instance.attackTrigger);
        }

        void Idle()
        {
            anim.SetBool(HashIDs.Instance.moveBool, false);
            //TODO
        }

        void Death()
        {
            anim.SetBool(HashIDs.Instance.moveBool, false);
            //TODO 死亡Bool
        }

        void TakeDamage()
        {
            anim.SetBool(HashIDs.Instance.moveBool, false);
            anim.SetTrigger(HashIDs.Instance.takeDamageTrigger);
        }

        /// <summary>
        /// 动画转化机
        /// </summary>
        /// <param name="state"></param>
        public void AnimatorManager(RPGPlayerState state)
        {
            switch(state)
            {
                case RPGPlayerState.Idle:
                    Idle();
                    break;
                case RPGPlayerState.Attack:
                    Attack();
                    break;
                case RPGPlayerState.Move:
                    Move();
                    break;
                case RPGPlayerState.Death:
                    Death();
                    break;
                case RPGPlayerState.TakeDamage:
                    TakeDamage();
                    break;
            }
        }
    }

}
