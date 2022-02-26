using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 2.5D回合制控制参数
    /// </summary>
    public class TurnRound_PlayerParameter : MonoSingleton<TurnRound_PlayerParameter>
    {
        [HideInInspector]
        public Vector3 inputFightCamera;
        public Vector2 inputFightCamera_2;
        public bool mouse0 = false;
        /// <summary>
        /// 初始化战斗场景相机输入
        /// </summary>
        void InitialInput()
        {
            inputFightCamera = new Vector3(InputManager.Instance.GetAxis("Horizontal"), InputManager.Instance.GetAxis("Mouse ScrollWheel"), 
                InputManager.Instance.GetAxis("Vertical"));
            inputFightCamera_2 = new Vector2(InputManager.Instance.GetAxis("Mouse X"), InputManager.Instance.GetAxis("Mouse Y"));
            mouse0 = InputManager.Instance.GetButton("Mouse0");
        }

        private void Update()
        {
            InitialInput();
        }
    }

}
