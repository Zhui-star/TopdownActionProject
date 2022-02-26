using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    public enum InventoryType
    {
        None,
        Equip,
        Forge
    }
    /// <summary>
    /// 背包系统管理
    /// </summary>
    public class InventoryManager :MonoSingleton<InventoryManager>
    {
        public ItemList itemList;

        private ItemUI pickedItem;//鼠标选中的物体

        private Canvas canvas;

        private ItemUI PickedItem
        {
            get
            {
                return pickedItem;
            }

            set
            {
                pickedItem = value;
            }
        }

        //当前所选中的Item 数据
        private Item itemPickedItem { get; set; }
        //当前所选中的Slot
        public Slot PickedSlot { set; get; }

        [HideInInspector]
        public InventoryType invetoryType;


        [SerializeField]
        private List<int> _activatedItems = new List<int>();

        /// <summary>
        /// 得到物品通过ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item GetItemById(int id)
        {
            foreach (Item item in itemList.itemList)
            {
                if (item.itemID == id)
                {
                    return item;
                }
            }
            return null;
        }

        //捡起物品槽指定数量的物品
        public void PickupItem(Item item)
        {
            itemPickedItem = item;
        }

        /// <summary>
        /// 从手上拿掉一个物品放在物品槽里面
        /// </summary>
        public void RemoveItem(int amount = 1)
        {
            PickedItem.ReduceAmount(amount);
            if (PickedItem.Amount <= 0)
            {
                PickedItem.Hide();
            }
        }

        /// <summary>
        /// 得到随机可交互ID
        /// </summary>
        /// <returns></returns>
        public int GetRandomActivatedItemId()
        {
            int id=1;

            if(_activatedItems.Count>0)
            {
                id = _activatedItems[Random.Range(0, _activatedItems.Count)];
            }

            return id;
        }
    }

}
