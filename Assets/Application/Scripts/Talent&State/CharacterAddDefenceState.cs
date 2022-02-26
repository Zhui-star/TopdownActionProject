using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    public class CharacterAddDefenceState : CharacterState
    {
        public CharacterConfig characterConfig;
        public CharacterAddDefenceState() : base()
        {
            characterConfig = CharacterManager.Instance.
            GetCharacter("Player1").GetComponent<CharacterIdentity>().characterConfigure;
        }
        public override void OnEnter()
        {
            characterConfig.additiveDefence+=2*level;
        }

        public override void OnExit()
        {
            characterConfig.additiveDefence-=2*level;
        }
    }

}
