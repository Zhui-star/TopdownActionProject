using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using System.Linq;
using System.Text;
using System;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    public class OwnItems : IComparable
    {
        public int id;
        public int itemQuality;
        public OwnItems(int id, int itemQuality)
        {
            this.id = id;
            this.itemQuality = itemQuality;

        }

        public int CompareTo(object obj)
        {
            OwnItems items = (OwnItems)obj;
            if (itemQuality <= items.itemQuality && id <= items.id)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
    public class HTDBManager : MonoSingleton<HTDBManager>
    {
        public List<int> knapsack = new List<int>();
        public List<int> equip = new List<int>();
        public event Action<int> AddEquipEvent;
        public event Action<int> RemoveEquipEvent;
        public event Action UpdateCoinEvent;

        [HideInInspector]
        public bool InitialOver = false;

        [Header("测试模式")]
        public bool TestModel = false;

        List<int> _pickedEquipList = new List<int>();

        public void Awake()
        {
            if(TestModel)
            {
                Initial();
            }
        }
        public void Initial()
        {
            InitialKnapsack();
            InitialEquip();
            InitPlayerCoin();
           // InitialNewPlayerKnapsack();
        }

        /// <summary>
        /// 初始化新手背包
        /// </summary>
        public void InitialNewPlayerKnapsack()
        {
            if (!PlayerPrefs.HasKey(Consts.archerWeapon + SaveManager.Instance.LoadGameID))
            {
                AddItemKnapsack(1);
                PlayerPrefs.SetFloat(Consts.archerWeapon + SaveManager.Instance.LoadGameID, 1); //送一个短弓
            }

            if (!PlayerPrefs.HasKey(Consts.magicianWeapon + SaveManager.Instance.LoadGameID))
            {
                AddItemKnapsack(5);
                PlayerPrefs.SetFloat(Consts.magicianWeapon + SaveManager.Instance.LoadGameID, 1); //送一个魔法棒
            }

            if (!PlayerPrefs.HasKey(Consts.sowrdWeapon + SaveManager.Instance.LoadGameID))
            {
                AddItemKnapsack(6);
                PlayerPrefs.SetFloat(Consts.sowrdWeapon + SaveManager.Instance.LoadGameID, 1); //送一个短剑
            }

            if (!PlayerPrefs.HasKey(Consts.shieldWeapon + SaveManager.Instance.LoadGameID))
            {
                AddItemKnapsack(4);
                PlayerPrefs.SetFloat(Consts.shieldWeapon + SaveManager.Instance.LoadGameID, 1); //送一盾
            }
            PlayerPrefs.Save();
        }


        public void InitialKnapsack()
        {
            knapsack.Clear();
            if (!PlayerPrefs.HasKey(Consts.Knapsack + SaveManager.Instance.LoadGameID)) return;
            string str = PlayerPrefs.GetString(Consts.Knapsack + SaveManager.Instance.LoadGameID);
            string[] itemArray = str.Split('-');

            for (int i = 0; i < itemArray.Length - 1; i++)
            {
                string itemStr = itemArray[i];
                if (itemStr != "0")
                {
                    //print(itemStr);
                    string[] temp = itemStr.Split(',');
                    int id = int.Parse(temp[0]);
                    knapsack.Add(id);
                }
            }


        }

        /// <summary>
        /// 添加物品进入数据库
        /// </summary>
        /// <param name="id"></param>
        public void AddItemKnapsack(int id, int num = 1)
        {
            for (int i = 0; i < num; i++)
            {
                knapsack.Add(id);
            }
        }

        /// <summary>
        /// 移除物品从数据库
        /// </summary>
        /// <param name="id"></param>
        public void RemoveKanpsack(int id)
        {
            knapsack.Remove(id);
        }

        public bool SelectKanpsack(int id)
        {
            return knapsack.Contains(id);
        }

        public void InitialEquip()
        {
            equip.Clear();
            if (!PlayerPrefs.HasKey(Consts.EquipPanel + SaveManager.Instance.LoadGameID)) return;
            string str = PlayerPrefs.GetString(Consts.EquipPanel + SaveManager.Instance.LoadGameID);
            string[] itemArray = str.Split('-');

            for (int i = 0; i < itemArray.Length - 1; i++)
            {
                string itemStr = itemArray[i];
                if (itemStr != "0")
                {
                    //print(itemStr);
                    string[] temp = itemStr.Split(',');
                    int id = int.Parse(temp[0]);
                    AddEquip(id);
                }
            }


        }

        public void SortKnapsack()
        {
            List<OwnItems> ownItemList = new List<OwnItems>();

            for (int i = 0; i < knapsack.Count; i++)
            {
                Item item = InventoryManager.Instance.GetItemById(knapsack[i]);

                OwnItems ownItem = new OwnItems(item.itemID, (int)item.itemQuality);

                ownItemList.Add(ownItem);
            }

            ownItemList.Sort((OwnItems x, OwnItems y) => x.CompareTo(y));

            knapsack.Clear();

            for (int i = 0; i < ownItemList.Count; i++)
            {
                knapsack.Add(ownItemList[i].id);
            }

        }

       public void SaveKnapsack()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < knapsack.Count; i++)
            {
                sb.Append(knapsack[i] + "-");
            }
            PlayerPrefs.SetString(Consts.Knapsack + SaveManager.Instance.LoadGameID, sb.ToString());
        }

        public List<int> SelectHeroWeapon(WeaponType type)
        {
            List<int> heroWeapons = new List<int>();
            foreach (var temp in knapsack)
            {
                Item item = InventoryManager.Instance.GetItemById(temp);
                if (item.weaponTypeList.Count <= 0)
                {
                    continue;
                }

                if (item.weaponTypeList.Contains(type))
                {
                    heroWeapons.Add(temp);
                }
            }
            return heroWeapons;
        }

        public int SelectBestWepaon(List<int> weaponList)
        {
            int bestQuality = -1;
            int weaponId = 0;

            foreach (var temp in weaponList)
            {

                Item item = InventoryManager.Instance.GetItemById(temp);
                if ((int)item.itemQuality > bestQuality)
                {
                    bestQuality = (int)item.itemQuality;
                    weaponId = temp;
                }
            }

            return weaponId;
        }

        public bool NeedWeaponEuip()
        {
            if (equip.Count <= 0)
            {
                return true;
            }

            Item currentWeapon = null;

            foreach (var temp in equip)
            {
                Item item = InventoryManager.Instance.GetItemById(temp);
                if (item.itemType == ItemType.Weapon)
                {
                    currentWeapon = item;
                    break;
                }
            }

            if (currentWeapon == null)
            {
                return true;
            }

            return false;
        }

        private ItemQuality ReturnCurrentWeaponQualtiy()
        {
            foreach (var temp in equip)
            {
                Item item = InventoryManager.Instance.GetItemById(temp);
                if (item.itemType == ItemType.Weapon)
                {
                    return item.itemQuality;
                }
            }

            return ItemQuality.None;
        }

        public Item ReturnCurrentWeapon()
        {
            foreach (var temp in equip)
            {
                Item item = InventoryManager.Instance.GetItemById(temp);
                if (item.itemType == ItemType.Weapon)
                {
                    return item;
                }
            }
            return null;
        }

        public bool CheckCurrentIsBestWeapon(WeaponType type)
        {
            int knapackBestId = SelectBestWepaon(SelectHeroWeapon(type));
            Item item = InventoryManager.Instance.GetItemById(knapackBestId);
            if (item == null)
            {
                return true;
            }


            if (item.itemQuality > ReturnCurrentWeaponQualtiy())
            {
                return false;
            }

            return true;
        }

        private void OnDisable()
        {
            SaveKnapsack();
        }

        public void RemoveEquip(int id)
        {
            equip.Remove(id);

            if (RemoveEquipEvent != null)
            {
                RemoveEquipEvent(id);
            }
        }

        public void AddEquip(int id)
        {
            equip.Add(id);

            if (AddEquipEvent != null)
            {
                AddEquipEvent(id);
            }
        }

        public void ResetWeapon()
        {
            Item item = ReturnCurrentWeapon();
            if (item != null)
            {
                equip.Remove(item.itemID);
            }
        }

        /// <summary>
        /// 捡起装备
        /// </summary>
        /// <param name="id"></param>
        public void  PickUpEquipment(int id)
        {
            _pickedEquipList.Add(id);

            AddItemKnapsack(id);
        }

        /// <summary>
        /// 得到当前捡到的装备清单
        /// </summary>
        /// <returns></returns>
        public List<int> GetPickUpEquipList()
        {
            return _pickedEquipList;
        }

        /// <summary>
        /// 清空见到的装备清单
        /// </summary>
        public void ClearPickUpList()
        {
            _pickedEquipList.Clear();
        }

        /// <summary>
        /// 移除捡到的装备中其中一个ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemovePickUpItem(int id)
        {
            bool success = false;
            if(_pickedEquipList.Contains(id))
            {
                _pickedEquipList.Remove(id);
                success = true;
            }

            return success;
        }

        /// <summary>
        /// 返回当前身上装备
        /// </summary>
        /// <returns></returns>

        public List<int> GetEquips()
        {
            return equip;
        }

        public void InitPlayerCoin()
        {
            if (!PlayerPrefs.HasKey(Consts.Coin+SaveManager.Instance.LoadGameID))
            {
                PlayerPrefs.SetInt(Consts.Coin + SaveManager.Instance.LoadGameID, 100);
            }
        }

        public void SaveCoinGet(int value)
        {
            PlayerPrefs.SetInt(Consts.Coin + SaveManager.Instance.LoadGameID,
                PlayerPrefs.GetInt(Consts.Coin+SaveManager.Instance.LoadGameID) + value);
            UpdateCoinEvent();
        }

        public void SaveCoinConsume(int value)
        {
            PlayerPrefs.SetInt(Consts.Coin + SaveManager.Instance.LoadGameID, PlayerPrefs.GetInt(Consts.Coin+SaveManager.Instance.LoadGameID) - value);
            UpdateCoinEvent();
        }

        //获取现有的金币数量
        public int GetCoins()
        {
            return PlayerPrefs.GetInt(Consts.Coin + SaveManager.Instance.LoadGameID);
        }
    }
}


