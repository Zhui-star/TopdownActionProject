using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 伙伴Item元素UI
    /// </summary>
    public class TabPatnerItemUI : TabShowItemUI
    {
        PatnerUnit _patnerUnit;

        Health _health;

        public override void UpdateUI()
        {
            base.UpdateUI();
            _itemName.text = _patnerUnit.Name;
        }

        public void SetPatner(PatnerUnit patnerUnit,Health health)
        {
            this._patnerUnit = patnerUnit;
            this._health = health;
        }


        protected override string GetItemInfo()
        {
            string info = "";
            info += "<size=20>";
            info += _patnerUnit.Name+"\n";
            info += "</size>";

            info += "<size=15>";
            info +=_patnerUnit.PatnerDescription+"\n";


            CharacterConfig config = _health.characterConfigure;
            info += "血量: " + _health.MaximumHealth;
            info += "攻击: " + (config.additiveAttack + config.characterAttack);
            info += "防御: " + _health.Defence;
            info += "</size>";

            return info;
        }
    }

}
