using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 控制敌人非射击动画
    /// 1 .敌人移动动画
    /// 2. 敌人方向调整
    /// </summary>
    public class fps_enemyAnimator : MonoBehaviour
    {

        public float deadZone = 5f;

        private Transform player;
        public EnemySightAndListen enemySight;
        public NavMeshAgent nav;
        private HashIDs hash;
        private AnimatorSetUp animSetup;

        public Animator anim;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag(Tags.Player).transform;

            hash = HashIDs.Instance;
            animSetup = new AnimatorSetUp(anim);
            nav.updateRotation = false;
            anim.SetLayerWeight(1, 1f);
            anim.SetLayerWeight(2, 1f);

            deadZone *= Mathf.Deg2Rad;

        }

        /// <summary>
        /// 角色移动
        /// </summary>
        void OnAnimatorMove()
        {
            nav.velocity = anim.deltaPosition / Time.deltaTime;
            transform.rotation = anim.rootRotation;

        }

        /// <summary>
        /// 移动监听
        /// </summary>
        private void Update()
        {
            NavAnimSetup();
        }

        /// <summary>
        /// 移动动画设置
        /// </summary>
        void NavAnimSetup()
        {
            float speed;
            float angle;
            if (enemySight.playerInSight)
            {
                speed = 0;
                angle = FindAngle(transform.forward, player.position - transform.position, transform.up);
            }
            else
            {
                speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
                angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

                if (Mathf.Abs(angle) < deadZone)
                {
                    transform.LookAt(transform.position + nav.desiredVelocity);
                }
            }
            animSetup.Setup(speed, angle);
        }

        /// <summary>
        /// 通过玩家位置调整该角色位置
        /// </summary>
        /// <param name="fromVector"></param>
        /// <param name="toVector"></param>
        /// <param name="upVector"></param>
        /// <returns></returns>
        private float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
        {
            if (toVector == Vector3.zero)
            {
                return 0;
            }
            float angle = Vector3.Angle(fromVector, toVector);
            Vector3 normal = Vector3.Cross(fromVector, toVector);
            angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
            angle *= Mathf.Deg2Rad;
         
            return angle;
        }
    }

}
