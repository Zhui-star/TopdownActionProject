using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    /// <summary>
    /// 暴击率
    /// </summary>
    public class WeaponCrit : MonoBehaviourSimplify
    {
        public CharacterConfig characterConfigure;

        public bool mustCrit = false;

        protected override void OnBeforeDestroy()
        {

        }

        public float GetDamageCrit(float damage)
        {
            if (mustCrit)
            {
                return damage * (characterConfigure.characterCritMultiple + characterConfigure.additiveCritMultiple);
            }

            if (MathUtility.Percent((int)((characterConfigure.characterCritRank + characterConfigure.additiveCritRank) * 100)))
            {
                return damage * (characterConfigure.characterCritMultiple + characterConfigure.additiveCritMultiple);
            }
            return damage;
        }
    }

}
