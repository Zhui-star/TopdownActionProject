using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using HTLibrary.Framework;
using HTLibrary.Utility;
using UnityEngine.UI;
using DG.Tweening;
namespace HTLibrary.Application
{
    public class MenueWeaponSlot : MonoBehaviour
    {
        public Item item { get; set; }

        public ItemUI itemUI;

        public Image slotBg;

        public Image equipTypeBg;

        public Image equipTypeImg;

        private void Start()
        {
            if(HTDBManager.Instance.InitialOver)
            {
                transform.DOShakeScale(0.5f, 2).SetUpdate(true);
            }

        }

        public virtual void UpdateUI()
        {
            itemUI.SetItem(item);

            slotBg.sprite = item.slotSprite;
            equipTypeBg.sprite = item.itemTypeBgSprite;
            equipTypeImg.sprite = item.itemTypeSprite;
        }
        
        
    }

}
