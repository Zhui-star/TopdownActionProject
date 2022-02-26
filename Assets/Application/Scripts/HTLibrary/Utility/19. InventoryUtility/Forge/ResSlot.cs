using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 结果物品预览 Slot
    /// </summary>
    public class ResSlot : Slot
    {
        public override void Awake()
        {
            base.Awake();
            isResSlot = true;
        }

        public void ChangeSlotItemType(Item currentItem)
        {
            iconTypeImage.sprite = currentItem.itemTypeSprite;
        }

        public void ChangeSlotBackground(bool isChange, Item item)
        {
            circleBgImg.gameObject.SetActive(isChange);
            if (isChange)
            {
                slotBgImg.sprite = item.slotSprite;
                circleBgImg.sprite = item.itemTypeBgSprite;
                slotBgImg.color = new Color(slotBgImg.color.r, slotBgImg.color.g, slotBgImg.color.b, 1);

                //feedback
                transform.DOShakeScale(0.25f, 2).SetUpdate(true);
            }
            else
            {
                slotBgImg.color = new Color(slotBgImg.color.r, slotBgImg.color.g, slotBgImg.color.b, 0);
            }
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
           
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }
    }

}
