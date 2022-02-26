using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Test;
using System.Text;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 背包
    /// </summary>
    public class Inventory : MonoBehaviour
    {
 
        public List<Slot> slotList = new List<Slot>();//当前Panel 下Slot 的集合
        public GameObject slot;//Slot预制体
        public Transform slotParent;//Slot 的父物体
        [Header("组件")]
        public ToolTips toolTips;//提示框引用
        public SlotBtnTips btnTips;//提示选项框

        protected StringBuilder sb;


        /// <summary>
        /// 存储物品通过物品ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool StoreItem(int id,bool isPutOff=false)
        {
            Item item = InventoryManager.Instance.GetItemById(id);
            return StoreItem(item, isPutOff);
        }

        /// <summary>
        /// 存储物品通过Item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool StoreItem(Item item, bool isPutOff = false)
        {
            if (item == null)
            {
                Debug.LogWarning("要存储的物品的id不存在");
                return false;
            }
            if (item.itemCapacity == 1)
            {
                Slot findSlot = FindEmptySlot();
                if (findSlot == null)
                {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
                else
                {
                    if (isPutOff)
                    {
                        List<GameObject> goList = Knapsack.Instance.hideSlotItemGoList;
                        bool isSetSib = false;
                        for (int i = 0; i <= goList.Count-1; i++)
                        {
                            Item goItem = goList[i].GetComponentInChildren<ItemUI>().Item;
                            if (goItem.itemType == item.itemType && goItem.itemQuality == item.itemQuality)
                            {
                                isSetSib = true;
                                findSlot.transform.SetSiblingIndex(i+1);
                                break;
                            }
                        }
                        if (!isSetSib)
                        {
                            int finalSameTypeItemIndex = 0;
                            for (int i = 0; i <= goList.Count - 1; i++)
                            {
                                Item goItem = goList[i].GetComponentInChildren<ItemUI>().Item;
                                if (item.itemType == goItem.itemType && item.itemQuality < goItem.itemQuality)
                                {
                                    finalSameTypeItemIndex = i;
                                }
                            }
                            findSlot.transform.SetSiblingIndex(finalSameTypeItemIndex+1);
                        }
                    }
                    findSlot.StoreItem(item, isPutOff);//把物品存储到这个空的物品槽里面
                    slotList.Add(findSlot);

                    UpdateSlotList();

                }
            }
            else
            {
                Slot findSlot = FindSameIdSlot(item);
                if (slot != null)
                {
                    findSlot.StoreItem(item,false);
                }
                else
                {
                    Slot emptySlot = FindEmptySlot();
                    slotList.Add(emptySlot);

                    if (emptySlot != null)
                    {
                        emptySlot.StoreItem(item,false);
                    }
                    else
                    {
                        Debug.LogWarning("没有空的物品槽");
                        return false;
                    }
                }
            }
            return true;
        }


        void UpdateSlotList()
        {
            slotList.Clear();

           for(int i=0;i<slotParent.childCount;i++)
            {
                slotList.Add(slotParent.GetChild(i).GetComponent<Slot>());
            }
        }

        /// <summary>
        /// 这个方法用来找到一个空的物品槽(生成一个空的物品槽）
        /// </summary>
        /// <returns></returns>
        private Slot FindEmptySlot()
        {
            GameObject newSlot = Instantiate(this.slot, slotParent);
            return newSlot.GetComponent<Slot>();
        }

        private Slot FindSameIdSlot(Item item)
        {
            foreach (Slot findSlot in slotList)
            {
                if (slot.transform.childCount >= 1 && findSlot.GetItemId() == item.itemID && findSlot.IsFilled() == false)
                {
                    return findSlot;
                }
            }
            return null;
        }


        public virtual void SaveInventory()
        {
            sb = new StringBuilder();
            foreach (Slot slot in slotList)
            {
                if (slot == null) continue;
                ItemUI itemUI = slot.transform.GetComponentInChildren<ItemUI>();
                if (itemUI != null)
                {
                    sb.Append(itemUI.Item.itemID + "-");
                }
                else
                {
                    sb.Append("0-");
                }
            }

        }

        public virtual void LoadInventory()
        {
            
        }

        protected virtual void OnEnable()
        {
          //  LoadInventory();
        }

        protected virtual void OnDisable()
        {
           // SaveInventory();
        }

        protected  virtual void ClearAllSlot()
        {
            slotList.Clear();

            for(int i=0;i<slotParent.childCount;i++)
            {
               Destroy(slotParent.GetChild(i).gameObject);
            }
        }
    }

}
