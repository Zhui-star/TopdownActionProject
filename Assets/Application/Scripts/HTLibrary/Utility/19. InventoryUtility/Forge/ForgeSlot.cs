using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 合成材料Slot
    /// </summary>
    public class ForgeSlot : Slot
    {
        public AudioClip putOnClip;

        public override void Awake()
        {
            base.Awake();
            isForgeSlot = true;
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

                //Feedback
               transform.DOShakeScale(0.25f, 2);

               if(putOnClip!=null)
                {
                    SoundManager.Instance.PlaySound(putOnClip, transform.position,false);
                }
                //transform.DOShakePosition(0.25f, 2);
            }
            else
            {
                slotBgImg.color = new Color(slotBgImg.color.r, slotBgImg.color.g, slotBgImg.color.b, 0);
            }
        }

        public override void UseItem(int num = 1)
        {
            GetItemUI().ReduceAmount(num);
            if (GetItemUI().Amount <= 0)
            {
                DestroyImmediate(GetItemUI().gameObject);
            }
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            if (GetItemUI() != null)
            {
                //Test 将材料放回背包

                InventoryManager.Instance.PickedSlot = this;

                ForgePanel.Instance.btnTips.ShowTips(true, false);


            }
        }
    }
}

