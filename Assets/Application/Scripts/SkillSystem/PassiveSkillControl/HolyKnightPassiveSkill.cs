using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 神圣剑士被动技能削弱地方防御
    /// </summary>
    public class HolyKnightPassiveSkill : MonoBehaviourSimplify
    {
        public float _defenceReducePercent = 0.1f;

        private void OnTriggerEnter(Collider other)
        {
           
            Health health = other.GetComponent<Health>();
            if(health!=null&&health.tag==Tags.Enemies)
            {
                health.ReduceDefenceByPercent(_defenceReducePercent);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Health health = other.GetComponent<Health>();
            if (health != null && health.tag == Tags.Enemies)
            {
                health.RestoreDefence();
            }
        }

        protected override void OnBeforeDestroy()
        {
            
        }
    }

}
