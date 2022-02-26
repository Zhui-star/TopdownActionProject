using MoreMountains.TopDownEngine;
using HTLibrary.Utility;


namespace HTLibrary.Application
{
    public class CharacterAddHpState : CharacterState
    {
        public CharacterConfig characterConfig;
        public CharacterAddHpState() : base()
        {
            characterConfig = CharacterManager.Instance.GetCharacter("Player1").
            GetComponent<CharacterIdentity>().characterConfigure;
        }

        public override void OnEnter()
        {
            characterConfig.additiveHP+=20*level;
        }

        public override void OnExit()
        {
            characterConfig.additiveHP-=20*level;
        }
    }
}

