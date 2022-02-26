using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Application;
using DG.Tweening;
namespace HTLibrary.Utility
{
 

    public class Knapsack :Inventory
    {
        ScrollRect _inventoryScroll;

        [HideInInspector]//这里的数组可以方便浏览背包物品，方便查询错误
        public List<GameObject> hideSlotItemGoList = new List<GameObject>();
        [HideInInspector]
        public bool isShowTheEquipTips = false;
        public GameObject entireInventoryGo;

        public bool IsGameScenes = true;

        public InventoryMenuePanel inventoryMenuePanel;

        #region 单例模式
        private static Knapsack _instance;
        public static Knapsack Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Knapsack>();
                }
                return _instance;
            }
        }
        #endregion

        private void Start()
        {
            if(!IsGameScenes)
            {
                PutOffWeaponSlot();
            }
        }

        public void OnEnterCallBack()
        {
            if (!IsGameScenes)
            {
                InventoryBtnManager.Instance.InventoryButtonClickEvent += ShowTheListGo;
                LoadInventory();
                PutOffWeaponSlot();
            }

            UpdateInventoryScroll();
        }

        public void OnExitCallBack()
        {
            if (!IsGameScenes)
            {
                InventoryBtnManager.Instance.InventoryButtonClickEvent -= ShowTheListGo;
            }
        }

        public void ShowTheListGo(bool isShow, ItemType itemType)
        {
            if(hideSlotItemGoList!=null)
            {
                foreach (GameObject go in hideSlotItemGoList)
                {
                    if (InventoryBtnManager.Instance.currentBtnType == itemType)
                    {
                        Item currentItem = go.transform.GetComponentInChildren<ItemUI>().Item;
                        go.SetActive(currentItem.itemType == itemType);
                    }
                }
            }
        }


        public override void LoadInventory()
        {
            ClearAllSlot();
            HTDBManager.Instance.SortKnapsack();
            if(!IsGameScenes)
            {
                hideSlotItemGoList.Clear();
                foreach (var id in HTDBManager.Instance.knapsack)
                {
                    StoreItem(id);
                }
            }
            else
            {
                CharacterIdentity characterIdentity = CharacterManager.Instance.GetCharacter("Player1").GetComponent<CharacterIdentity>();
                List<int> heroList = HTDBManager.Instance.SelectHeroWeapon(characterIdentity.heroType);
                foreach(var temp in heroList)
                {
                    StoreItem(temp);
                }
            }
        }
          

        private void PutOffWeaponSlot()
        {
            if (EquipPanel.Instance == null) return;
            EquipSlot equipSlot = EquipPanel.Instance.EquipSlotInfo(ItemType.Weapon);
            Item weaponItem = EquipPanel.Instance.EquipSlotItemInfo(ItemType.Weapon);
            if (weaponItem!=null)
            {
                equipSlot.ChangeSlotBackground(false, null);
                HTDBManager.Instance.AddItemKnapsack(weaponItem.itemID);
                EquipPanel.Instance.PutOff(weaponItem);
                equipSlot.UseItem(1);
            }
        }

        public void OnKnapsackCloseClick()
        {
            PutOffWeaponSlot();
            // entireInventoryGo.SetActive(false);
            UIManager.Instance.PopPanel();
        }

        /// <summary>
        /// Scroll start vertical position in top 
        /// </summary>
        void UpdateInventoryScroll()
        {
            if(!_inventoryScroll)
            {
                _inventoryScroll=GetComponent<ScrollRect>();
            }

            _inventoryScroll.content.DOLocalMoveY(0,0.2f);
        }
    }

}
