using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HTLibrary.Application;
using System;
namespace HTLibrary.Utility
{
    public class StorePanel : Inventory
    {
        public GameObject buyTipsBoard;
        public Text walletAmountTxt;
        public Text buyGdNumTxt;
        public Slider buyGdNumSld;
        private int buyGdTotalNum = 1;
        private Item purchaseItem;
        private GameMenuePanel gameMenuePanel;

        private int result; //金钱动态变化变量
        private int changeNum;//一次性增加的数字

        private static StorePanel _instance;
        public static StorePanel Instance//单例
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<StorePanel>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 商店拥有的商品
        /// </summary>
        public List<int> goodsIDList = new List<int>();

        [Header("商店物品数量配置")]
        public int SellItemNum = 0;
        public int discountNum = 0;
        [HideInInspector]
        private List<int> discountList = new List<int>();
        [HideInInspector]
        public List<int> selledItems = new List<int>();

        public event Action SellItemEvent;
        private new void OnEnable()
        {
            base.OnEnable();
            //Wallet.Instance.GetCoinAmountEvent += UpdateWalletCoinNum;
            walletAmountTxt.text = HTDBManager.Instance.GetCoins().ToString();
        }

        private new void OnDisable()
        {
            base.OnDisable();
            //Wallet.Instance.GetCoinAmountEvent -= UpdateWalletCoinNum;
        }


        private void Start()
        {
            walletAmountTxt.text = HTDBManager.Instance.GetCoins().ToString();
            gameMenuePanel = GameObject.Find("GameMenuePanel(Clone)").GetComponent<GameMenuePanel>();
            InitShop();
        }

        void InitShop()
        {
            List<int> list = goodsIDList;

            if (list.Count > SellItemNum)
            {
                int randomIndex;
                for (int i = 0; i < list.Count; i++)
                {
                    randomIndex = UnityEngine.Random.Range(0, list.Count);
                    list.RemoveAt(randomIndex);
                    if (list.Count <= SellItemNum)
                    {
                        break;
                    }
                }
            }

            int randomDiscountIndex;

            while (discountList.Count < discountNum)
            {

            id: randomDiscountIndex = UnityEngine.Random.Range(0, list.Count);
                if (discountList.Contains(randomDiscountIndex))
                {
                    goto id;
                }

                discountList.Add(list[randomDiscountIndex]);
            }

            foreach (int itemId in list)
            {
                StoreItem(itemId);
            }
        }

        /// <summary>
        /// 是否是打折ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsDiscountID(int id)
        {
            return discountList.Contains(id);
        }

        public void UpdateWalletCoinNum()
        {
            walletAmountTxt.text = HTDBManager.Instance.GetCoins().ToString();
            Debug.Log("当前钱数： " + HTDBManager.Instance.GetCoins().ToString());
        }

        /// <summary>
        /// 购买物品
        /// </summary>
        /// <param name="item"></param>
        public void BuyItem(Item item)
        {
            purchaseItem = item;
            buyTipsBoard.SetActive(true);
        }

        public void BuyTipsConfirm()
        {
            //bool isPurchaseSuccess = Wallet.Instance.ConsumerCoin
            //((IsDiscountID(purchaseItem.itemID)?purchaseItem.discountPrice:purchaseItem.buyPrice)*buyGdTotalNum);
            bool isPurchaseSuccess = false;
            int currentMoney = HTDBManager.Instance.GetCoins();
            int purchaseItemPrice = (IsDiscountID(purchaseItem.itemID) ? purchaseItem.discountPrice : purchaseItem.buyPrice) * buyGdTotalNum;
            if (currentMoney > purchaseItemPrice)
            {
                isPurchaseSuccess = true;
            }
            if (isPurchaseSuccess)
            {
                // Knapsack.Instance.StoreItem(item,true);
                //TODO 等待本地存储
                Debug.Log("背包数据+" + buyGdTotalNum + " ID:" + purchaseItem.itemID);
                HTDBManager.Instance.AddItemKnapsack(purchaseItem.itemID, buyGdTotalNum);
                HTDBManager.Instance.SaveCoinConsume(purchaseItemPrice);
                gameMenuePanel.UpdatePlayerCoin();
                selledItems.Add(purchaseItem.itemID);
                SellItemEvent?.Invoke();
                //UpdateWalletCoinNum();

                DOTween.To(() => currentMoney
           , v => currentMoney = v, HTDBManager.Instance.GetCoins(), 0.5f).OnUpdate(() =>
           {
               walletAmountTxt.text = Mathf.Floor(currentMoney).ToString();
           }).SetUpdate(true);

            }
            else
            {
                Debug.Log("钱不够 傻子！");
            }
            buyTipsBoard.SetActive(false);
            ReSetTheBuyNum();
        }

        //购买商品金钱动态变化
        IEnumerator MoneyChangeText(int currentVal, int newVal)
        {
            int minVal = currentVal < newVal ? currentVal : newVal;
            int maxVal = currentVal > newVal ? currentVal : newVal;
            if (maxVal - minVal <= 10)
            {
                changeNum = 1;
            }
            else
            {
                changeNum = (maxVal - minVal) / 10;
            }
            result = currentVal;
            if (currentVal > newVal)
            {
                for (int i = minVal; i < maxVal; i++)
                {
                    result = result - changeNum;
                    if (result <= newVal)
                    {
                        break;
                    }
                    walletAmountTxt.text = result.ToString();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            walletAmountTxt.text = newVal.ToString();
            StopCoroutine(MoneyChangeText(currentVal, newVal));
        }
        /// <summary>
        /// id 商品是否已经出售
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsSelledItem(int id)
        {
            return selledItems.Contains(id);
        }

        public void OnBuyGdNumSldChange()
        {
            if (buyGdNumSld.value <= 0) { buyGdNumSld.value = 1; }
            buyGdNumTxt.text = buyGdNumSld.value.ToString();
            buyGdTotalNum = (int)buyGdNumSld.value;
        }

        private void ReSetTheBuyNum()
        {
            buyGdNumTxt.text = "1";
            buyGdNumSld.value = 1;
            buyGdTotalNum = 1;
        }

        public void OnBuyTipsCancel()
        {
            buyTipsBoard.SetActive(false);
            ReSetTheBuyNum();
        }

        public void OnBuyItemById(int id)
        {
            Item item = InventoryManager.Instance.GetItemById(id);

            BuyItem(item);
        }

        /// <summary>
        /// 出售物品
        /// </summary>
        /// <param name="item"></param>
        public void SellItem(Item item)
        {
            Wallet.Instance.EarnCoin(item.sellPrice);
            //TODO 背包物品扣除
        }

        public void SellItemById(int id)
        {
            Item item = InventoryManager.Instance.GetItemById(id);
            SellItem(item);
            //TODO 背包物品扣除
        }
    }

}
