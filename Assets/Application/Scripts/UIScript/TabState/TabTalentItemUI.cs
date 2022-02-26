using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    /// <summary>
    /// 天赋Tab元素UI
    /// </summary>
    public class TabTalentItemUI : TabShowItemUI
    {
        TalentItem _talentItem;

        public override void UpdateUI()
        {
            base.UpdateUI();
            _itemIcon.sprite = _talentItem.TalentIcon;
        }
        public void SetItem(TalentItem talentItem)
        {
            _talentItem = talentItem;
        }

        protected override string GetItemInfo()
        {
            string info = "";
            info += "<size=20>";
            info += _talentItem.Name + "</size> \n";
            info += "<size=15>";
            info += _talentItem.Descritions[0] + "</size>";

            return info;
        }
    }

}
