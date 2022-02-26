using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    /// <summary>
    /// 攻速增长模式
    /// </summary>
    public enum AdditiveSpeedMode
    {
        Number,//指定攻速增长
        Double//攻速增长2倍
    }
    public class AttackSpeedBuff : MonoBehaviour
    {
        [HideInInspector]
        public GameObject owner;

        public float additiveAttackSpeed;
        [Header("攻速成长百分比, Number时才有效")]
        public float _addPercent = 0;
        public AdditiveSpeedMode additiveAttackSpeedMode;

        CharacterConfig characterConfigure;

        /// <summary>
        /// 攻速增长移除
        /// </summary>
        private void OnDisable()
        {
             RemoveAttackSpeed();
        }

      
        /// <summary>
        /// 增加攻速
        /// </summary>
        void AddAttackSpeed()
        {
            CharacterMovement move=  owner.GetComponent<CharacterMovement>();
            if(move !=null)
            {
                characterConfigure = move.characterConfigure;
               
                switch(additiveAttackSpeedMode)
                {
                    case AdditiveSpeedMode.Number:
                        additiveAttackSpeed = (characterConfigure.characterAddtiveAttackSpeed + characterConfigure.characterAddtiveAttackSpeed)*_addPercent;
                        break;
                    case AdditiveSpeedMode.Double:
                        additiveAttackSpeed = characterConfigure.characterAddtiveAttackSpeed;
                        break;
                }

                characterConfigure.characterAddtiveAttackSpeed += additiveAttackSpeed;

            }
        }

        /// <summary>
        /// 移除增加攻速
        /// </summary>
        void RemoveAttackSpeed()
        {
          if(characterConfigure!=null)
            {
                characterConfigure.characterAddtiveAttackSpeed -= additiveAttackSpeed;
            }
        }

        /// <summary>
        /// 该Buff的使用者
        /// </summary>
        /// <param name="owner"></param>
        public void SetOwner(GameObject owner)
        {
            this.owner = owner;

            AddAttackSpeed();
        }
    }

}
