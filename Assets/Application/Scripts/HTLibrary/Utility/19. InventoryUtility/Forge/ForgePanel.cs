using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTLibrary.Application;

namespace HTLibrary.Utility
{
    public class ForgePanel : Inventory
    {
        private static ForgePanel instance;
        public static ForgePanel Instance
        {
            get
            {
                return instance;
            }
        }

        public FormularConfigure configure;

        private List<Formular> formularList;

        public Text tipsTxt;

        public ResSlot resSlot;

        [HideInInspector]
        public int ResId;

        public ForgeAnimPanel animPanel;

        public Button synthesisBtn;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            formularList = configure.formulars;
            synthesisBtn.interactable = false;
            CheckSynthesisBtnState(false);
        }

        /// <summary>
        /// 检测合成按钮状态
        /// </summary>
        /// <param name="state"></param>
        void CheckSynthesisBtnState(bool state)
        {
            synthesisBtn.interactable = state;
            if(state)
            {
                synthesisBtn.image.color = Color.grey;
            }
            else
            {
                synthesisBtn.image.color = Color.white;
            }
        }


        public bool PutOn(Item item)
        {
            bool puton = false;
            foreach (Slot slot in slotList)
            {
                ForgeSlot forgeSlot = (ForgeSlot)slot;

                if (forgeSlot.GetItemUI() == null)
                {
                    forgeSlot.ChangeSlotItemType(item);
                    forgeSlot.ChangeSlotBackground(true, item);
                    forgeSlot.StoreItem(item, true);
                    puton = true;
                    break;
                }
            }

            bool IsNull = false;
            foreach (Slot slot in slotList)
            {
                if (slot.GetItemUI() == null)
                {
                    IsNull = true;
                }
            }

            if (!IsNull)
            {
                int id = CheckFormularRes();
                if (id != -1)
                {
                    Item resItem = InventoryManager.Instance.GetItemById(id);
                    resSlot.ChangeSlotItemType(resItem);
                    resSlot.ChangeSlotBackground(true, resItem);
                    resSlot.StoreItem(InventoryManager.Instance.GetItemById(id), false);
                }
            }
            else
            {
                if (resSlot.GetItemUI() != null)
                {
                    resSlot.ChangeSlotBackground(false, null);
                    Destroy(resSlot.transform.GetComponentInChildren<ItemUI>().gameObject);
                }
            }

            return puton;
        }

        public void PutOff(Item item)
        {
            Knapsack.Instance.StoreItem(item, true);

            ItemUI itemUI = resSlot.GetItemUI();

            resSlot.ChangeSlotBackground(false, null);

            if (itemUI != null)
            {
                DestroyImmediate(itemUI.gameObject);
            }

            CheckSynthesisBtnState(false);
        }

        public void SynthesisClick()
        {
            int id = CheckFormularRes();
            ResId = id;

            if (id != -1)
            {
                foreach (var temp in slotList)
                {
                    (temp as ForgeSlot).ChangeSlotBackground(false, null);
                    DestroyImmediate(temp.GetItemUI().gameObject);
                }

                resSlot.ChangeSlotBackground(false, null);

                Destroy(resSlot.GetItemUI().gameObject);

                HTDBManager.Instance.AddItemKnapsack(id);

                Knapsack.Instance.StoreItem(InventoryManager.Instance.GetItemById(id), true);
                CheckSynthesisBtnState(false);
            }

            animPanel.TransformState(true);
        }

        /// <summary>
        /// 检查配方
        /// </summary>
        public int CheckFormularRes()
        {
            List<int> itemIDs = new List<int>();

            foreach (var temp in slotList)
            {
                ItemUI itemUI = temp.GetItemUI();

                if (itemUI == null)
                {
                    if (tipsTxt != null)
                    {
                        tipsTxt.text = "请放入相同品质装备";
                    }
                    return -1;
                }
                itemIDs.Add(itemUI.Item.itemID);
            }

            bool isMatch = false;

            foreach (var temp in formularList)
            {
                if (temp.IsMatch(itemIDs))
                {
                    isMatch = true;
                    tipsTxt.text = "提示:相同品质装备可以合成更品质装备";
                    CheckSynthesisBtnState(true);
                    return temp.ResID;
                }
            }

            if (!isMatch)
            {
                tipsTxt.text = "请放入相同品质装备";
            }

            return -1;
        }

        /// <summary>
        /// 是否当前已有装备在合成栏
        /// </summary>
        /// <returns></returns>
        public bool IsOverload()
        {
            foreach (var temp in slotList)
            {
                ItemUI itemUI = temp.GetItemUI();

                if (itemUI == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 关闭面板时是否存在有物品在合成面板栏
        /// </summary>
        /// <returns></returns>
        public void ClearExistItemWhenClosePanel()
        {
            foreach (var temp in slotList)
            {

                ItemUI itemUI = temp.GetItemUI();

                if (itemUI != null)
                {
                    Item exitItem = itemUI.Item;
                    PutOff(exitItem);

                    HTDBManager.Instance.AddItemKnapsack(exitItem.itemID);

                    (temp as ForgeSlot).ChangeSlotBackground(false, null);

                    (temp as ForgeSlot).UseItem(1);
                }
            }
        }


    }

}
