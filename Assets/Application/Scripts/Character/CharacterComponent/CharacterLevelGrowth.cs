using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using Doodah.Components.Progression;
using Doodah.Components;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    /// <summary>
    /// 角色成长能力
    /// </summary>
    public class CharacterLevelGrowth : MonoBehaviour
    {
        public LevelGrowthConfigure levelGrowthConfigure;

        public CharacterConfig characterConfigure;

        public Progression progression;

        private float attackInrement;

        private float critmutileIncrement;

        private float critRankIncrement;

        private float defenceIncrement;

        private float hpIncrement;

        private float dodgeIncrement;

        private float moveSpeedIncrement;

        public Health health;

        [Header("升级反馈")]
        public GameObject levelUpEffect;
        public AudioClip levelClip;
        private void Awake()
        {
            if (progression == null) return;
            progression.EventLevelUpdated += EventLevelUpdated;
        }

        private void OnDisable()
        {
            if (progression == null) return;
            progression.EventLevelUpdated -= EventLevelUpdated;
        }

        /// <summary>
        /// 成长增加
        /// </summary>
        /// <param name="previous"></param>
        /// <param name="current"></param>
        public void  EventLevelUpdated(int previous,int current)
        {
            LevelGrowthUnit unit = levelGrowthConfigure.ReturnGrowthData(progression.Level);
            if (unit == null) return;

            characterConfigure.additiveAttack += unit.Attack;
            characterConfigure.additiveCritMultiple += unit.CritMultiple;
            characterConfigure.additiveCritRank += unit.CritRank;
            characterConfigure.additiveDefence += unit.Defence;
            characterConfigure.additiveHP += unit.HP;
            characterConfigure.additiveDodge += unit.Dodge;
            characterConfigure.additiveMoveSpeed += unit.MoveSpeed;

            attackInrement += unit.Attack;
            critmutileIncrement += unit.CritMultiple;
            critRankIncrement += unit.CritRank;
            defenceIncrement += unit.Defence;
            hpIncrement += unit.HP;
            dodgeIncrement += unit.Dodge;
            moveSpeedIncrement += unit.MoveSpeed;

            PlayerPrefs.SetInt(Consts.GameLevel+SaveManager.Instance.LoadGameID, progression.Level);
            PlayerPrefs.Save();

            if (!progression.InitalFinished)
            {
               // health.UpdateHealthBar(false,false);
                return;
            }

            if(levelUpEffect!=null)
            {
                levelUpEffect.SetActive(true);
            }

            if(levelClip!=null)
            {
                SoundManager.Instance.PlaySound(levelClip, transform.position, false);
            }

            health.CurrentHealth += (int)unit.HP;//当前生命恢复
        }

        /// <summary>
        /// 重置成长
        /// </summary>
        private void OnDestroy()
        {
            characterConfigure.additiveAttack -= attackInrement;
            characterConfigure.additiveCritMultiple -= critmutileIncrement;
            characterConfigure.additiveCritRank -= critRankIncrement;
            characterConfigure.additiveDefence -= defenceIncrement;
            characterConfigure.additiveHP -= hpIncrement;
            characterConfigure.additiveDodge -= dodgeIncrement;
            characterConfigure.additiveMoveSpeed -= moveSpeedIncrement;
        }
    }

}
