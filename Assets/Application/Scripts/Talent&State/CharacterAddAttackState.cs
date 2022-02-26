using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Application;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 攻击力增幅状态
    /// </summary>
    public class CharacterAddAttackState : CharacterState
    {
        CharacterConfig configure;

        public CharacterAddAttackState():base()
        {
            configure = CharacterManager.Instance.GetCharacter("Player1")
            .GetComponent<CharacterIdentity>().characterConfigure;
        }
        public override void OnEnter()
        {
           configure.additiveAttack+=4*level;
        }

        public override void OnExit()
        {
            configure.additiveAttack-=4*level;
        }

        public override void Process()
        {
            base.Process();
        }
    }

}
