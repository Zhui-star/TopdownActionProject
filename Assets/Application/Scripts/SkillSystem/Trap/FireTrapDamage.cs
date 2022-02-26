using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 火焰陷阱角色翻滚刻意躲避其伤害
    /// </summary>
    public class FireTrapDamage : DamageOnTouch
    {
        protected override void OnCollideWithDamageable(Health health)
        {
            

            if(health.tag==Tags.Player)
            {
                CharacterMovement  characterMovement = health.gameObject.GetComponent<CharacterMovement>();
                if(characterMovement._movement.CurrentState==CharacterStates.MovementStates.Dashing)
                {
                    return;
                }
            }

            base.OnCollideWithDamageable(health);
            
        }
    }

}
