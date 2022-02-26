using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    public class GreatSwordLevel2PassiveSkill:PassiveSkill
    {
        [Range(0, 100)]
        public int chance = 20;

        private void OnEnable()
        {
            EventTypeManager.AddListener(HTEventType.GreatSwordLevelPassiveSkill, TriggerPassiveSkill);
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener(HTEventType.GreatSwordLevelPassiveSkill, TriggerPassiveSkill);
        }

        /// <summary>
        /// 触发被动技能
        /// </summary>
        void TriggerPassiveSkill()
        {
            if(MathUtility.Percent(chance))
            {
                if (_index == -1)
                    return;

                _skillReleseTrigger.TriggerSkill(_index);
            }
        }

        protected override void OnBeforeDestroy()
        {
        }
    }

}
