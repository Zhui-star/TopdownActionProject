using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using HTLibrary.Application;
using HTLibrary.Utility;
using HTLibrary.Framework;
namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this class to a character so it can use weapons
    /// Note that this component will trigger animations (if their parameter is present in the Animator), based on 
    /// the current weapon's Animations
    /// Animator parameters : defined from the Weapon's inspector
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/Abilities/Character Handle Weapon")]
    public class CharacterHandleWeapon : CharacterAbility
    {
        [Header("")]
        [Header("Accumlate")]
        private bool _accumlate = false;
        private float _accumlateEndAt;
        public event System.Action<float> WeaponAccumlateEvent;
        public float _accumlateTime;
        public HTAccumlateBar _accumlateBar;
        public Transform _accumlateTrs;
        bool _accumlateBroadCast = false;

        bool _generalAttack = true;
        public MMFeedbacks _accumlateFeedBacks;
        /// This method is only used to display a helpbox text at the beginning of the ability's inspector
        public override string HelpBoxText() { return "This component will allow your character to pickup and use weapons." +
                " What the weapon will do is defined in the Weapon classes. " +
                "This just describes the behaviour of the 'hand' holding the weapon, not the weapon itself. " +
                "Here you can set an initial weapon for your character to start with, allow weapon pickup, " +
                "and specify a weapon attachment (a transform inside of your character, could be just an empty child gameobject," +
                " or a subpart of your model."; }

        [Header("Weapon")]
        /// the initial weapon owned by the character
        public Weapon InitialWeapon;
        private SprintWeapon _sprintWeapon;
        /// if this is set to true, the character can pick up PickableWeapons
        public bool CanPickupWeapons = true;

        [Header("Feedbacks")]
        /// a feedback that gets triggered at the character level everytime the weapon is used
        public MMFeedbacks WeaponUseFeedback;

        [Header("Binding")]
        /// the position the weapon will be attached to. If left blank, will be this.transform.
        public Transform WeaponAttachment;
        /// the position from which projectiles will be spawned (can be safely left empty)
        public Transform ProjectileSpawn;
        /// if this is true this animator will be automatically bound to the weapon
        public bool AutomaticallyBindAnimator = true;
        /// if this is true, IK will be automatically setup if possible
        public bool AutoIK = true;

        [Header("Input")]
        /// if this is true you won't have to release your fire button to auto reload
        public bool ContinuousPress = false;
        /// whether or not this character getting hit should interrupt its attack (will only work if the weapon is marked as interruptable)
        public bool GettingHitInterruptsAttack = false;

        [Header("Buffering")]
        /// whether or not attack input should be buffered, letting you prepare an attack while another is being performed, making it easier to chain them
        public bool BufferInput;
        [MMCondition("BufferInput", true)]
        /// if this is true, every new input will prolong the buffer
        public bool NewInputExtendsBuffer;
        [MMCondition("BufferInput", true)]
        /// the maximum duration for the buffer, in seconds
        public float MaximumBufferDuration = 0.25f;
        /// if this is true, and if this character is using GridMovement, then input will only be triggered when on a perfect tile
        [MMCondition("BufferInput", true)]
        public bool RequiresPerfectTile = false;

        /// returns the currently equipped weapon
        [Header("Debug")]
        [MMReadOnly]
        public Weapon CurrentWeapon;


        /// an animator to update when the weapon is used
        public Animator CharacterAnimator { get; set; }
        /// the weapon's weapon aim component, if it has one
        public WeaponAim WeaponAimComponent { get { return _weaponAim; } }

        protected float _fireTimer = 0f;
        protected float _secondaryHorizontalMovement;
        protected float _secondaryVerticalMovement;
        protected WeaponAim _weaponAim;
        protected ProjectileWeapon _projectileWeapon;
        protected WeaponIK _weaponIK;
        protected Transform _leftHandTarget = null;
        protected Transform _rightHandTarget = null;
        protected float _bufferEndsAt = 0f;
        protected bool _buffering = false;

        protected const string _weaponEquippedAnimationParameterName = "WeaponEquipped";
        protected const string _weaponEquippedIDAnimationParameterName = "WeaponEquippedID";
        protected int _weaponEquippedAnimationParameter;
        protected int _weaponEquippedIDAnimationParameter;
        protected CharacterGridMovement _characterGridMovement;

        protected CharacterDash3D dash;
        protected CharacterBattleController characterBattleController;

        private SkillReleaseTrigger _skillReleseTrigger;

        /// <summary>
        /// Sets the weapon attachment
        /// </summary>
        protected override void PreInitialization()
        {
            base.PreInitialization();
            // filler if the WeaponAttachment has not been set
            if (WeaponAttachment == null)
            {
                WeaponAttachment = transform;
            }
        }

        // Initialization
        protected override void Initialization()
        {
            base.Initialization();

            if (dash == null)
            {
                dash = GetComponent<CharacterDash3D>();
            }

            characterBattleController = GetComponent<CharacterBattleController>();
            _skillReleseTrigger = GetComponent<SkillReleaseTrigger>();

            Setup();

            InitialAccumlateBar();

            _accumlateFeedBacks?.Initialization();

            InitialSprintWeapon();
        }

        /// <summary>
        ///Initial Sprint weapon which is when player sprint press attack input key
        /// </summary>
        void InitialSprintWeapon()
        {

            _sprintWeapon = CurrentWeapon.GetComponent<SprintWeapon>();

            if (_sprintWeapon != null)
            {
                _sprintWeapon.Initialization();
                _sprintWeapon.SetOwner(this);
            }
        }

        /// <summary>
        /// Grabs various components and inits stuff
        /// </summary>
        public virtual void Setup()
        {
            _character = this.gameObject.MMGetComponentNoAlloc<Character>();
            _characterGridMovement = this.gameObject.GetComponent<CharacterGridMovement>();
            CharacterAnimator = _animator;
            // filler if the WeaponAttachment has not been set
            if (WeaponAttachment == null)
            {
                WeaponAttachment = transform;
            }
            if ((_animator != null) && (AutoIK))
            {
                _weaponIK = _animator.GetComponent<WeaponIK>();
            }
            // we set the initial weapon
            if (InitialWeapon != null)
            {
                ChangeWeapon(InitialWeapon, null);
            }
        }

        /// <summary>
        /// Every frame we check if it's needed to update the ammo display
        /// </summary>
        public override void ProcessAbility()
        {
            base.ProcessAbility();
            HandleFeedbacks();
            UpdateAmmoDisplay();
            HandleBuffer();
            ProcessAccumlate();
        }

        /// <summary>
        /// Triggers the weapon used feedback if needed
        /// </summary>
        protected virtual void HandleFeedbacks()
        {
            if (CurrentWeapon != null)
            {
                if (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse)
                {
                    WeaponUseFeedback?.PlayFeedbacks();
                }
            }
        }

        /// <summary>
        /// Gets input and triggers methods based on what's been pressed
        /// </summary>
        protected override void HandleInput()
        {
            if (GameManager.Instance.PlayingTimeline||GameManager.Instance.Paused) return;

            if (!AbilityPermitted
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal))
            {
                return;
            }
            if ((_inputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonDown) || (_inputManager.ShootAxis == MMInput.ButtonStates.ButtonDown))
            {
                if (CurrentWeapon.TriggerMode != Weapon.TriggerModes.Accumlate)
                {
                    ShootStart();
                }
            }

            if (CurrentWeapon != null)
            {
                if (ContinuousPress && (CurrentWeapon.TriggerMode == Weapon.TriggerModes.Accumlate) &&
                 (_inputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed))
                {
                    if (!_accumlate && !CurrentWeapon.WeaponState.CurrentState.Equals(Weapon.WeaponStates.WeaponAccumlate))
                    {
                        StartAccumlate();
                    }
                }
                else if (ContinuousPress && (CurrentWeapon.TriggerMode == Weapon.TriggerModes.Auto) && 
                (_inputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed))
                {
                    ShootStart();
                }
            }

            if ((_inputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonUp) || 
            (_inputManager.ShootAxis == MMInput.ButtonStates.ButtonUp))
            {
                if (CurrentWeapon.AccumlateWeapon)
                {
                    ShootStart();
                }
                else if ((CurrentWeapon.TriggerMode == Weapon.TriggerModes.Accumlate && _generalAttack))
                {
                    ShootStart();
                    _accumlate = false;
                }
                else
                {
                    _accumlate = false;
                    ShootStop();
                }

                if (EventTypeManager.ContainHTEventType(HTEventType.StopAccumlate))
                {
                    EventTypeManager.Broadcast(HTEventType.StopAccumlate);
                }

                _accumlateBroadCast = false;
                _accumlateFeedBacks?.StopFeedbacks();

                _generalAttack = true;
            }

            if (CurrentWeapon != null)
            {
                if ((CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBetweenUses)
                && ((_inputManager.ShootAxis == MMInput.ButtonStates.Off) && 
                (_inputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.Off)))
                {
                    CurrentWeapon.WeaponInputStop();
                }
            }
        }

        /// <summary>
        /// Triggers an attack if the weapon is idle and an input has been buffered
        /// </summary>
        protected virtual void HandleBuffer()
        {
            if (CurrentWeapon == null)
            {
                return;
            }

            // if we are currently buffering an input and if the weapon is now idle
            if (_buffering && (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle))
            {
                // and if our buffer is still valid, we trigger an attack
                if (Time.time < _bufferEndsAt)
                {
                    ShootStart();
                }
                else
                {
                    _buffering = false;
                }
            }
        }

        /// <summary>
        /// Causes the character to start shooting
        /// </summary>
        public virtual void ShootStart()
        {
            if (GameManager.Instance.PlayingTimeline) return;

            if (dash != null && dash._dashing && dash.directionEnum == DashDirectionEnum.SowrdMan)//sprint attack
            {
                if (_sprintWeapon != null)               
                {
                    _sprintWeapon.WeaponUse();
                }
                return;
            }

            if (_sprintWeapon != null)
            {
                AnimatorStateInfo animState = _animator.GetCurrentAnimatorStateInfo(1); 
                if (animState.IsName("SprintAttack"))
                {
                    return;
                }
            }

            if (_skillReleseTrigger != null && _skillReleseTrigger.OnAttacking) return;

            // if the Shoot action is enabled in the permissions, we continue, if not we do nothing.  If the player is dead we do nothing.
            if (!AbilityPermitted
                || (CurrentWeapon == null)
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal))
            {
                return;
            }

            //  if we've decided to buffer input, and if the weapon is in use right now
            if (BufferInput && (CurrentWeapon.WeaponState.CurrentState != Weapon.WeaponStates.WeaponIdle))
            {
                // if we're not already buffering, or if each new input extends the buffer, we turn our buffering state to true
                ExtendBuffer();
            }

            if (BufferInput && RequiresPerfectTile && (_characterGridMovement != null))
            {
                if (!_characterGridMovement.PerfectTile)
                {
                    ExtendBuffer();
                    return;
                }
                else
                {
                    _buffering = false;
                }
            }

            PlayAbilityStartFeedbacks();
            CurrentWeapon.WeaponInputStart();

            if (characterBattleController != null)
            {
                characterBattleController.StartBattleState();
            }
        }

        protected virtual void ExtendBuffer()
        {
            if (!_buffering || NewInputExtendsBuffer)
            {
                _buffering = true;
                _bufferEndsAt = Time.time + MaximumBufferDuration;
            }
        }

        /// <summary>
        /// Causes the character to stop shooting
        /// </summary>
        public virtual void ShootStop()
        {
            // if the Shoot action is enabled in the permissions, we continue, if not we do nothing
            if (!AbilityPermitted
                || (CurrentWeapon == null))
            {
                return;
            }

            if (characterBattleController != null)
            {
                characterBattleController.StopBattleState();
            }

            if (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle)
            {
                return;
            }

            if ((CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReload)
                || (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReloadStart)
                || (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponReloadStop))
            {
                return;
            }

            if ((CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBeforeUse) && 
                (!CurrentWeapon.DelayBeforeUseReleaseInterruption))
            {
                return;
            }

            if ((CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBetweenUses) && 
                (!CurrentWeapon.TimeBetweenUsesReleaseInterruption))
            {
                return;
            }

            if (CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse)
            {
                return;
            }

            StopStartFeedbacks();
            PlayAbilityStopFeedbacks();
            CurrentWeapon.TurnWeaponOff();


        }

        /// <summary>
        /// Reloads the weapon
        /// </summary>
        protected virtual void Reload()
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.InitiateReloadWeapon();
            }
        }

        /// <summary>
        /// Changes the character's current weapon to the one passed as a parameter
        /// </summary>
        /// <param name="newWeapon">The new weapon.</param>
        public virtual void ChangeWeapon(Weapon newWeapon, string weaponID, bool combo = false)
        {
            // if the character already has a weapon, we make it stop shooting
            if (CurrentWeapon != null)
            {
                if (!combo)
                {
                    ShootStop();
                    if (_weaponAim != null) { _weaponAim.RemoveReticle(); }
                    Destroy(CurrentWeapon.gameObject);
                }
            }

            if (newWeapon != null)
            {
                if (!combo)
                {
                    CurrentWeapon = (Weapon)Instantiate(newWeapon, WeaponAttachment.transform.position + newWeapon.WeaponAttachmentOffset, WeaponAttachment.transform.rotation);
                }
                CurrentWeapon.transform.parent = WeaponAttachment.transform;
                CurrentWeapon.transform.localPosition = newWeapon.WeaponAttachmentOffset;
                CurrentWeapon.SetOwner(_character, this);
                CurrentWeapon.WeaponID = weaponID;
                _weaponAim = CurrentWeapon.gameObject.MMGetComponentNoAlloc<WeaponAim>();
                // we handle (optional) inverse kinematics (IK) 
                if (_weaponIK != null)
                {
                    _weaponIK.SetHandles(CurrentWeapon.LeftHandHandle, CurrentWeapon.RightHandHandle);
                }
                _projectileWeapon = CurrentWeapon.gameObject.MMFGetComponentNoAlloc<ProjectileWeapon>();
                if (_projectileWeapon != null)
                {
                    _projectileWeapon.SetProjectileSpawnTransform(ProjectileSpawn);
                }
                // we turn off the gun's emitters.
                CurrentWeapon.Initialization();
                CurrentWeapon.InitializeComboWeapons();
                CurrentWeapon.InitializeAnimatorParameters();
                InitializeAnimatorParameters();
            }
            else
            {
                CurrentWeapon = null;
            }
        }

        /// <summary>
        /// Flips the current weapon if needed
        /// </summary>
        public override void Flip()
        {
        }

        /// <summary>
        /// Updates the ammo display bar and text.
        /// </summary>
        public virtual void UpdateAmmoDisplay()
        {
            if ((GUIManager.Instance != null) && (_character.CharacterType == Character.CharacterTypes.Player))
            {
                if (CurrentWeapon == null)
                {
                    GUIManager.Instance.SetAmmoDisplays(false, _character.PlayerID);
                    return;
                }

                if (!CurrentWeapon.MagazineBased && (CurrentWeapon.WeaponAmmo == null))
                {
                    GUIManager.Instance.SetAmmoDisplays(false, _character.PlayerID);
                    return;
                }

                if (CurrentWeapon.WeaponAmmo == null)
                {
                    GUIManager.Instance.SetAmmoDisplays(true, _character.PlayerID);
                    GUIManager.Instance.UpdateAmmoDisplays(CurrentWeapon.MagazineBased, 0, 0, CurrentWeapon.CurrentAmmoLoaded, CurrentWeapon.MagazineSize, _character.PlayerID, false);
                    return;
                }
                else
                {
                    GUIManager.Instance.SetAmmoDisplays(true, _character.PlayerID);
                    GUIManager.Instance.UpdateAmmoDisplays(CurrentWeapon.MagazineBased, CurrentWeapon.WeaponAmmo.CurrentAmmoAvailable, CurrentWeapon.WeaponAmmo.MaxAmmo, CurrentWeapon.CurrentAmmoLoaded, CurrentWeapon.MagazineSize, _character.PlayerID, true);
                    return;
                }
            }
        }

        /// <summary>
        /// Adds required animator parameters to the animator parameters list if they exist
        /// </summary>
        protected override void InitializeAnimatorParameters()
        {
            if (CurrentWeapon == null)
            { return; }

            RegisterAnimatorParameter(_weaponEquippedAnimationParameterName, AnimatorControllerParameterType.Bool, out _weaponEquippedAnimationParameter);
            RegisterAnimatorParameter(_weaponEquippedIDAnimationParameterName, AnimatorControllerParameterType.Int, out _weaponEquippedIDAnimationParameter);
        }

        /// <summary>
        /// Override this to send parameters to the character's animator. This is called once per cycle, by the Character
        /// class, after Early, normal and Late process().
        /// </summary>
        public override void UpdateAnimator()
        {
            MMAnimatorExtensions.UpdateAnimatorBool(_animator, _weaponEquippedAnimationParameter, (CurrentWeapon != null), _character._animatorParameters);
            if (CurrentWeapon == null)
            {
                MMAnimatorExtensions.UpdateAnimatorInteger(_animator, _weaponEquippedIDAnimationParameter, -1, _character._animatorParameters);
                return;
            }
            else
            {
                MMAnimatorExtensions.UpdateAnimatorInteger(_animator, _weaponEquippedIDAnimationParameter, CurrentWeapon.WeaponAnimationID, _character._animatorParameters);
            }
        }

        protected override void OnHit()
        {
            base.OnHit();
            if (GettingHitInterruptsAttack && (CurrentWeapon != null))
            {
                CurrentWeapon.Interrupt();
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            ShootStop();
            StopWeaponAttackAnim();
            
            if (CurrentWeapon != null)
            {
                ChangeWeapon(null, "");
            }
        }

        protected override void OnRespawn()
        {
            base.OnRespawn();
            Setup();
        }

        /// <summary>
        /// Start accumlate normal attack  e.p magician bigger projectile
        /// </summary>
        private void StartAccumlate()
        {
            _accumlate = true;
            _accumlateEndAt = Time.time + _accumlateTime;
        }

        /// <summary>
        /// process accumalte event e.p accumlate value increas with deltaTime
        /// </summary>
        private void ProcessAccumlate()
        {
            if (_accumlate)
            {
                float percent = (_accumlateTime - (_accumlateEndAt - Time.time)) / _accumlateTime;
                if (percent >= 0.95f)
                {
                    CurrentWeapon.WeaponState.ChangeState(Weapon.WeaponStates.WeaponAccumlate);
                    _accumlate = false;
                    CurrentWeapon.AccumlateWeapon = true;
                }
                else
                {
                    WeaponAccumlateEvent?.Invoke(percent);
                    if (percent > 0.1f)
                    {
                        _generalAttack = false;
                        if (!_accumlateBroadCast)
                        {
                            if (EventTypeManager.ContainHTEventType(HTEventType.StartAccumlate))
                            {
                                EventTypeManager.Broadcast(HTEventType.StartAccumlate);
                            }

                            _accumlateBroadCast = true;
                        }
                        _accumlateFeedBacks?.PlayFeedbacks();
                    }
                    else
                    {
                        _generalAttack = true;
                    }

                }
            }

        }

        /// <summary>
        /// 初始化 AccumlateBar
        /// </summary>
        void InitialAccumlateBar()
        {
            if (_accumlateBar != null && _accumlateTrs != null)
            {
                HTAccumlateBar bar = GameObject.Instantiate(_accumlateBar, _accumlateTrs);
                bar.transform.SetLocalPosXYZ(0, 0, 0);
            }
        }

        /// <summary>
        /// StopWeaponAttackAnim e.p when character dead 
        /// </summary>
        void StopWeaponAttackAnim()
        {
            _animator.SetTrigger("StopWeaponRelese");
        }


    }
}