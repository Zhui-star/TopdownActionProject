using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 进度条单位
    /// </summary>
    public class ActionBar : MonoBehaviour
    {
        private float speed = 2;

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        private bool canExcute = false;
        public bool CanExcute
        {
            get
            {
                return canExcute;
            }
            set
            {
                if (value == false)
                {
                    if(EndRoundEvent!=null)
                    {
                        EndRoundEvent();
                    }

                    SetSlideValue(0);

                    ActionBarRoundManager.Instance.State = ActionBarRoundState.Computer;
                }

                canExcute = value;
            }
        }

        public event Action InitRoundEvent;
        public event Action EndRoundEvent;
        public event Action ExcuteRoundEvent;

        [Header("组件")]
        public Slider actionBarSlide;

        [Header("行动条速度增量比率")]
        public float step = 1000;
        /// <summary>
        /// 设置行动条速度
        /// </summary>
        /// <param name="speed"></param>
        public void SetSpeed(int speed)
        {
            Speed = speed;
        }

        /// <summary>
        /// 设置进度条值
        /// </summary>
        /// <param name="value"></param>
        public void SetSlideValue(float value)
        {
            actionBarSlide.value = value;
        }

        /// <summary>
        /// 如果是非敌人控制也非主角控制，行动条才能行动
        /// </summary>
        private void Update()
        {
            if (ActionBarRoundManager.Instance.State == ActionBarRoundState.Computer) //TODO 当前非 不能执行
            {
                if (actionBarSlide.value < 1)
                {
                    actionBarSlide.value += speed / 1000f;
                }
                else
                {
                    CanExcute = true;

                    if(InitRoundEvent!=null)
                    {
                        InitRoundEvent();
                    }
                }
            }
            else
            {
                if(ExcuteRoundEvent!=null)
                {
                    ExcuteRoundEvent();
                }
            }

        }
    }
}
