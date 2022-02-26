using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using MoreMountains.InventoryEngine;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    public class CharacterSkill2 : CharacterAbility
    {
        SkillReleaseTrigger skillReleaseTrigger;
        CharacterBattleController characterBattleController;
        protected override void Initialization()
        {
            base.Initialization();
            skillReleaseTrigger = GetComponent<SkillReleaseTrigger>();
            characterBattleController = GetComponent<CharacterBattleController>();
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
        }

        /// <summary>
        /// Start use skill2
        /// </summary>
        protected override void HandleInput()
        {
            base.HandleInput();

            if (_inputManager.Skill2Button.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                DoSomething();
            }else if(_inputManager.Skill2Button.State.CurrentState==MMInput.ButtonStates.ButtonUp)
            {
                TurnOffSkillRelese();
            }

        }

        /// <summary>
        /// Skill 2 logic
        /// </summary>
        public virtual void DoSomething()
        {
            skillReleaseTrigger.TriggerSkill(1);

            if(characterBattleController!=null)
            {
                characterBattleController.StartBattleState();
            }
        }

        /// <summary>
        /// 停止技能释放
        /// </summary>
        public virtual void TurnOffSkillRelese()
        {
            if(characterBattleController!=null)
            {
                characterBattleController.StopBattleState();
            }
        }

        protected override void InitializeAnimatorParameters()
        {
            base.InitializeAnimatorParameters();
        }
    }

}
