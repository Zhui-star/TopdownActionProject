﻿using UnityEngine;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this component to a character and it'll be able to activate button zones
    /// Animator parameters : Activating (bool)
    /// </summary>
    [MMHiddenProperties("AbilityStopFeedbacks")]
    [AddComponentMenu("TopDown Engine/Character/Abilities/Character Button Activation")] 
	public class CharacterButtonActivation : CharacterAbility 
	{
		/// This method is only used to display a helpbox text at the beginning of the ability's inspector
		public override string HelpBoxText() { return "This component allows your character to interact with button powered objects (dialogue zones, switches...). "; }
		/// the current button activated zone

		protected bool _activating = false;

        protected const string _activatingAnimationParameterName = "Activating";
        protected int _activatingAnimationParameter;

		private HashSet<ButtonActivated> _buttonActivateds = new HashSet<ButtonActivated>();

		public HashSet<ButtonActivated> GetActivateds()
        {
			return _buttonActivateds;
        }

		public void AddButtonAcitvateds(ButtonActivated buttonActivated)
        {
			_buttonActivateds.Add(buttonActivated);
        }

		public void RemoveActivated(ButtonActivated buttonActivated)
        {
			_buttonActivateds.Remove(buttonActivated);
        }

		/// <summary>
		/// Gets and stores components for further use
		/// </summary>
		protected override void Initialization()
		{
			base.Initialization();
		}

		/// <summary>
		/// Every frame, we check the input to see if we need to pause/unpause the game
		/// </summary>
		protected override void HandleInput()
		{
            if (_inputManager.InteractButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
				ButtonActivation();
				//ButtonActivatedZone.TriggerButtonAction();
			}
		}
        
		/// <summary>
		/// Tries to activate the button activated zone
		/// </summary>
		protected virtual void ButtonActivation()
		{
			// if the player is in a button activated zone, we handle it
			if (_buttonActivateds.Count>0
				&& (_condition.CurrentState == CharacterStates.CharacterConditions.Normal || _condition.CurrentState == CharacterStates.CharacterConditions.Frozen)
			    && (_movement.CurrentState != CharacterStates.MovementStates.Dashing)&&(!GameManager.Instance.Paused))
			{
				// we trigger a character event
				 MMCharacterEvent.Trigger(_character, MMCharacterEventTypes.ButtonActivation);
				foreach(var activatedZone in _buttonActivateds)
				{
					activatedZone.TriggerButtonAction();
				}

                PlayAbilityStartFeedbacks();
                _activating = true;
			}
		}

		/// <summary>
		/// Adds required animator parameters to the animator parameters list if they exist
		/// </summary>
		protected override void InitializeAnimatorParameters()
		{
			RegisterAnimatorParameter (_activatingAnimationParameterName, AnimatorControllerParameterType.Bool, out _activatingAnimationParameter);
		}

		/// <summary>
		/// At the end of the ability's cycle, we send our current crouching and crawling states to the animator
		/// </summary>
		public override void UpdateAnimator()
		{
            MMAnimatorExtensions.UpdateAnimatorBool(_animator, _activatingAnimationParameter, _activating, _character._animatorParameters);
            _activating = false;
        }
	}
}
