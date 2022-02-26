using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 提示框
    /// </summary>
    public class ToolTips : MovingUI { 

        public Text toolTipTxt;
        public Text contentTxt;
        public GameObject equipedTooltipsGo;
        public Text equipToolTipTxt;
        public Text equipContentTxt;
        private CanvasGroup canvasGroup;
        private Animator showAnimator;
        public ToolTipType toolTipType;

        public bool IsFollowMouse = false;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            showAnimator = GetComponent<Animator>();
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        /// <param name="text"></param>
        public void ShowCurrentSlotInfo(string title,string content)
        {
            toolTipTxt.text = title;
            contentTxt.text = content;
            //canvasGroup.alpha = 1;
            showAnimator.SetBool("isShow", true);

            if (IsFollowMouse)
            {
                SetFollowMousePosition();
            }
        }

        public void ShowEquipSlotInfo(bool isShow, string title, string content)
        {
            equipedTooltipsGo.SetActive(isShow);
            if(title != null && content != null)
            {
                equipToolTipTxt.text = title;
                equipContentTxt.text = content;
            }
        }

        /// <summary>
        /// 隐藏提示提示
        /// </summary>
        public void Hide()
        {
            //canvasGroup.alpha = 0;
            showAnimator.SetBool("isShow", false);
        }

    }

}
