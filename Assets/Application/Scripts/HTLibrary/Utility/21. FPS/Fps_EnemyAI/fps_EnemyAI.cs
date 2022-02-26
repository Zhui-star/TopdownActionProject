using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 射击游戏AI
    /// 1. 控制敌人巡逻
    /// 2. 控制敌人追击
    /// </summary>
    public class fps_EnemyAI : MonoBehaviour
    {
        public float patrolSpeed = 2f;
        public float chaseSpeed = 5f;
        public float chaseWaitTime = 5f;
        public float patroWaitTime = 1f;

        private EnemySightAndListen enemySight;
        private NavMeshAgent nav;
        private Transform player;
        private fps_PlayerHealth playerHealth;
        private float chaseTimer;
        private float patrolTimer;
        private int wayPointIndex;

        public Transform[] patrolWayPoint;

        public event Action shootHanlde;
        /// <summary>
        /// 初始化组件
        /// </summary>
        private void Start()
        {
            enemySight = this.GetComponent<EnemySightAndListen>();
            nav = this.GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
            playerHealth = player.GetComponent<fps_PlayerHealth>();

        }

        /// <summary>
        /// 监听追击 巡逻
        /// </summary>
        private void Update()
        {
            if (enemySight.playerInSight && playerHealth.hp > 0)//看到了
            {
                Chasing();
            }
            else if (enemySight.playerPosition != enemySight.resetPosition && playerHealth.hp > 0)//听到了
            {
                Chasing();
            }
            else
            {
                Patrolling();
            }


        }

        /// <summary>
        /// 射击
        /// </summary>
        private void Shooting()
        {
           if(shootHanlde!=null)
            {
                shootHanlde();
            }
         }

        /// <summary>
        /// 追击
        /// </summary>
        private void Chasing()
        {
            Vector3 sightingDeltaPos = enemySight.playerPosition - transform.position;
            if (sightingDeltaPos.sqrMagnitude > 4f)
            {
                nav.destination = enemySight.playerPosition;

            }
            else
            {
                Shooting();
                Debug.Log("Shoot");
            }

            nav.speed = chaseSpeed;
            if (nav.remainingDistance < nav.stoppingDistance)
            {
                chaseTimer += Time.deltaTime;

                if (chaseTimer >= chaseWaitTime)
                {
                    enemySight.playerPosition = enemySight.resetPosition;
                    chaseTimer = 0;
                }
            }
            else
            {
                chaseTimer = 0;
            }

        }

        /// <summary>
        /// 巡逻
        /// </summary>
        private void Patrolling()
        {
            nav.speed = patrolSpeed;
            if (nav.destination == enemySight.resetPosition || nav.remainingDistance < nav.stoppingDistance)
            {
                patrolTimer += Time.deltaTime;
                if (patrolTimer >= patroWaitTime)
                {
                    if (wayPointIndex == patrolWayPoint.Length - 1)
                    {
                        wayPointIndex = 0;
                    }
                    else
                    {
                        wayPointIndex++;
                    }
                    patrolTimer = 0;
                }
            }
            else
            {
                patrolTimer = 0;
            }


            nav.destination = patrolWayPoint[wayPointIndex].position;
        }
    }

}
