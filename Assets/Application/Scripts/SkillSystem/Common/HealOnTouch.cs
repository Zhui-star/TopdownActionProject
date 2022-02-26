using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    /// <summary>
    /// 治疗触发区域
    /// </summary>
    public class HealOnTouch : MonoBehaviourSimplify
    {
        public LayerMask _layerMask;
        public int _defaultHealNumber = 20;
        public CharacterConfig _characterConfigure;
        [Range(0,100)]
        public int _attackToHealPercent = 50;

        public void OnTriggerEnter(Collider other)
        {
            Colliding(other);
        }

        protected virtual void Colliding(Collider other)
        {
            CharacterFunctionSwitch functionSwitch = other.GetComponent<CharacterFunctionSwitch>();
            if(functionSwitch==null)
            {
                return;
            }

            if(!MMLayers.LayerInLayerMask(other.gameObject.layer,_layerMask))
            {
                return;
            }

            int targetHealNumber = 0;

            if(_characterConfigure!=null)
            {
                targetHealNumber = (int)(_characterConfigure.additiveAttack + _characterConfigure.characterAttack) * (_attackToHealPercent/100);
            }
            else
            {
                targetHealNumber = _defaultHealNumber;
            }


            functionSwitch.Heal(targetHealNumber);

        }

        protected override void OnBeforeDestroy()
        {
            
        }
    }

}
