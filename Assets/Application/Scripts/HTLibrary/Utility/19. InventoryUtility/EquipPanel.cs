using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Application;

namespace HTLibrary.Utility
{
    public class EquipPanel :Inventory
    {
        private static EquipPanel _instance;
        public static EquipPanel Instance
        {
            get
            {
                if(_instance==null)
                {
                    _instance = FindObjectOfType<EquipPanel>();
                }

                return _instance;
            }
        }

        public CharacterConfig characterConfigure;

        public Item EquipSlotItemInfo(ItemType itemType)
        {
            Item slotItem = null;
            foreach(Slot eSlot in slotList)
            {
                EquipSlot equipmentSlot = (EquipSlot)eSlot;
                slotItem = equipmentSlot.CurrentItemInfo(itemType);
                if (slotItem != null) break;
            }
            return slotItem;
        }

        public EquipSlot EquipSlotInfo(ItemType itemType)
        {
            EquipSlot equipmentSlot = null;
            foreach (Slot eSlot in slotList)
            {
                equipmentSlot = (EquipSlot)eSlot;
                if(equipmentSlot.itemType == itemType)
                {
                    break;
                }
            }
            return equipmentSlot;
        }

        /// <summary>
        /// 穿上装备
        /// </summary>
        /// <param name="item"></param>
        public void PutOn(Item item, SlotBg slotBg)
        {
            Item exitItem = null;
            foreach (Slot eSlot in slotList)//这里的slot是装备面板的slot
            {
                EquipSlot equipmentSlot = (EquipSlot)eSlot;
                if (equipmentSlot.IsRightItem(item))
                {
                    equipmentSlot.GetComponent<Animator>().SetTrigger("equiped");
                    if (equipmentSlot.transform.GetComponentInChildren<ItemUI>()!=null)//判断装备槽下是否有装备
                    {
                        ItemUI currentItemUI = equipmentSlot.transform.GetComponentInChildren<ItemUI>();
                        exitItem = currentItemUI.Item;
                        ChangeProperty(false, exitItem);

                        currentItemUI.SetItem(item, 1);
                        equipmentSlot.ChangeSlotItemType(item);
                        ChangeProperty(true, item);
                        HTDBManager.Instance.AddEquip(item.itemID);
                    }
                    else
                    {
                        equipmentSlot.StoreItem(item,false);
                        HTDBManager.Instance.AddEquip(item.itemID);
                        ChangeProperty(true, item);
                    }
                    equipmentSlot.GetComponentInChildren<ItemUI>().transform.SetSiblingIndex(2);
                    equipmentSlot.ChangeSlotBackground(true, slotBg);
                    break;
                }
            }
            if (exitItem != null)
            {
                //Debug.Log(exitItem.itemTypeSprite);
                Knapsack.Instance.StoreItem(exitItem,true);
                HTDBManager.Instance.AddItemKnapsack(exitItem.itemID);
                HTDBManager.Instance.RemoveEquip(exitItem.itemID);
            }
        }

        /// <summary>
        /// 脱掉装备
        /// </summary>
        /// <param name="item"></param>
        public void PutOff(Item item)
        {
            Knapsack.Instance.StoreItem(item,true);
            HTDBManager.Instance.RemoveEquip(item.itemID);
            ChangeProperty(false, item);
        }

        private void ChangeProperty(bool positive,Item item)
        {
            if(!positive)
            {
                characterConfigure.ChangeProperty(-item.hp, -item.Attack, -item.Defence, -item.Crit, -item.Dodge, -item.MoveSpeed, -item.AttackSpeed);
            }
            else
            {
                characterConfigure.ChangeProperty(item.hp, item.Attack, item.Defence, item.Crit, item.Dodge, item.MoveSpeed, item.AttackSpeed);
            }
        }

        public override void SaveInventory()
        {
            base.SaveInventory();

            PlayerPrefs.SetString(Consts.EquipPanel+SaveManager.Instance.LoadGameID, sb.ToString());
        }

        protected override void ClearAllSlot()
        {
            foreach(var slot in slotList)
            {
                ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
                if(itemUI!=null)
                {
                    Destroy(itemUI.gameObject);
                }
            }
        }

        public override void LoadInventory()
        {
            base.LoadInventory();
            foreach (var id in HTDBManager.Instance.equip)
            {
              Item item= InventoryManager.Instance.GetItemById(id);
              InitialEquipPanel(item);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            LoadInventory();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SaveInventory();
        }

        void InitialEquipPanel(Item item)
        {
            foreach (Slot eSlot in slotList)//这里的slot是装备面板的slot
            {
               
                EquipSlot equipmentSlot = (EquipSlot)eSlot;

                if (equipmentSlot.IsRightItem(item))
                {

                    ItemUI itemUI = equipmentSlot.GetComponentInChildren<ItemUI>();
                    if(itemUI!=null)
                    {
                       DestroyImmediate(itemUI.gameObject);
                    }

                    equipmentSlot.StoreItem(item, false);

                    equipmentSlot.GetComponentInChildren<ItemUI>().transform.SetSiblingIndex(2);
                    equipmentSlot.ChangeSlotBackground(true,equipmentSlot.ChooseSlotBg(item));
                }
            }
        }
    }

}
