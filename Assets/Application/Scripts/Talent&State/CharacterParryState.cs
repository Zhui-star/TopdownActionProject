using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    public class CharacterParryState : CharacterState
    {
        private CharacterConfig characterConfigure;
        private CharacterMovement characaterMovement;
        private float AddedDefence; //要增加的防御值
        private bool isCancel = true; //判断是否已增加防御值
        private Health _health;
        public CharacterParryState() : base()
        {
            characterConfigure = CharacterManager.Instance
            .GetCharacter("Player1").GetComponent<CharacterIdentity>().characterConfigure;
            characaterMovement = CharacterManager.Instance
            .GetCharacter("Player1").GetComponent<CharacterMovement>();
            _health = CharacterManager.Instance
            .GetCharacter("Player1").GetComponent<Health>();
        }

        public override void OnEnter()
        {
            EventTypeManager.AddListener(HTEventType.AddIdleDefence, AddIdleDefence);
        }

        public override void OnExit()
        {
            EventTypeManager.RemoveListener(HTEventType.AddIdleDefence, AddIdleDefence);

            characterConfigure.additiveDefence -= AddedDefence;
        }


        /// <summary>
        /// 在静止时增加5%的防御值
        /// </summary>
        public void AddIdleDefence()
        {
            switch(characaterMovement._movement.CurrentState)
            {
                case CharacterStates.MovementStates.Walking:
                     if (isCancel == true)
                    {
                        characterConfigure.additiveDefence -= AddedDefence;
                        AddedDefence = 0;
                        isCancel = false;
                    }
                    break;
                default:
                    if (isCancel == false)
                    {
                        AddedDefence = _health.Defence* 0.15f;
                        characterConfigure.additiveDefence += AddedDefence;
                        isCancel = true;
                    }
                    break;
            }
        }
    }
}

