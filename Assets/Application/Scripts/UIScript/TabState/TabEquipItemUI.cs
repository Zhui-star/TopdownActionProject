using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using UnityEngine.UI;

namespace HTLibrary.Application
{
    /// <summary>
    /// Tab装备Item 单位UI
    /// </summary>
    public class TabEquipItemUI :TabShowItemUI
    {
        public Item currentItem;

        public Image _slotImg;
        public Image _iconTypeImg;
        public Image _iconTypeSlotImg;
        /// <summary>
        /// 设置装备Item
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(Item item)
        {
            currentItem = item;
        }

        public override void UpdateUI()
        {
            _itemIcon.sprite = currentItem.itemSprite;
            _slotImg.sprite = currentItem.slotSprite;
            _iconTypeImg.sprite = currentItem.itemTypeSprite;
            _iconTypeSlotImg.sprite = currentItem.itemTypeBgSprite;
        }

        protected override string GetItemInfo()
        {
            string info = "";
            info += ShowItemName();
            info += ShowItemDecription();
            return info;
        }

        /// <summary>
        /// 显示Item名字
        /// </summary>
        /// <returns></returns>
        string ShowItemName()
        {

            string nameStr = "";
            string qualityDes = "";
            switch (currentItem.itemQuality)
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
            nameStr += "</color>";
            return nameStr;
        }

        /// <summary>
        /// 显示Item描述
        /// </summary>
        /// <returns></returns>
        string ShowItemDecription()
        {

            string description = "";
            description += "\n";
            description += "<color=grey><size=12> 属性</size></color>\n";
            description += "<size=15><color=white>" + currentItem.itemDescription + "</color></size>\n";
            if (currentItem.hp > 0)
            {
                description += "<size=15><color=white>血量 +" + currentItem.hp + "</color></size>\n";
            }
            if (currentItem.Defence > 0)
            {
                description += "<size=15><color=white>防御 +" + currentItem.Defence + "</color></size>\n";
            }
            if (currentItem.Attack > 0)
            {
                description += "<size=15><color=white>攻击 +" + currentItem.Attack + "</color></size>\n";
            }
            if (currentItem.MoveSpeed > 0)
            {
                description += "<size=15><color=white>移动速度 +" + currentItem.MoveSpeed + "</color></size>\n";
            }
            if (currentItem.AttackSpeed > 0)
            {
                description += "<size=15><color=white>攻击速度 +" + currentItem.AttackSpeed * 100 + "%</color></size>\n";

            }
            if (currentItem.Dodge > 0)
            {
                description += "<size=15><color=white>闪避 +" + currentItem.Dodge * 100 + "%</color></size>\n";
            }
            if (currentItem.Crit > 0)
            {
                description += "<size=15><color=white>闪避 +" + currentItem.Crit * 100 + "%</color></size>\n";
            }

            description += "<size=10><color=grey>套装效果</color></size>\n";
            description += "<size=15><color=orange>【天龙套】攻击+10%</color></size>\n";

            return description;
        }
    }
    

}
