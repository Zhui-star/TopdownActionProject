using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 敌人的视觉和听觉感官AI
    /// 1. 监听玩家
    /// 2. 视野是否看到了玩家
    /// </summary>
    public class EnemySightAndListen : MonoBehaviour
    {
        public float fieldOfViewAngle = 110f;
        public bool playerInSight;
        public Vector3 playerPosition;
        public Vector3 resetPosition = Vector3.zero;

        [Header("Component")]
        public NavMeshAgent nav;
        public SphereCollider col;
        public GameObject player;
        private HashIDs hash;


        private void Start()
        {
            hash = HashIDs.Instance;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == player)
            {
                playerInSight = false;
                Vector3 direction = other.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < fieldOfViewAngle * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit))
                    {
                        if (hit.collider.gameObject == player)
                        {
                            playerInSight = true;//看到敌人了
                            playerPosition = player.transform.position;

                        }
                    }
                }

                //TODO: 监听Player 如果玩家在干嘛
                ListenPlayer();

            }
        }

        /// <summary>
        /// 敌人从视野消失了
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                playerInSight = false;
            }
        }

        /// <summary>
        /// 监听玩家 的声音
        /// </summary>
        private void ListenPlayer()
        {
            if (Vector3.Distance(player.transform.position, transform.position) < col.radius)
            {
                playerPosition = player.transform.position;

            }
        }

    }
}
