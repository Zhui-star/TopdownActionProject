using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HTLibrary.Utility
{
    public class EquipSlot : Slot
    {
        public Button closeInventoryBtn;

        public ItemType itemType;//装备槽对应的物品类型

        public Sprite slotOriginBg;//装备槽原本的背景

        public bool IsRightItem(Item item)
        {
            if (item.itemType == itemType)
            {
                return true;
            }

            return false;
        }

        public Item CurrentItemInfo(ItemType itemType)
        {
            Item currentItem = null;
            if (GetComponentInChildren<ItemUI>() != null)
            {
                currentItem = GetComponentInChildren<ItemUI>().Item;
                if (currentItem.itemType == itemType)
                {
                    return currentItem;
                }
            }
            return null;
        }

        protected override void Start()
        {
            base.Start();

            closeInventoryBtn = transform.parent.transform.parent.transform.Find("Close_Btn").GetComponent<Button>();
        }

        public void ChangeSlotItemType(Item currentItem)
        {
            iconTypeImage.sprite = currentItem.itemTypeSprite;
        }

        public void ChangeSlotBackground(bool isChange, SlotBg slotBg)
        {
            circleBgImg.gameObject.SetActive(isChange);
            if (isChange)
            {
                slotBgImg.sprite = slotBg.slotBgSprite;
                circleBgImg.sprite = slotBg.circleBgSprite;
            }
            else
            {
                slotBgImg.sprite = slotOriginBg;
            }
        }

        /// <summary>
        /// 鼠标点击的交互
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            image.color = clickColor;
            ItemUI itemUI = GetItemUI();

            if (itemUI == null) return;
            InventoryManager.Instance.PickedSlot = this;


            bool isWeapon = itemUI.Item.itemType == ItemType.Weapon ? true : false;
            Knapsack.Instance.btnTips.ShowTips(true, isWeapon);

        }

        public override void UseItem(int num = 1)
        {
            GetItemUI().ReduceAmount(num);
            if (GetItemUI().Amount <= 0)
            {
                DestroyImmediate(GetItemUI().gameObject);
            }
        }

        public override void Awake()
        {
            base.Awake();
            isEquipSlot = true;
        }

        public void OnAnimateStart()
        {
            closeInventoryBtn.interactable = false;
        }

        public void OnAnimateFinished()
        {
            closeInventoryBtn.interactable = true;
        }
    }


}
