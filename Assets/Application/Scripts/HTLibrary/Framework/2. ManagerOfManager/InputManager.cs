using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Framework
{
    /// <summary>
    /// 输入管理器
    /// 1. 添加按钮热键管理
    /// 2. 添加Axis管理
    /// 3. 添加自定义Axis管理
    /// </summary>
    public class InputManager :MonoSingleton<InputManager>
    {
        public class fps_InputAxis
        {
            public KeyCode positive;
            public KeyCode negative;

        }

        public Dictionary<string, KeyCode> buttons = new Dictionary<string, KeyCode>();

        public Dictionary<string, fps_InputAxis> axis = new Dictionary<string, fps_InputAxis>();

        public List<string> unityAxis = new List<string>();

        /// <summary>
        /// 初始化各个管理
        /// </summary>
        private void Start()
        {
            SetUpDefaults();
        }

        /// <summary>
        /// 添加热键管理
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        private void AddButton(string n, KeyCode k)
        {
            if (buttons.ContainsKey(n))
            {
                buttons[n] = k;
            }
            else
            {
                buttons.Add(n, k);
            }
        }

        /// <summary>
        /// 添加自定义轴向管理
        /// </summary>
        /// <param name="n"></param>
        /// <param name="pk"></param>
        /// <param name="nk"></param>
        private void AddAxis(string n, KeyCode pk, KeyCode nk)
        {
            if (axis.ContainsKey(n))
            {
                axis[n] = new fps_InputAxis() { positive = pk, negative = nk };
            }
            else
            {
                axis.Add(n, new fps_InputAxis() { positive = pk, negative = nk });
            }

        }

        /// <summary>
        /// 添加Unity 轴向管理
        /// </summary>
        /// <param name="n"></param>
        private void AddUnityAxis(string n)
        {
            if (!unityAxis.Contains(n))
            {
                unityAxis.Add(n);
            }
        }

        /// <summary>
        /// 初始化
        /// 1. 热键
        /// 2. Axis
        /// 3. UnityAxis
        /// </summary>
        /// <param name="type"></param>
        private void SetUpDefaults(string type = "")
        {
            if (type == "" || type == "buttons")
            {
                if (buttons.Count == 0)
                {
                    AddButton("Mouse0", KeyCode.Mouse0);
                    //AddButton("Reload", KeyCode.R);
                    //AddButton("Jump", KeyCode.R);
                    //AddButton("Crouch", KeyCode.C);
                    //AddButton("Sprint", KeyCode.LeftShift);
                }
            }

            if (type == "" || type == "Axis")
            {
                if (axis.Count == 0)
                {
                    AddAxis("Horizontal", KeyCode.W, KeyCode.S);
                    AddAxis("Vertical", KeyCode.A, KeyCode.D);


                }

                if (type == "" || type == "UnityAxis")
                {
                    if (unityAxis.Count == 0)
                    {
                        AddUnityAxis("Mouse X");
                        AddUnityAxis("Mouse Y");
                        AddUnityAxis("Horizontal");
                        AddUnityAxis("Vertical");
                        AddUnityAxis("Mouse ScrollWheel");// 鼠标滚轮
                    }
                }
            }

        }

        /// <summary>
        /// 返回指定热键Button状态
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButton(string button)
        {
            if (buttons.ContainsKey(button))
            {
                return Input.GetKey(buttons[button]);
            }
            return false;
        }

        /// <summary>
        /// 返回指定热键Down状态
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonDown(string button)
        {
            if (buttons.ContainsKey(button))
            {
                return Input.GetKeyDown(buttons[button]);
            }
            return false;
        }

        /// <summary>
        /// 返回自定义轴向
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public float GetAxis(string axis)
        {
            if (this.unityAxis.Contains(axis))
            {
                return Input.GetAxis(axis);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回AxisRaw
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public float GetAxisRaw(string axis)
        {
            if (this.axis.ContainsKey(axis))
            {
                float val = 0;
                if (Input.GetKey(this.axis[axis].positive))
                    return 1;

                if (Input.GetKey(this.axis[axis].negative))
                {
                    return -1;
                }

                return val;


            }

            else if (unityAxis.Contains(axis))
            {
                return Input.GetAxisRaw(axis);
            }
            else
            {
                return 0;
            }
        }
    }
}

