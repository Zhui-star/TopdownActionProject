using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using System;
namespace HTLibrary.Application
{
    public class ItemSport : MonoBehaviour
    {
        public ParticleSystem trail;
        public Rigidbody rigidBody;
        public Collider _collider;
        public float _originalFlySpeed = 15.0f;
        private float flySpeed;
        public Animator anim;
        public float delayFlyTime = 0.5f;
        private bool fly = false;
        private Character character;
        public Vector3 destinationOffest;
        public event  Action triggerEvent;
        private Vector3 spawnImpact;
        private float spawnTimer = 0;
        public float spawnTime = 0.5f;
        public float SpawnForce = 1.0f;
        public float Acceleration = 2.0f;
        private float SlowDown = 0;

        [MoreMountains.Tools.MMInformation("如果超过这个时间强行吸收",MoreMountains.Tools.MMInformationAttribute.InformationType.Info,false)]
        public float lastTime = 2.0f;
        private void Start()
        {
            if(character==null)
            {
                character = CharacterManager.Instance.GetCharacter("Player1");
            }          
        }

        private void OnEnable()
        {
            SlowDown = SpawnForce;

            spawnImpact = transform.right * UnityEngine.Random.Range(-5, 5) + transform.up * 10 +
                transform.forward * UnityEngine.Random.Range(-5, 5);

            flySpeed = _originalFlySpeed;

            Invoke("StartFly", lastTime);
        }

        void StartFly()
        {
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
            _collider.isTrigger = true;

            if (this.gameObject.activeInHierarchy)
            {
                StartCoroutine(IFly());
            }
        }

        void SpawnSport()
        {
            if(spawnTimer<spawnTime)
            {
                rigidBody.AddForce(spawnImpact * SlowDown,ForceMode.Acceleration);
                spawnTimer += Time.deltaTime;
                SlowDown += Time.deltaTime*Acceleration;
            }
           
        }

        /// <summary>
        /// 碰撞到地面
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.layer==9)
            {
                StartFly();
            }
        }


        /// <summary>
        /// 飞行开始
        /// </summary>
        /// <returns></returns>

        IEnumerator IFly()
        {
            yield return new WaitForSeconds(delayFlyTime);
            fly = true;
            anim.SetBool("Fly", true);
            trail.Play();

            if (character == null)
            {

                character = CharacterManager.Instance.GetCharacter("Player1");
            }

            if (character != null)
            {
                transform.LookAt(character.transform.position + destinationOffest);
            }
        }

        /// <summary>
        /// 处理飞行
        /// </summary>
        private void FixedUpdate()
        {
            SpawnSport();

            if(fly)
            {
                if(character==null)
                {

                    character = CharacterManager.Instance.GetCharacter("Player1");
                }

                transform.position = Vector3.Lerp(transform.position, character.transform.position + destinationOffest, Time.deltaTime * flySpeed);
            }
        }

        /// <summary>
        /// 经验/Hp糖果获取
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag==Tags.Player)
            {
                flySpeed = 0;
                this.gameObject.SetActive(false);
                triggerEvent?.Invoke();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void OnDisable()
        {
            rigidBody.useGravity = true;
            rigidBody.isKinematic = false;
            _collider.isTrigger = false;
            fly = false;
            spawnTimer = 0;
            anim.SetBool("Fly", false);


        }
    }

}
