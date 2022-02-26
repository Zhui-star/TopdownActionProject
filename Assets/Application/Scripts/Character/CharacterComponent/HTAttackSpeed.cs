using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using System;
namespace HTLibrary.Application
{
    /// <summary>
    /// 攻速控制中心
    /// </summary>
    public class HTAttackSpeed : MonoBehaviour
    {
        public CharacterConfig playerConfigure;

        private float basicAttackSpeed;
        public float BasicAttackSpeed
        {
            get
            {
                if (playerConfigure != null)
                {
                    return playerConfigure.characterAttachSpeed;
                }
                return basicAttackSpeed;
            }
            set
            {
                basicAttackSpeed = value;
            }
        }

        public float TotalAttackSpeed()
        {
            return playerConfigure.characterAddtiveAttackSpeed + BasicAttackSpeed;
        }

        public float AnimSpeedPercent()
        {
            return BasicAttackSpeed / TotalAttackSpeed();
        }
    }

}
