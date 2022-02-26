using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Utility
{
    public enum PlayerState
    {
        None,
        Idle,
        Walk,
        Crouch,
        Run
    }

    public class fps_PlayerControl : MonoBehaviour
    {
        private PlayerState state = PlayerState.None;
        public PlayerState State
        {
            get
            {
                if (runnig)
                    state = PlayerState.Run;
                else if (walking)
                    state = PlayerState.Walk;
                else if (crouching)
                    state = PlayerState.Crouch;
                else
                    state = PlayerState.Idle;
                return state;
            }
        }
        public float sprintSpeed = 10.0f;
        public float sprintJumpSpeed = 8.0f;
        public float normalSpeed = 6.0f;
        public float normalJumpSpeed = 7.0f;
        public float crouchSpeed = 2.0f;
        public float crouchJumpSpeed = 5.0f;
        public float crouchDeltaHeight = 0.5f;

        public float gravity = 20.0f;
        public float cameraMoveSpeed = 8.0f;

        //当前属性
        private float speed;
        private float jumpSpeed;
        private Transform mainCamera;
        private float standardCamHeight;
        private float crouchingCamHeight;
        private bool grounded = false;

        private bool walking = false;
        private bool crouching = false;

        private bool runnig = false;

        //正常情况下角色控制器的参数
        private Vector3 normalControllerCenter = Vector3.zero;
        private float normalControllerHeight = 0.0f;

        private CharacterController controller;

        private fps_PlayerParameter parameter;
        private Vector3 moveDirection = Vector3.zero;

        /// <summary>
        /// 初始化
        /// 1. 状态
        /// 2. 组件
        /// </summary>
        private void Start()
        {
            crouching = false;
            walking = false;
            runnig = false;
            speed = normalJumpSpeed;
            mainCamera = GameObject.FindGameObjectWithTag(Tags.MainCamera).transform;
            standardCamHeight = mainCamera.localPosition.y;
            crouchingCamHeight = standardCamHeight - crouchDeltaHeight;
            //audioSource = this.GetComponent<AudioSource>();
            controller = this.GetComponent<CharacterController>();
            parameter = this.GetComponent<fps_PlayerParameter>();
            normalControllerCenter = controller.center;
            normalControllerHeight = controller.height;



        }

        /// <summary>
        /// Update Move
        /// </summary>
        private void FixedUpdate()
        {
            UpdateMove();
           // AudioManagerment();
        }

        /// <summary>
        /// 控制行为
        /// 1. 控制三维移动
        /// 2. 跳跃
        /// 3. 冲刺
        /// 4. 蹲下
        /// 5. 控制各个状态的速度
        /// </summary>
        private void UpdateMove()
        {
            if (grounded)
            {
                moveDirection = new Vector3(parameter.inputMoveVector.x, 0, parameter.inputMoveVector.y);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;

                if (parameter.inputJump)
                {
                    moveDirection.y = jumpSpeed;
                    //AudioSource.PlayClipAtPoint(jumpAudio, transform.position);
                    CurrentSpeed();
                }
            }
            moveDirection.y -= gravity * Time.deltaTime;
            CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
            grounded = (flags & CollisionFlags.CollidedBelow) != 0;
            if (Mathf.Abs(moveDirection.x) > 0 && grounded || Mathf.Abs(moveDirection.z) > 0 && grounded)
            {
                if (parameter.inputSprint)
                {
                    walking = false;
                    runnig = true;
                    crouching = false;
                }
                else if (parameter.inputCrouch)
                {
                    crouching = true;
                    walking = false;
                    runnig = false;
                }
                else
                {
                    walking = true;
                    runnig = false;
                    crouching = false;
                }
            }
            else
            {
                if (walking)
                    walking = false;
                if (runnig)
                    runnig = false;
                if (parameter.inputCrouch)
                    crouching = true;
                else
                    crouching = false;

            }
            if (crouching)
            {
                controller.height = normalControllerHeight - crouchDeltaHeight;
                //中心点在中心所以是高度的一半
                controller.center = normalControllerCenter - new Vector3(0, crouchDeltaHeight / 2, 0);
            }
            else
            {
                controller.height = normalControllerHeight;
                controller.center = normalControllerCenter;
            }
            UpdateCrouch();
            CurrentSpeed();
        }

        /// <summary>
        /// 控制速度
        /// </summary>
        private void CurrentSpeed()
        {
            switch (State)
            {
                case PlayerState.None:


                    break;
                case PlayerState.Idle:
                    speed = normalSpeed;
                    jumpSpeed = normalJumpSpeed;
                    break;
                case PlayerState.Walk:
                    speed = normalSpeed;
                    jumpSpeed = normalJumpSpeed;
                    break;
                case PlayerState.Crouch:
                    speed = crouchSpeed;
                    jumpSpeed = crouchJumpSpeed;

                    break;
                case PlayerState.Run:
                    speed = sprintSpeed;
                    jumpSpeed = sprintJumpSpeed;
                    break;
            }
        }

        /// <summary>
        /// 蹲下行为
        /// </summary>
        private void UpdateCrouch()
        {
            if (crouching)
            {
                if (mainCamera.localPosition.y > crouchingCamHeight)
                {
                    if (mainCamera.localPosition.y - (crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed) < crouchingCamHeight)
                    {
                        mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);

                    }
                    else
                    {
                        mainCamera.localPosition -= new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed, 0);

                    }
                }
                else
                {
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);
                }

            }
            else
            {
                if (mainCamera.localPosition.y < standardCamHeight)
                {
                    if ((mainCamera.localPosition.y + (crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed) > standardCamHeight))
                        mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standardCamHeight, mainCamera.localPosition.z);
                    else
                        mainCamera.localPosition += new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed, 0);
                }
                else
                {
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standardCamHeight, mainCamera.localPosition.z);
                }
            }
        }
    }

}
