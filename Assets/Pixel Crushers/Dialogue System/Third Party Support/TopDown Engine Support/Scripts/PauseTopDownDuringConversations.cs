using MoreMountains.TopDownEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrushers.DialogueSystem.TopDownEngineSupport
{

    /// <summary>
    /// Pauses TopDown and/or disables player input during conversations.
    /// If you add it to the Dialogue Manager, it will effect all conversations.
    /// If you add it to a player, it will only effect conversations that the
    /// player is involved in.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Dialogue System/Third Party/TopDown Engine/Pause TopDown During Conversations")]
    public class PauseTopDownDuringConversations : MonoBehaviour
    {
        [Tooltip("Tell Topdown Engine to pause during conversations.")]
        public bool pauseDuringConversations = true;

        [Tooltip("Disable TopDown player input during conversations.")]
        public bool disableInputDuringConversations = false;

        public string[] floatAnimatorParametersToStop = new string[] { "Speed" };
        public string[] boolAnimatorParametersToStop = new string[] { "Walking", "Running", "Jumping" };

        protected int pauseDepth = 0;
        protected bool prevSendNavEvents = false;

        protected virtual void OnConversationStart(Transform actor)
        {
            if (pauseDuringConversations) Pause();
            if (disableInputDuringConversations) SetTopDownInput(false);
            prevSendNavEvents = EventSystem.current.sendNavigationEvents;
            EventSystem.current.sendNavigationEvents = true;
        }

        private void OnConversationEnd(Transform actor)
        {
            if (pauseDuringConversations) Unpause();
            if (disableInputDuringConversations) SetTopDownInput(true);
            EventSystem.current.sendNavigationEvents = prevSendNavEvents;
        }

        public virtual void Pause()
        {
            // In case we get multiple requests to pause before unpause, only unpause after last call to Unpause:
            if (pauseDepth == 0)
            {
                TopDownEngineEvent.Trigger(TopDownEngineEventTypes.Pause, null);
                GUIManager.Instance.PauseScreen.SetActive(false);
            }
            pauseDepth++;
        }

        public virtual void Unpause()
        {
            pauseDepth--;
            if (pauseDepth == 0)
            {
                TopDownEngineEvent.Trigger(TopDownEngineEventTypes.Pause, null);
                GUIManager.Instance.PauseScreen.SetActive(false);
            }
        }

        protected virtual void SetTopDownInput(bool value)
        {
            SetAllInputManagers(value);
            SetAllPlayersComponents(value);
            if (value == false) FixAnimators();
        }

        protected virtual void SetAllInputManagers(bool value)
        {
            foreach (var inputManager in FindObjectsOfType<InputManager>())
            {
                SetInputManager(inputManager, value);
            }
        }

        protected virtual void SetInputManager(InputManager inputManager, bool value)
        {
            if (inputManager == null) return;
            inputManager.JumpButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.RunButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.DashButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.CrouchButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.ShootButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.InteractButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.SecondaryShootButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.ReloadButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.PauseButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.TimeControlButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.SwitchWeaponButton.State.ChangeState(MoreMountains.Tools.MMInput.ButtonStates.Off);
            inputManager.enabled = value;
        }

        protected virtual void SetAllPlayersComponents(bool value)
        {
            foreach (var player in MoreMountains.TopDownEngine.LevelManager.Instance.Players)
            {
                SetPlayerComponents(player, value);
            }
        }

        protected virtual void SetPlayerComponents(Character player, bool value)
        {            
            var character = player.GetComponent<Character>();
            if (character.MovementState.CurrentState == CharacterStates.MovementStates.Running)
            { 
                    var run = player.GetComponent<CharacterRun>();
                    if (run != null) run.RunStop();
            }
            foreach (var handleWeapon in player.GetComponents<CharacterHandleWeapon>())
            {
                handleWeapon.ShootStop();
            }
            character.ConditionState.ChangeState(value ? CharacterStates.CharacterConditions.Normal : CharacterStates.CharacterConditions.Frozen);
            character.MovementState.ChangeState(CharacterStates.MovementStates.Idle);
            var controller = player.GetComponent<TopDownController>();
            controller.Grounded = true;
            var jump2D = player.GetComponent<CharacterJump2D>();
            if (jump2D != null) jump2D.ResetAbility();
            var jump3D = player.GetComponent<CharacterJump3D>();
            if (jump3D != null) jump3D.ResetNumberOfJumps();
        }

        protected virtual void FixAnimators()
        {
            StartCoroutine(FixAnimatorsCoroutine());
        }

        protected IEnumerator FixAnimatorsCoroutine()
        {
            yield return null;
            foreach (var player in MoreMountains.TopDownEngine.LevelManager.Instance.Players)
            {
                var animator = player.GetComponent<Character>()._animator;
                foreach (var floatParameter in floatAnimatorParametersToStop)
                {
                    animator.SetFloat(floatParameter, 0);
                }
                foreach (var boolParameter in boolAnimatorParametersToStop)
                {
                    animator.SetBool(boolParameter, false);
                }
                foreach (var ps in player.GetComponent<CharacterMovement>().WalkParticles)
                {
                    ps.Stop();
                }
            }
        }
    }
}
