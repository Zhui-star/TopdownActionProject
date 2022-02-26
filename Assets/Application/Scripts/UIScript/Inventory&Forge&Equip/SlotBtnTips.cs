using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTLibrary.Application;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Utility
{
    public class SlotBtnTips : MonoBehaviour
    {
        public GameObject shotDownTipsBtnGo;
        private GameObject equipBtnGo;
        private GameObject unwieldBtnGo;

        [Header("Forge panel")]
        public GameObject putOffFromForgePanelBtnGo;
        public GameObject putOnToForgeBtnGo;
        private bool isBtnActive = false;
        private GameObject sellBtnGo;
        private GameObject dropBtnGo;
        private Text equipBtnTxt;
        private bool isWeapon;

        public AudioClip equipSoundClip;
        private Canvas uiCanvas;

        void Start()
        {
            equipBtnGo = transform.Find("Equip_Btn").gameObject;
            unwieldBtnGo = transform.Find("Unwield_Btn").gameObject;
            dropBtnGo = transform.Find("Drop_Btn").gameObject;
            sellBtnGo = transform.Find("Sell_Btn").gameObject;
            equipBtnTxt = transform.Find("Equip_Btn/Text").GetComponent<Text>();
            uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            this.gameObject.SetActive(false);
        }

        public void ShowTips(bool isShow, bool isWeapon)
        {
            this.isWeapon = isWeapon;
            isBtnActive = isShow;
            bool isEquipSlot = InventoryManager.Instance.PickedSlot.isEquipSlot;
            /* RectTransform slotTr = InventoryManager.Instance.PickedSlot.GetComponent<RectTransform>();
             Vector2 position = slotTr.anchoredPosition+ new Vector2(115f,90f);
             (transform as RectTransform).anchoredPosition = position;*/
            bool isForgeSlot = InventoryManager.Instance.PickedSlot.isForgeSlot;

            Vector2 position;
            if (uiCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle((uiCanvas.transform as RectTransform),
              Input.mousePosition, uiCanvas.worldCamera, out position);
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle((uiCanvas.transform as RectTransform),
          Input.mousePosition, null, out position);
            }

            position += new Vector2(95f, -65f);
            transform.localPosition = position;

            if (!Knapsack.Instance.IsGameScenes)
            {
              

                switch(InventoryManager.Instance.invetoryType)
                {
                    case InventoryType.None:
                        break;
                    case InventoryType.Equip:
                        equipBtnGo.SetActive(!isEquipSlot);
                        unwieldBtnGo.SetActive(isEquipSlot);
                        putOnToForgeBtnGo.SetActive(false);
                        putOffFromForgePanelBtnGo.SetActive(false);
                        if (isWeapon)
                        {
                            equipBtnTxt.text = "预览";
                        }
                        else
                        {
                            equipBtnTxt.text = "装备";
                        }
                        break;
                    case InventoryType.Forge:
                        putOnToForgeBtnGo.SetActive(!isForgeSlot);
                        equipBtnGo.SetActive(false);
                        unwieldBtnGo.SetActive(false);
                        putOffFromForgePanelBtnGo.SetActive(isForgeSlot);
                        break;
                    default:
                        break;
                }
              

               

            }
            else
            {
                equipBtnGo.SetActive(!isEquipSlot);
                equipBtnTxt.text = "装备";
            }

            this.gameObject.SetActive(isShow);
            shotDownTipsBtnGo.SetActive(this.gameObject.activeInHierarchy);//click everywhere can shot down the tips

        }

        public void OnShowDownTipsClick()
        {
            if (isBtnActive)
            {
                ShowTips(false, isWeapon);
            }


        }

        public void OnEquipClick()
        {
            if (isBtnActive)
            {
                if (!Knapsack.Instance.IsGameScenes)
                {
                    InventoryManager.Instance.PickedSlot.WearEquipment();
                    ShowTips(false, isWeapon);
                }
                else
                {
                    if (HTDBManager.Instance.ReturnCurrentWeapon() != null)
                    {
                        HTDBManager.Instance.RemoveEquip(HTDBManager.Instance.ReturnCurrentWeapon().itemID);
                    }

                    HTDBManager.Instance.AddEquip(InventoryManager.Instance.PickedSlot.GetItem().itemID);

                    if (equipSoundClip != null)
                    {
                        SoundManager.Instance.PlaySound(equipSoundClip, transform.localPosition, false);
                    }

                    UIManager.Instance.PopPanel();
                }

            }
        }
        public void OnUnwieldClick()
        {
            if (isBtnActive)
            {
                EquipSlot pickedSlot = (EquipSlot)InventoryManager.Instance.PickedSlot;
                Item pickedItem = pickedSlot.GetComponentInChildren<ItemUI>().Item;
                pickedSlot.ChangeSlotBackground(false, null);
                EquipPanel.Instance.PutOff(pickedItem);
                HTDBManager.Instance.AddItemKnapsack(pickedItem.itemID);
                pickedSlot.UseItem(1);
                ShowTips(false, isWeapon);
            }
        }

        public void OnSellItemClick()
        {
            if (isBtnActive)
            {
                Slot pickedSlot = InventoryManager.Instance.PickedSlot;
                Item item = pickedSlot.GetItem();
                HTDBManager.Instance.RemoveKanpsack(pickedSlot.GetItemId());
                Wallet.Instance.EarnCoin(item.sellPrice);
                Knapsack.Instance.LoadInventory();
                ShowTips(false, isWeapon);
            }
        }

        public void OnDropItemClick()
        {
            //TODO丢弃物品
        }

        /// <summary>
        /// 将物品放入合成面板
        /// </summary>
        public void OnPutOnForgePanelClick()
        {
            if(isBtnActive)
            {
                if (ForgePanel.Instance.IsOverload()) return;

                InventoryManager.Instance.PickedSlot.PutOnItemIntoForgePanel();

                ShowTips(false, false);
            }

        }

        /// <summary>
        /// 从合成面板取回
        /// </summary>
        public void OnPutOffItemFromForgePanel()
        {
            if(isBtnActive)
            {
              
                Item exitItem = InventoryManager.Instance.PickedSlot.GetItem();

                ForgeSlot forgeSlot = (InventoryManager.Instance.PickedSlot as ForgeSlot);  

                ForgePanel.Instance.PutOff(exitItem);

                HTDBManager.Instance.AddItemKnapsack(exitItem.itemID);

                forgeSlot.ChangeSlotBackground(false, null);

                ShowTips(false, false);

                forgeSlot.UseItem(1);

            }
        }

    }
}
