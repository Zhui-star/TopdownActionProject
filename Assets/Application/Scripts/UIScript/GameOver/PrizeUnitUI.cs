using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using UnityEngine.UI;

namespace HTLibrary.Application
{
    /// <summary>
    /// 奖励结算物品
    /// </summary>
    public class PrizeUnitUI :MenueWeaponSlot
    {
        public GameObject _equipUIGo;
        [System.Serializable]
      public  struct SpritialSourceUI
        {
            public GameObject spritialUIGo;
            public Text count;
        }

       public SpritialSourceUI spritialUI;

        /// <summary>
        /// 更新装备奖励显示
        /// </summary>
        /// <param name="item"></param>
        public void UpdateEquipUI(Item item)
        {   
            
            base.item = item;
            base.UpdateUI();

            spritialUI.spritialUIGo.SetActive(false);
            _equipUIGo.SetActive(true);
        }


        /// <summary>
        /// 更新灵源奖励显示
        /// </summary>
        /// <param name="count"></param>
        public void UpdateSpritialSourceUI(uint count)
        {
            _equipUIGo.SetActive(false);
            spritialUI.spritialUIGo.SetActive(true);
            spritialUI.count.text ="X "+ count.ToString();
        }
    }

}
