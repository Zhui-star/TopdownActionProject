using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    public class CharacterAddCritState : CharacterState
    {
        public CharacterConfig characterConfig;
        public CharacterAddCritState(): base()
        {
            characterConfig = CharacterManager.Instance.
            GetCharacter("Player1").GetComponent<CharacterIdentity>().characterConfigure;
        }
        public override void OnEnter()
        {
            characterConfig.additiveCritRank+=0.05f*level;
        }

        public override void OnExit()
        {
            characterConfig.additiveCritRank-=0.05f*level;
        }

        
    }
}

