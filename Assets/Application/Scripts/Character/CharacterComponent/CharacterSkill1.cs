using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using MoreMountains.InventoryEngine;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    public class CharacterSkill1 : CharacterAbility
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
        /// Start use skill 1
        /// </summary>
        protected override void HandleInput()
        {
            base.HandleInput();

            if (_inputManager.Skill1Button.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                DoSomething();
            }else if(_inputManager.Skill1Button.State.CurrentState == MMInput.ButtonStates.ButtonUp)
            {
                TurnOffSkillRelese();
            }
           
        }

        /// <summary>
        /// Skill 1 logic
        /// </summary>
        public virtual void DoSomething()
        {
            skillReleaseTrigger.TriggerSkill(0);

            if(characterBattleController!=null)
            {
                characterBattleController.StartBattleState();
            }

        }

        /// <summary>
        /// 关闭技能
        /// </summary>
        public virtual void TurnOffSkillRelese()
        {
            if (characterBattleController != null)
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
