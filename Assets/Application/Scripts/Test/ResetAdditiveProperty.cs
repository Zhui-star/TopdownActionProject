using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Application;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Test
{
    public class ResetAdditiveProperty : MonoBehaviour
    {
        public CharacterConfig characterConfig;

        /// <summary>
        /// 调试，将角色属性进行重置
        /// </summary>
        public void ResetCharacterProerty()
        {
            characterConfig.ClearAdditiveProperty();
        }
    }

}

