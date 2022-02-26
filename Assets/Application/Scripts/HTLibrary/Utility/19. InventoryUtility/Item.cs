using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 道具数据
    /// </summary>
    [Serializable]
    public class Item 
    {
        public string itemName;

        public int itemID;

        public ItemType itemType;

        public Sprite itemTypeSprite;

        public Sprite slotSprite;

        public Sprite itemTypeBgSprite;

        public ItemQuality itemQuality;

        public string itemDescription;

        public int itemCapacity;

        public bool isHaveDiscount;

        public int discountPrice;

        public int buyPrice;

        public int sellPrice;

        public int hp;

        public Sprite itemSprite;

        public float Crit;

        public float Dodge;

        public float Attack;

        public float Defence;

        public float MoveSpeed;

        public float AttackSpeed;

        public List<WeaponType> weaponTypeList;

    }

}
