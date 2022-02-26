using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 2.5D相机控制
    /// </summary>
    public class TurnRound_Camera : MonoBehaviour
    {
        [Header("属性")]
        public float y_speed = 3.0f;
        public float min_y = 0.0f;
        public float max_y = 0.0f;
        public float x_speed = 3.0f;
        public float min_x = 0.0f;
        public float max_x = 0.0f;
        public float z_speed = 3.0f;
        public float min_z = 0.0f;
        public float max_z = 0.0f;
        public float mouseX_speed = 0.0f;
        public float mouseY_speed = 0.0f;
        Vector2 storePosition = Vector2.zero;
        /// <summary>
        /// 战斗场景相机移动
        /// 1. 范围限制
        /// 2. 键盘控制相机x z移动
        /// 3. 鼠标控制相机x y z移动
        /// </summary>
        private void Update()
        {
            transform.Translate(TurnRound_PlayerParameter.Instance.inputFightCamera.x * x_speed
                , TurnRound_PlayerParameter.Instance.inputFightCamera.y * y_speed, TurnRound_PlayerParameter.Instance.inputFightCamera.z * z_speed
                , Space.World);

            if (Input.GetMouseButtonDown(0))
            {
                storePosition = Input.mousePosition;
            }

            if (TurnRound_PlayerParameter.Instance.mouse0)
            {
                if (Vector2.Distance(storePosition, Input.mousePosition) > 10.0f)
                {
                    transform.Translate(-TurnRound_PlayerParameter.Instance.inputFightCamera_2.x * mouseX_speed
                       , 0, -TurnRound_PlayerParameter.Instance.inputFightCamera_2.y * mouseY_speed
                       , Space.World);
                }

            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, min_x, max_x),
                Mathf.Clamp(transform.position.y, min_y, max_y), Mathf.Clamp(transform.position.z, min_z, max_z));

        }
    }

}
