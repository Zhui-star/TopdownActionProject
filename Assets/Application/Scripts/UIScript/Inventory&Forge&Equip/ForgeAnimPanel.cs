using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using UnityEngine.UI;
using System;
using HTLibrary.Framework;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 合成按钮反馈动画
    /// </summary>
    public class ForgeAnimPanel :MonoBehaviour
    {
        public ResSlot resSlot;
        public Slider slide;
        public GameObject animationImg;
        public GameObject resultImg;
        public float totalTime = 2.0f;
        public float stepTime = 0.02f;
        float currentTime = 0;
        public AudioClip equipClip;
        bool _animEnd=false;

        public RenderTexture avatarRenderTexture;
        /// <summary>
        /// 更新Slide
        /// </summary>
        private void Update() 
        {
            currentTime+=stepTime;
            slide.value = currentTime / totalTime;

            if(currentTime>=totalTime&&!_animEnd)
            {
                CallBack();              
                _animEnd=true;
            }
        }


        /// <summary>
        /// 回调领取
        /// </summary>
        void CallBack()
        {
            animationImg.SetActive(false);
            resultImg.SetActive(true);

            Item item = InventoryManager.Instance.GetItemById(ForgePanel.Instance.ResId);
            resSlot.ChangeSlotItemType(item);
            resSlot.ChangeSlotBackground(true, item);
            resSlot.StoreItem(item,false);

            if(equipClip!=null)
            {
                SoundManager.Instance.PlaySound(equipClip, transform.position, false);
            }
        }

        /// <summary>
        /// 对外引用开启状态
        /// </summary>
        /// <param name="open"></param>
        public void TransformState(bool open)
        {
            currentTime = 0;
            resultImg.SetActive(!open);
            animationImg.SetActive(open);
            this.gameObject.SetActive(open);
            _animEnd=false;
        }

        /// <summary>
        /// 领取奖励
        /// </summary>
        public void ClaimBtnClick()
        {
            DestroyImmediate(resSlot.GetItemUI().gameObject);
            this.gameObject.SetActive(false);
        }

    }

}
