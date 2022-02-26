using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NaughtyAttributes;
using HTLibrary.Application;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 背包格子
    /// </summary>
    public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        public GameObject itemPrefab;// Item 的预制体

        [HideInInspector]
        public Image slotBgImg;//Slot 的背景图组件
        [HideInInspector]
        public Image circleBgImg;//Slot 物品种类的背景图组件
        protected Image image; //Slot 当下图片组件
        public bool isStoreGoods = false;//是否为商店的slot
        [HideInInspector]
        public bool isEquipSlot = false;
        [HideInInspector]
        public bool isForgeSlot = false;
        [HideInInspector]
        public bool isResSlot = false;
        [HideInInspector]
        public Image iconTypeImage;// 物品类型图片
        public string slotItemName;

        [Header("属性")]
        public Color normalColor;
        public Color highlightColor;
        public Color clickColor;

        [Header("品质背景图片")]
        [ReorderableList]
        public List<SlotBg> slotBg;//武器品质背景图片

        //商店slot的信息物体
        private Text gdNameTxt;
        private Text gdDmgTxt;
        private Text gdDefTxt;
        private Text gdCriticTxt;
        private Text gdDodTxt;
        private Text gdDescrpTxt;
        private Text gdPriceTxt;
        private Text gdPrePriceTxt;
        private Button gdPurchaseBtn;
        private GameObject gdDisPreButtonGo;
        private GameObject gdDisButtonGo;
        private GameObject gdDisLableGo;
        [Header("商品Slot 设置")]
        public Text itemDescriptTxt;
        public GameObject selledMask;

        public virtual void Awake()
        {
            isEquipSlot = false;


            slotBgImg = transform.Find("ItemBackground_Img").GetComponent<Image>();
            circleBgImg = transform.Find("ItemTypeBg_Img").GetComponent<Image>();
            iconTypeImage = transform.Find("ItemTypeBg_Img/ItemType_Img").GetComponent<Image>();
            if (isStoreGoods)
            {
                gdNameTxt = transform.Find("ItemNmBg_Img/ItemName_Txt").GetComponent<Text>();
                gdDmgTxt = transform.Find("ItemAtrbTxtGroup/Dmg_Txt").GetComponent<Text>();
                gdDefTxt = transform.Find("ItemAtrbTxtGroup/Def_Txt").GetComponent<Text>();
                gdCriticTxt = transform.Find("ItemAtrbTxtGroup/Crtic_Txt").GetComponent<Text>();
                gdDodTxt = transform.Find("ItemAtrbTxtGroup/Dod_Txt").GetComponent<Text>();
                gdDescrpTxt = transform.Find("ItemAtrbTxtGroup/Descrp_Txt").GetComponent<Text>();
                gdDisButtonGo = transform.Find("PriceBtnGroup/prePrice").gameObject;
                gdPriceTxt = transform.Find("PriceBtnGroup/Purchase_Btn/ItemPrice_Txt").GetComponent<Text>();
                gdPurchaseBtn = transform.Find("PriceBtnGroup/Purchase_Btn").GetComponent<Button>();
                gdDisPreButtonGo = transform.Find("PriceBtnGroup/prePrice").gameObject;
                gdPrePriceTxt = transform.Find("PriceBtnGroup/prePrice/prePrice_Txt").GetComponent<Text>();
                gdDisLableGo = transform.Find("UpBg_Img/discountLabel").gameObject;
                gdPurchaseBtn.onClick.AddListener(OnGdPurchaseBtnClick);

                
            }

        }

        void OnEnable()
        {
            if (!isStoreGoods && !Knapsack.Instance.IsGameScenes&&!isForgeSlot&&!isResSlot)
            {
                InventoryBtnManager.Instance.InventoryButtonClickEvent += HideSlotItem;
            }

            if(isStoreGoods)
            {
                StorePanel.Instance.SellItemEvent += SelledItemCallBack;
            }
        }

        void OnDisable()
        {
            if (!isStoreGoods && !Knapsack.Instance.IsGameScenes&&!isForgeSlot)
            {
                InventoryBtnManager.Instance.InventoryButtonClickEvent -= HideSlotItem;
            }

            if (isStoreGoods)
            {
                StorePanel.Instance.SellItemEvent -= SelledItemCallBack;
            }
        }

        public void HideSlotItem(bool isShow, ItemType itemType)
        {
            Item currentItem;
            if (transform.GetComponentInChildren<ItemUI>() != null && !isEquipSlot)
            {
                currentItem = transform.GetComponentInChildren<ItemUI>().Item;
                gameObject.SetActive(currentItem.itemType == itemType);
            }
        }

        protected virtual void Start()
        {

            image = GetComponent<Image>();

            if (transform.GetComponentInChildren<ItemUI>() != null)
            {
                slotItemName = transform.GetComponentInChildren<ItemUI>().Item.itemName;
            }

            if (slotBg != null)
            {
                if (this is EquipSlot) return;

                foreach (SlotBg bg in slotBg)
                {
                    if (GetItemQuality() == bg.itemQuality)
                    {
                        slotBgImg.sprite = bg.slotBgSprite;
                        circleBgImg.sprite = bg.circleBgSprite;
                    }
                }
            }

        }

        ///<summary>
        /// 如果是商店中的物品，则需要为按钮注册对应事件
        ///</summary>
        public void OnGdPurchaseBtnClick()
        {
            Item currentItem = transform.GetComponentInChildren<ItemUI>().Item;
            StorePanel.Instance.BuyItem(currentItem);
        }

        /// <summary>
        /// 商品出售回调
        /// </summary>
        public void SelledItemCallBack()
        {
            bool selled = StorePanel.Instance.IsSelledItem(GetItemId());
            selledMask.SetActive(selled);
        }

        /// <summary>
        /// 把item放在自身下面
        /// 如果自身下面已经有item了，amount++
        /// 如果没有 根据itemPrefab去实例化一个item，放在下面
        /// </summary>
        /// <param name="item"></param>
        public void StoreItem(Item item, bool isPutOff)//在这里添加实时排序算法
        {
            if (transform.GetComponentInChildren<ItemUI>() == null)
            {
                GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
                itemGameObject.transform.SetParent(slotBgImg.transform);
                itemGameObject.transform.localScale = Vector3.one;
                itemGameObject.transform.localPosition = Vector3.zero;
                itemGameObject.transform.SetSiblingIndex(1);
                itemGameObject.GetComponent<ItemUI>().SetItem(item);
                if (isStoreGoods)
                {
                    GetStoreGoodsInfo(item);
                }
            }
            else
            {
                //transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
                transform.GetComponentInChildren<ItemUI>().AddAmount();
            }
            iconTypeImage.sprite = item.itemTypeSprite;

            if (!isEquipSlot && !isStoreGoods && !Knapsack.Instance.IsGameScenes)
            {
                gameObject.SetActive(InventoryBtnManager.Instance.currentBtnType == item.itemType);
            }

            if (!isEquipSlot && !isStoreGoods && !isForgeSlot & !isResSlot)
            {
                if (isPutOff)
                {
                    List<GameObject> goList = Knapsack.Instance.hideSlotItemGoList;
                    bool isInsert = false;
                    for (int i = 0; i <= goList.Count - 1; i++)
                    {
                        Item goItem = goList[i].GetComponentInChildren<ItemUI>().Item;
                        if (item.itemType == goItem.itemType && item.itemQuality == goItem.itemQuality)
                        {
                            isInsert = true;
                            goList.Insert(i + 1, gameObject);
                            break;
                        }
                    }
                    if (!isInsert)
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

                        if(Knapsack.Instance.hideSlotItemGoList.Count>=finalSameTypeItemIndex+1)
                        {
                            Knapsack.Instance.hideSlotItemGoList.Insert(finalSameTypeItemIndex + 1, gameObject);
                        }else
                        {
                            Knapsack.Instance.hideSlotItemGoList.Add(gameObject);
                        }                        
                    }
                }
                else
                {
                    Knapsack.Instance.hideSlotItemGoList.Add(gameObject);

                }
            }
        }

        private void GetStoreGoodsInfo(Item currentItem)
        {
            bool isDiscount = currentItem.discountPrice > 0 && StorePanel.Instance.IsDiscountID(currentItem.itemID);
            gdNameTxt.text = ShowItemName(currentItem);
            /*   gdDescrpTxt.text = "Its Attack-speed is: " + "<color=yellow>Attack Speed: +" + currentItem.AttackSpeed + ", </color>" + currentItem.itemDescription;
               gdDmgTxt.text = "<color=red>+" + currentItem.Attack + "</color>";
               gdCriticTxt.text = "+" + currentItem.Crit.ToString();
               gdDefTxt.text = "+" + currentItem.Defence.ToString();
               gdDodTxt.text = "<color=bule>+" + currentItem.Dodge + "</color>";*/

            itemDescriptTxt.text = ShowItemDescription(currentItem);
            gdDisLableGo.SetActive(isDiscount);
            gdDisButtonGo.SetActive(isDiscount);
            gdDisPreButtonGo.SetActive(isDiscount);
            if (isDiscount)
            {
                gdPriceTxt.text = currentItem.discountPrice.ToString();
                gdPrePriceTxt.text = currentItem.buyPrice.ToString();
            }
            else
            {
                gdPriceTxt.text = currentItem.buyPrice.ToString();
            }
        }

        public ItemQuality GetItemQuality()
        {
            if (transform.GetComponentInChildren<ItemUI>() == null) return ItemQuality.White;

            return transform.GetComponentInChildren<ItemUI>().Item.itemQuality;
        }

        public Item GetItem()
        {
            return transform.GetComponentInChildren<ItemUI>().Item;
        }

        /// <summary>
        /// 得到当前物品槽存储的物品类型
        /// </summary>
        /// <returns></returns>
        public ItemType GetItemType()
        {
            return transform.GetComponentInChildren<ItemUI>().Item.itemType;
        }
        /// <summary>
        /// 得到物品的id
        /// </summary>
        /// <returns></returns>
        public int GetItemId()
        {
            return transform.GetComponentInChildren<ItemUI>().GetComponent<ItemUI>().Item.itemID;
        }

        /// <summary>
        /// 得到物品槽中的ItemUI
        /// </summary>
        /// <returns></returns>
        public ItemUI GetItemUI()
        {
            if (transform.GetComponentInChildren<ItemUI>() == null) return null;
            return transform.GetComponentInChildren<ItemUI>();
        }

        public bool IsFilled()
        {
            ItemUI itemUI = transform.GetComponentInChildren<ItemUI>();
            return itemUI.Amount >= itemUI.Item.itemCapacity;//当前的数量大于等于容量
        }

        /// <summary>
        /// 鼠标进入的交互
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (transform.GetComponentInChildren<ItemUI>() == null || isStoreGoods) return;
            Item currentItem = transform.GetComponentInChildren<ItemUI>().Item;
            image.color = highlightColor;
            switch (Knapsack.Instance.toolTips.toolTipType)
            {
                case ToolTipType.ItemEffect:
                    Knapsack.Instance.toolTips.ShowCurrentSlotInfo(GetItemUI().GetItemEffectTxt(), "0");
                    break;
                case ToolTipType.None:
                    ShowTheSlotTooltips(currentItem);
                    //Knapsack.Instance.toolTips.Show(ShowItemName(currentItem), ShowItemDescription(currentItem));
                    break;
            }
        }

        private void ShowTheSlotTooltips(Item currentItem)
        {
            if (!Knapsack.Instance.IsGameScenes)
            {
                Item equipSlotItem = EquipPanel.Instance.EquipSlotItemInfo(currentItem.itemType);
                Knapsack.Instance.toolTips.ShowCurrentSlotInfo(ShowItemName(currentItem), ShowItemDescription(currentItem));
                if (equipSlotItem != null && !isEquipSlot&&!isForgeSlot)
                {
                    Knapsack.Instance.isShowTheEquipTips = true;
                    Knapsack.Instance.toolTips.ShowEquipSlotInfo(true, ShowItemName(equipSlotItem), ShowItemDescription(equipSlotItem));
                }
                else
                {
                    Knapsack.Instance.isShowTheEquipTips = false;
                    Knapsack.Instance.toolTips.ShowEquipSlotInfo(false, null, null);
                }
            }
            else
            {
                Knapsack.Instance.toolTips.ShowCurrentSlotInfo(ShowItemName(currentItem), ShowItemDescription(currentItem));
                if (!HTDBManager.Instance.NeedWeaponEuip())
                {
                    Item item = HTDBManager.Instance.ReturnCurrentWeapon();
                    Knapsack.Instance.toolTips.ShowEquipSlotInfo(true, ShowItemName(item), ShowItemDescription(item));
                }
                else
                {
                    Knapsack.Instance.toolTips.ShowEquipSlotInfo(false, null, null);
                }

            }

        }

        private string ShowItemName(Item currentItem)
        {
            string nameStr = "";
            string qualityDes = "";
            switch(currentItem.itemQuality)
            {
                case ItemQuality.White:
                    nameStr += "<color=white>";
                    qualityDes = "<size=15>(普通)</size>";
                    break;
                case ItemQuality.Blue:
                    nameStr += "<color=lightblue>";
                    qualityDes = "<size=15>(精良)</size>";
                    break;
                case ItemQuality.Green:
                    nameStr += "<color=lime>";
                    qualityDes = "<size=15>(稀有)</size>";
                    break;
                case ItemQuality.Purple:
                    nameStr += "<color=purple>";
                    qualityDes = "<size=15>(史诗)</size>";
                    break;
                case ItemQuality.Red:
                    nameStr += "<color=maroon>";
                    qualityDes = "<size=15>(传说)</size>";
                    break;
            }
            nameStr += currentItem.itemName;
            nameStr += " ";
            nameStr += qualityDes;
            nameStr+= "</color>";
            return nameStr;
        }

        private string ShowItemDescription(Item currentItem)
        {
            string description = "";
            description += "\n";
            description += "<color=grey><size=12> 属性</size></color>\n";
            description+= "<size=15><color=white>"+currentItem.itemDescription+"</color></size>\n";
            if (currentItem.hp>0)
            {
                description += "<size=15><color=white>血量 +" + currentItem.hp + "</color></size>\n";
            }
            if(currentItem.Defence>0)
            {
                description += "<size=15><color=white>防御 +" + currentItem.Defence+ "</color></size>\n";
            }
            if(currentItem.Attack>0)
            {
                description += "<size=15><color=white>攻击 +" + currentItem.Attack+ "</color></size>\n";
            }
            if(currentItem.MoveSpeed>0)
            {
                description += "<size=15><color=white>移动速度 +" + currentItem.MoveSpeed + "</color></size>\n";
            }
            if(currentItem.AttackSpeed>0)
            {
                description += "<size=15><color=white>攻击速度 +" + currentItem.AttackSpeed*100 + "%</color></size>\n";

            }
            if(currentItem.Dodge>0)
            {
                description += "<size=15><color=white>闪避 +" + currentItem.Dodge*100 + "%</color></size>\n";
            }
            if (currentItem.Crit > 0)
            {
                description += "<size=15><color=white>暴击率 +" + currentItem.Crit * 100 + "%</color></size>\n";
            }

            description += "<size=10><color=grey>套装效果</color></size>\n";
            description+="<size=15><color=orange>【天龙套】攻击+10%</color></size>\n";

            return description;
        }

        /// <summary>
        /// 使用（丢弃）物品槽的物品
        /// </summary>
        /// <param name="num"></param>
        public virtual void UseItem(int num = 1)
        {
            GetItemUI().ReduceAmount(num);
            if (GetItemUI().Amount <= 0)
            {
                Knapsack.Instance.slotList.Remove(this);
                HTDBManager.Instance.RemoveKanpsack(GetItemUI().Item.itemID);
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// 穿装备
        /// </summary>
        public void WearEquipment()
        {
            if (transform.GetComponentInChildren<ItemUI>() != null)
            {
                ItemUI currentItemUI = transform.GetComponentInChildren<ItemUI>();

                UseItem(1);
                Item currentItem = currentItemUI.Item;
                if (currentItemUI.Amount <= 0)
                {
                    DestroyImmediate(currentItemUI.gameObject);
                    Knapsack.Instance.toolTips.Hide();
                }
                Knapsack.Instance.hideSlotItemGoList.Remove(gameObject);
                EquipPanel.Instance.PutOn(currentItem, ChooseSlotBg(currentItem));
            }

        }

        public void PutOnItemIntoForgePanel()
        {
            if (GetItemUI() != null)
            {
                ItemUI currentItemUI = GetItemUI();

                UseItem(1);
                Item currentItem = currentItemUI.Item;
                if (currentItemUI.Amount <= 0)
                {
                    DestroyImmediate(currentItemUI.gameObject);
                    Knapsack.Instance.toolTips.Hide();
                }
                Knapsack.Instance.hideSlotItemGoList.Remove(gameObject);
                ForgePanel.Instance.PutOn(currentItem);
            }

        }

        public SlotBg ChooseSlotBg(Item currentItem)
        {
            SlotBg finalSlotBg = new SlotBg();
            if (slotBg != null)
            {
                foreach (SlotBg bg in slotBg)
                {
                    if (currentItem.itemQuality == bg.itemQuality)
                    {
                        finalSlotBg = bg;
                        break;
                    }
                }
            }
            return finalSlotBg;
        }

        /// <summary>
        /// 鼠标点击的交互
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (transform.GetComponentInChildren<ItemUI>() == null || isStoreGoods) return;
            image.color = clickColor;

            ItemUI itemUI = GetItemUI();

            InventoryManager.Instance.PickedSlot = this;

            switch (itemUI.Item.itemType)
            {
                case ItemType.Consumable:
                    UseItem();
                    break;
                default:

                    bool isWeapon = itemUI.Item.itemType == ItemType.Weapon ? true : false;
                    Knapsack.Instance.btnTips.ShowTips(true, isWeapon);
                    //WearEquipment();

                    break;
            }
        }

        /// <summary>
        /// 鼠标退出的交互
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (transform.GetComponentInChildren<ItemUI>() == null || isStoreGoods) return;
            image.color = normalColor;

            switch (Knapsack.Instance.toolTips.toolTipType)
            {
                case ToolTipType.ItemEffect:
                    Knapsack.Instance.toolTips.Hide();
                    break;
                case ToolTipType.None:
                    Knapsack.Instance.toolTips.Hide();
                    break;
            }
        }
    }

}
