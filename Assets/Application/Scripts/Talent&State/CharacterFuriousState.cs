using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    public class CharacterFuriousState : CharacterState
    {
        public CharacterConfig characterConfig;
        private int index = 0; //记录处于哪一等级的变化点
        private int previousAddAttack = 0; //变化前增加的攻击力

        public CharacterFuriousState( ): base()
        {
            characterConfig = CharacterManager.Instance.
            GetCharacter("Player1").GetComponent<CharacterIdentity>().characterConfigure;
        }
        public override void OnEnter()
        {
            EventTypeManager.AddListener<int,int>(HTEventType.Furious, AddAttack);
        }

        public override void OnExit()
        {
            EventTypeManager.RemoveListener<int,int>(HTEventType.Furious, AddAttack);
            if (characterConfig != null)
            {
                characterConfig.additiveAttack -= GetRecoverAttack(index);
            }

        }

        /// <summary>
        /// 根据血量百分比增加攻击力
        /// </summary>
        /// <param name="currentHealth"></param>
        /// <param name="maximumHealth"></param>
        public void AddAttack(int currentHealth,int maximumHealth)
        {
           if(characterConfig!=null)
            {
                double healthRate = (double)currentHealth / maximumHealth;
                if (healthRate < 1 && healthRate >= 0.8&&index!=1)
                {
                    characterConfig.additiveAttack -= GetRecoverAttack(index);
                    characterConfig.additiveAttack += 5;
                    index = 1;
                }
                else if (healthRate < 0.8 && healthRate >= 0.6&&index!=2)
                {
                    characterConfig.additiveAttack -= GetRecoverAttack(index);
                    characterConfig.additiveAttack += 10;
                    index = 2;
                }
                else if (healthRate < 0.6 && healthRate >= 0.4&&index!=3)
                {
                    characterConfig.additiveAttack -= GetRecoverAttack(index);
                    characterConfig.additiveAttack += 15;
                    index = 3;
                }
                else if (healthRate < 0.4 && healthRate >= 0.2&&index!=4)
                {
                    characterConfig.additiveAttack -= GetRecoverAttack(index);
                    characterConfig.additiveAttack += 20;
                    index = 4;
                }
                else if(healthRate < 0.2 && healthRate > 0 && index!=5)
                {
                    characterConfig.additiveAttack -= GetRecoverAttack(index);
                    characterConfig.additiveAttack += 25;
                    index = 5;
                }
            }
        }

        /// <summary>
        /// 记录血量变化前增加的攻击力
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetRecoverAttack(int index)
        {
            return previousAddAttack=index*5;
        }


    }
}

