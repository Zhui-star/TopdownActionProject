using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    public class ThroughDamageOnTouch :DamageOnTouch
    {
        [Header("穿越敌人后的伤害百分比")]
        public int damagePercent = 85;

        public override void OnTriggerEnter(Collider collider)
        {
            
            base.OnTriggerEnter(collider);
            DamageCaused *=damagePercent;
            DamageCaused /= 100;
        }

    }

}
