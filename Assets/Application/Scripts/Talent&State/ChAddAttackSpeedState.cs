using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    /// <summary>22
    /// 攻击速度增长状态
    /// </summary>
    public class ChAddAttackSpeedState : CharacterState
    {
        public CharacterConfig characterConfigure;

        public ChAddAttackSpeedState():base()
        {
            characterConfigure = CharacterManager.Instance.GetCharacter("Player1").
            GetComponent<CharacterIdentity>().characterConfigure;    
        }
        public override void OnEnter()
        {
            characterConfigure.characterAddtiveAttackSpeed+=0.1f*level;
        }

        public override void OnExit()
        {
            characterConfigure.characterAddtiveAttackSpeed-=0.1f*level;
        }
    }

}
