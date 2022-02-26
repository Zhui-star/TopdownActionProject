using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// Fps游戏相机
    /// 1. 接受输入
    /// 2. 相机上下旋转 人物左右旋转
    /// </summary>
    public class fps_Camera : MonoBehaviour
    {

        //鼠标灵敏度
        public Vector2 mouseLookSenesitivity = new Vector2(5, 5);
        //X轴旋转限制
        public Vector2 rotationXLimit = new Vector2(87, -87);
        public Vector2 rotationYLimit = new Vector2(-360, 360);
        public Vector3 positionOffset = new Vector3(0, 2, -0.2f);

        private Vector2 currentMouseLook = Vector2.zero;
        private float x_Angle = 0;
        private float y_Angle = 0;
        private fps_PlayerParameter parameter;
        private Transform m_Transform;

        private void Start()
        {
            parameter = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<fps_PlayerParameter>();
            m_Transform = transform;
            m_Transform.localPosition = positionOffset;
        }

        private void GetMouseLook()
        {
            currentMouseLook.x = parameter.inputSmoothLook.x;
            currentMouseLook.y = parameter.inputSmoothLook.y;

            currentMouseLook.x *= mouseLookSenesitivity.x;
            currentMouseLook.y *= mouseLookSenesitivity.y;

            currentMouseLook.y *= -1;

        }

        private void Update()
        {
            Debug.Log(Input.GetAxis("Mouse Y"));
            UpdateInput();
        }

        private void LateUpdate()
        {
            Quaternion xQuternion = Quaternion.AngleAxis(y_Angle, Vector3.up);
            Quaternion yQuternion = Quaternion.AngleAxis(0, Vector3.left);
            m_Transform.parent.rotation = xQuternion * yQuternion;

            yQuternion = Quaternion.AngleAxis(x_Angle, Vector3.right);
            m_Transform.rotation = xQuternion * yQuternion;
        }

        private void UpdateInput()
        {
            if (parameter.inputSmoothLook == Vector2.zero)
            {
                return;
            }

            GetMouseLook();
            y_Angle += currentMouseLook.x;
            x_Angle += currentMouseLook.y;

            y_Angle = y_Angle < -360 ? y_Angle += 360 : y_Angle;
            y_Angle = y_Angle > 360 ? y_Angle -= 360 : y_Angle;
            y_Angle = Mathf.Clamp(y_Angle, rotationYLimit.x, rotationYLimit.y);

            x_Angle = x_Angle < -360 ? x_Angle += 360 : x_Angle;
            x_Angle = x_Angle > 360 ? x_Angle -= 360 : x_Angle;
            x_Angle = Mathf.Clamp(x_Angle, -rotationXLimit.x, -rotationXLimit.y);

        }
    }

}
