using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    public class CharacterAddMoveSpeedState : CharacterState
    {
        public CharacterConfig characterConfig;
        public CharacterAddMoveSpeedState() : base()
        {
            characterConfig = CharacterManager.
            Instance.GetCharacter("Player1").GetComponent<CharacterIdentity>().characterConfigure;
        }
        public override void OnEnter()
        {
            characterConfig.additiveMoveSpeed+=5*level;
        }

        public override void OnExit()
        {
            characterConfig.additiveMoveSpeed-=5*level;
        }

        
    }
}

