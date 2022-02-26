using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    public class MovingUI : MonoBehaviour
    {
        /// <summary>
        /// 当前所在的Canvas
        /// </summary>
        public Canvas canvas;

        //是否启动鼠标模式
        bool FollowMouse = false;


        private void Start()
        {
            if(canvas==null)
            {
                canvas = GameObject.Find("UICanvas").GetComponent<Canvas>();               
            }
        }

        /// <summary>
        /// 设置UI位置
        /// </summary>
        /// <param name="Position"></param>
        protected virtual void SetLocaPosition(Vector3 Position)
        {
            this.transform.localPosition = Position;
        }

        protected Vector2 MousePositionFromRectTransform()
        {
            Vector2 position;

            if (canvas == null)
            {
                canvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            }

            if(canvas.renderMode==RenderMode.ScreenSpaceCamera)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle
(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle
(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            }

            //Debug.Log(position.x);
            if(position.x >= 112.0f && Knapsack.Instance.isShowTheEquipTips)
            {
                position += new Vector2(-420.0f, 0);
            }
            return position + new Vector2(25, 0);

            //x=313的时候要将ui物体显示在左侧
        }

        /// <summary>
        /// UI跟随鼠标移动
        /// </summary>
        protected virtual void SetFollowMousePosition()
        {
            FollowMouse = true;
        }

        private void Update()
        {
            if(FollowMouse)
            {
              transform.localPosition = MousePositionFromRectTransform();
            }
        }
    }

}

