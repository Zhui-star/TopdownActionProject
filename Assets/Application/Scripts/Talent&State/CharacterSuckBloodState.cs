using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 角色吸血状态
    /// </summary>
    public class CharacterSuckBloodState : CharacterState
    {
        public Health characterHealth;

        public CharacterSuckBloodState():base()
        {
           Initial();
        }

        public override void OnEnter()
        {
            EventTypeManager.AddListener<int>(HTEventType.SuckBlood, SuckBlood);
        }

        public override void OnExit()
        {
            EventTypeManager.RemoveListener<int>(HTEventType.SuckBlood, SuckBlood);
        }

        void Initial() 
        {
            characterHealth = CharacterManager.Instance.
            GetCharacter("Player1").GetComponent<Health>();
        }

        void SuckBlood(int damage)
        {
            float getHealth = 0;
            getHealth=damage*0.05f*level;
        
            if (getHealth <= 1)
                getHealth = 1;

            if(MathUtility.Percent(30))
            {
                if(characterHealth!=null)
                {
                    characterHealth.CurrentHealth += (int)getHealth;
                }               
            }

        }
    }

}
