using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using HTLibrary.Framework;
using HTLibrary.Application;
using HTLibrary.Utility;
namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this component to a character and it'll be able to activate/desactivate the pause
    /// </summary>
    [MMHiddenProperties("AbilityStopFeedbacks")]
    [AddComponentMenu("TopDown Engine/Character/Abilities/Character Pause")]
    public class CharacterPause : CharacterAbility
    {
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "Allows this character (and the player controlling it) to press the pause button to pause the game."; }

        /// <summary>
        /// Every frame, we check the input to see if we need to pause/unpause the game
        /// </summary>
        protected override void HandleInput()
        {
            //When character transfering other profession
            if (UIManager.Instance.GetPeekPanelType() == UIPanelType.CharacterSelectPanel ||
             UIManager.Instance.GetPeekPanelType() == UIPanelType.HTSkillSelectPanel ||
                  UIManager.Instance.GetPeekPanelType() == UIPanelType.GSEquipPanel ||
                   UIManager.Instance.GetPeekPanelType() == UIPanelType.WeaponSwitchTipsPanel||
                   UIManager.Instance.GetPeekPanelType()==UIPanelType.GameOverPanel||
                   UIManager.Instance.GetPeekPanelType()==UIPanelType.VictoryPanel)
                return;

            if (_inputManager.PauseButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {

                //TriggerPause();
                if (GameManager.Instance.Paused)
                {
                    if (UIManager.Instance.GetPanelStackCount() > 1)
                    {
                        UIManager.Instance.PopPanel();
                    }

                }
                else
                {
                    UIManager.Instance.PushPanel(UIPanelType.GamePausePanel);
                }
            }

            //Tab 信息面板
            if (_inputManager.TabButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                if (UIManager.Instance.GetPeekPanelType() == UIPanelType.TabStatePanel)
                {
                    UIManager.Instance.PopPanel();
                }
                else
                {
                    UIManager.Instance.PushPanel(UIPanelType.TabStatePanel);
                }

            }
        }

        /// <summary>
        /// If the pause button has been pressed, we change the pause state
        /// </summary>
        public virtual void TriggerPause()
        {
            if (!AbilityPermitted
            && (_condition.CurrentState == CharacterStates.CharacterConditions.Normal || _condition.CurrentState == CharacterStates.CharacterConditions.Paused))
            {
                return;
            }
            PlayAbilityStartFeedbacks();
            // we trigger a Pause event for the GameManager and other classes that could be listening to it too
            TopDownEngineEvent.Trigger(TopDownEngineEventTypes.Pause, null);
        }

        /// <summary>
        /// Puts the character in the pause state
        /// </summary>
        public virtual void PauseCharacter()
        {
            _condition.ChangeState(CharacterStates.CharacterConditions.Paused);
        }

        /// <summary>
        /// Restores the character to the state it was in before the pause.
        /// </summary>
        public virtual void UnPauseCharacter()
        {
            _condition.RestorePreviousState();
        }
    }
}