using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;


namespace HTLibrary.Application
{
    public class CharacterAddExpState : CharacterState
    {
        public CharacterConfig characterConfig;
        public bool isCanAddExp = false;
        public static CharacterAddExpState _instance;
        public CharacterAddExpState() : base()
        {
            characterConfig = CharacterManager.Instance.GetCharacter("Player1")
            .GetComponent<CharacterIdentity>().characterConfigure;
            _instance = this;
        }
        
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }


        public float AddExp( float experience)
        {
            experience*=1+level*0.1f;
            return experience;
        }
    }
}

