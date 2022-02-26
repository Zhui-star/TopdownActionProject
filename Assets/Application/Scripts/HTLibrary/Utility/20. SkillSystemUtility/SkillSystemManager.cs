using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 技能系统管理
    /// </summary>
    public class SkillSystemManager : MonoSingleton<SkillSystemManager>
    {
        private SkillUnit pickedUnit;

        public SkillUnit PickedUnit
        {
            get
            {
                return pickedUnit;
            }
            set
            {
                pickedUnit = value;
            }
        }

        public SkillUnitList skillUnitList;

        /// <summary>
        /// 返回技能ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillUnit GetSkillById(int id)
        {
            if(skillUnitList==null)
            {
                skillUnitList = Resources.Load<SkillUnitList>("Configure/SkillUnitList");
            }

            foreach(var temp in skillUnitList.skillList)
            {
                if(id==temp.skillID)
                {
                    return temp;
                }
            }
            return null;
        }
    }

}
