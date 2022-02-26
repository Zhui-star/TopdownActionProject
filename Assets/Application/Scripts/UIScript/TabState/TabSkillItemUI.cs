using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    /// <summary>
    /// 技能信息ItemUI
    /// </summary>
    public class TabSkillItemUI : TabShowItemUI
    {
        SkillUnit _skillUnit;

        public void SetSkillUnit(SkillUnit skillUnit)
        {
            _skillUnit = skillUnit;
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            _itemIcon.sprite = _skillUnit.skillIcon;
        }

        protected override string GetItemInfo()
        {
            string info = "";
            info += "<size=20>";
            info += _skillUnit.skillName+"</size> \n";
            info += "<size=15>";
            info += _skillUnit.skillDescription+"</size>";

            return info;
        }

        public override string ToString()
        {
            return base.ToString();
        }

       
    }

}
