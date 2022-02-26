using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Utility
{
    public enum GravityEnum
    {
        None,
        Rigidbody,
        CharacterController
    }
    /// <summary>
    /// 物体重力施加
    /// </summary>
    public class ObjectGravity : MonoBehaviour
    {
        public bool GravityActive;
        public float gravity = 9.8f;
        public float  m_yDistance;
        public GravityEnum gravityEnum;
        private CharacterController characterController;
        private Rigidbody _rigidbody;

        private GameManager _gameManager;
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            _rigidbody = GetComponent<Rigidbody>();
            _gameManager = GameManager.Instance;
        }

        /// <summary>
        /// 设置跳跃的力
        /// </summary>
        /// <param name="jumpValue"></param>
        public void SetJumpValue(float jumpValue)
        {
            m_yDistance = jumpValue;
        }

        public void AddGravity()
        {
            m_yDistance -= gravity * Time.deltaTime;

            switch(gravityEnum)
            {
                case GravityEnum.None:
                    break;
                case GravityEnum.Rigidbody:
                    _rigidbody.velocity=new Vector3(0, m_yDistance, 0);
                    break;
                case GravityEnum.CharacterController:
                    characterController.Move(new Vector3(0, m_yDistance, 0));
                    break;
            }
        }

        private void FixedUpdate()
        {
            if(GravityActive)
            {
                AddGravity();
            }
        }

        /// <summary>
        /// 得到Y轴Speed;
        /// </summary>
        /// <returns></returns>
        public float GetMyDistance()
        {
            return m_yDistance;
        }



    }

}
