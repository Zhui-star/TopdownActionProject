using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

namespace HTLibrary.Application
{
    public class HTReduceCoolDown : MonoBehaviour
    {
        public float reduceStage;

        public int skillID;

        public SkillReleaseTrigger skillRelease;

        public void ReduceSkillCoolDown()
        {

            if (!skillRelease.skillSetting[skillID].CanReduceCoolDown) return;
            if(skillRelease.recoderCooldown.ContainsKey(skillID))
            {
                float nextfire = skillRelease.recoderCooldown.TryGet<int, float>(skillID);
                
                nextfire -= reduceStage;
                
                skillRelease.recoderCooldown[skillID] = nextfire;
                
            }
        }
    }

}
