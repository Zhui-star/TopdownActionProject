using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    /// <summary>
    /// 技能快捷栏管理者
    /// </summary>
    public class SkillBoxManager:MonoSingleton<SkillBoxManager>
    {
        public SkillBox[] skillBoxs;
        public void ImplementSkillBoxs(bool add)
        {
            foreach(var temp in skillBoxs)
            {
                temp.ImplementSkillBox(add);
            }
        }
    }

}
