using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace HTLibrary.Application
{
    /// <summary>
    /// Tab信息面板单元UI
    /// </summary>
    public class TabShowItemUI : MonoBehaviourSimplify,IPointerEnterHandler
    {
        public Image _itemIcon;
        public Text _itemName;

        /// <summary>
        /// 得到单位信息
        /// </summary>
        /// <returns></returns>
        protected virtual string GetItemInfo()
        {
            return "";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(EventTypeManager.ContainHTEventType(HTEventType.ShowTabInfo))
            {
                EventTypeManager.Broadcast<string>(HTEventType.ShowTabInfo, GetItemInfo());
            }
        }

        protected override void OnBeforeDestroy()
        {
          
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        /// <param name="sprite"></param>
       public virtual void UpdateUI()
        {
            //_itemIcon.sprite = sprite;
        }

    }
}

