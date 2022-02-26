using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 物品UI
    /// </summary>
    public class ItemUI : MonoBehaviour
    {
        #region Data
        public Item Item { get; private set; } //物品数据
        public int Amount { get; private set; } //物品数量
        #endregion

        #region UI Component
        public Image itemImage; //物品图片
        public Text amountText; //数量Txt
        #endregion

        public bool IsUseConfigure = false;

        public Item SetItem(Item item, int amount = 1)
        {
            transform.localScale = Vector3.one;
            this.Item = item;
            this.Amount = amount;

            // update ui 
            if(IsUseConfigure)
            {
                itemImage.sprite = item.itemSprite;
            }

            if (Item.itemCapacity> 1)
                amountText.text = Amount.ToString();
            else
                amountText.text = "";
            return item;
        }

        public void AddAmount(int amount = 1)
        {
            transform.localScale = Vector3.one;
            this.Amount += amount;
            //update ui 
            if (Item.itemCapacity> 1)
                amountText.text = Amount.ToString();
            else
                amountText.text = "";
        }

        public void ReduceAmount(int amount = 1)
        {
            transform.localScale = Vector3.one;
            this.Amount -= amount;
            //update ui 
            if (Item.itemCapacity > 1)
                amountText.text = Amount.ToString();
            else
                amountText.text = "";
        }

        public void SetAmount(int amount)
        {
            transform.localScale = Vector3.one;
            this.Amount = amount;
            //update ui 
            if (Item.itemCapacity> 1)
                amountText.text = Amount.ToString();
            else
                amountText.text = "";
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public string GetItemEffectTxt()
        {
            string text = "";
          

            return text;
        }

    }

}
