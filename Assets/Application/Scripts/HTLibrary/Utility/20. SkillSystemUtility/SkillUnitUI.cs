using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace HTLibrary.Utility
{
    public class SkillUnitUI : MonoBehaviour
    {
        public Image iconImage;

        public Text skillInfoTxt;

        public SkillUnit skillUnit;

        public bool useConfigure = false;
        public void SetSkillUnit(SkillUnit skillUnit)
        {
            transform.localScale = Vector3.one;
            this.skillUnit = skillUnit;

            GetSkillInfo();
        }



        void GetSkillInfo()
        {
            skillInfoTxt.text = string.Format("技能名称:{0}\n {1}", skillUnit.skillName, skillUnit.skillDescription);
        }
    }

}
