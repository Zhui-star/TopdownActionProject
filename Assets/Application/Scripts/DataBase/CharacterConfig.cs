using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
namespace HTLibrary.Application
{
    [Serializable]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Character Information")]
        public string characterName = "";//角色昵称

        [Space]
        [Header("Character HP")]
        public float characterHP; //生命值
       
        public float additiveHP;

        [Space]
        [Header("Character Attack")]
        public float characterAttack; //攻击力
       
        public float additiveAttack;

        [Space]
        [Header("Character Denfence")]
        public float characterDefence; //防御值
       
        public float additiveDefence;
        [Space]
        [Header("Character Crit")]
        public float characterCritRank; //暴击率
        
        public float additiveCritRank;
        public float characterCritMultiple; //暴击倍数
        
        public float additiveCritMultiple;
        [Space]
        [Header("Character Dodge")]
        public float characterDodge; //闪避
        
        public float additiveDodge;
        [Space]
        [Header("Character Move")]
        public float characterMoveSpeed; //移动速度

        public float additiveMoveSpeed;
        [Space]
        [Header("Character Attack Speed Relevant")]
        public float characterAttachSpeed; //攻击速度
        public float characterAddtiveAttackSpeed;//角色额外攻速获得
        [Space]
        [Header("Abnormal resist")]
        [Range(0,100)]
        public uint abNormalResistChance;
        public uint additiveNormalResistChance;

        public delegate void UpdatePropertyDele(float hp, float attack, float defence, float crit, float dodge);

        public event UpdatePropertyDele UpdatePropertyEvent;

        public void ChangeProperty(float hp, float characterAttack, float characterDefence, float characterCritRank,
            float characterDodge, float characterMoveSpeed,float characterAddtiveAttackSpeed)
        {

            UpdatePropertyEvent?.Invoke(this.characterHP+this.additiveHP,this.characterAttack+ this.additiveAttack,this.characterDefence+
                this.additiveDefence,this.characterCritRank+ this.additiveCritRank,this.characterDodge+ this.additiveDodge);
        }

        public void ClearAdditiveProperty()
        {
            additiveHP = 0;
            additiveAttack = 0;
            additiveCritMultiple = 0;
            additiveCritRank = 0;
            additiveDefence = 0;
            additiveMoveSpeed = 0;
            additiveDodge = 0;
        }



    }

}
