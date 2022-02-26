using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    public class CharacterAddDodgeState : CharacterState
    {
        public CharacterConfig characterConfig;
        public CharacterAddDodgeState() : base()
        {
            characterConfig = CharacterManager.Instance.
            GetCharacter("Player1").GetComponent<CharacterIdentity>().characterConfigure;
        }
        public override void OnEnter()
        {
            characterConfig.additiveDodge+=0.03f*level;
        }

        public override void OnExit()
        {
            characterConfig.additiveDodge-=0.03f*level;
        }
    }
}

