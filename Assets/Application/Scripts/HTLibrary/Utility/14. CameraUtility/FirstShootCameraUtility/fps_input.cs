using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;

namespace HTLibrary.Utility
{
    /// <summary>
    /// InputManager框架使用工具（FPS模板）
    /// 不断检测输入
    /// </summary>
    public class fps_input : MonoBehaviour
    {

        public bool LockCursor
        {
            get { return Cursor.lockState == CursorLockMode.Locked ? true : false; }
            set
            {
                Cursor.visible = value;
                Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
            }


        }
        private fps_PlayerParameter parameter;
        private InputManager input;

        private void Start()
        {
            LockCursor = true;
            parameter = this.GetComponent<fps_PlayerParameter>();
            input = InputManager.Instance;
        }

        private void Update()
        {
            InitialInput();
        }

        private void InitialInput()
        {
            parameter.inputMoveVector = new Vector2(input.GetAxis("Horizontal"), input.GetAxis("Vertical"));
            parameter.inputSmoothLook = new Vector2(input.GetAxisRaw("Mouse X"), input.GetAxisRaw("Mouse Y"));
        }
    }

}
