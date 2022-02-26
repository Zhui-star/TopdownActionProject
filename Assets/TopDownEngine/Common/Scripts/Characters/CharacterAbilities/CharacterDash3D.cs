 using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using System;
using HTLibrary.Utility;
using DG.Tweening;
namespace MoreMountains.TopDownEngine
{
    public enum DashDirectionEnum { Archer, SowrdMan, AI }

    public enum DashType
    {
        LerpPosition,
        Flash
    }
    /// <summary>
    /// Add this ability to a 3D character and it'll be able to dash (cover the specified distance in the specified time)
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/Abilities/Character Dash 3D")]
    public class CharacterDash3D : CharacterAbility
    {
        /// the possible dash modes (fixed = always the same direction)
        public enum DashModes { Fixed, MainMovement, SecondaryMovement, MousePosition }
        /// the current dash mode
        public DashModes DashMode = DashModes.MainMovement;
      
        [Header("Dash")]
        /// the direction of the dash, relative to the character
        public Vector3 DashDirection = Vector3.forward;
        /// the distance to cover
        public float DashDistance = 10f;
        /// the duration of the dash
        public float DashDuration = 0.5f;
        /// the curve to apply to the dash's acceleration
        public AnimationCurve DashCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

        [Header("Cooldown")]
        /// this ability's cooldown
        public MMCooldown Cooldown;
        
        /// the feedbacks to play when dashing
        public MMFeedbacks DashFeedback;

        public bool _dashing;
        protected bool _dashStartedThisFrame;
        protected float _dashTimer;
        protected Vector3 _dashOrigin;
        protected Vector3 _dashDestination;
        protected Vector3 _newPosition;
        protected Vector3 _dashAngle = Vector3.zero;
        protected Vector3 _inputDirection;
        protected Plane _playerPlane;
        protected Camera _mainCamera;
        protected const string _dashingAnimationParameterName = "Dashing";
        protected const string _dashStartedAnimationParameterName = "DashStarted";
        protected int _dashingAnimationParameter;
        protected int _dashStartedAnimationParameter;

        public DashDirectionEnum directionEnum;

        public event Action<float> MonitorCDEvent;

        [Header("当攻击时突然冲刺")]
        public int animIndex;

        public List<string> animationClipName;

        bool IsAttackAnim = false;

        [Header("冲刺过程的反馈")]
        public MMFeedbacks dashingFeedbakcs;

        CharacterOrientation3D orientation;

        [Header("闪避类型")]
        public DashType _dashType = DashType.LerpPosition;

        [Header("闪避阻碍物 （只针对闪现类型)")]
        public LayerMask _preventFlashLayer;

        RayCastCheckUtility _rayCastCheckUtility;

        /// <summary>
        /// On init we initialize our cooldown and feedback
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            _playerPlane = new Plane(Vector3.up, Vector3.zero);
            _mainCamera = Camera.main;
            Cooldown.Initialization();
            DashFeedback?.Initialization(this.gameObject);
            dashingFeedbakcs?.Initialization(this.gameObject);

            orientation = GetComponent<CharacterOrientation3D>();

            _rayCastCheckUtility = RayCastCheckUtility.Instance;
        }

        /// <summary>
        /// Watches for input and starts a dash if needed
        /// </summary>
        protected override void HandleInput()
        {
            base.HandleInput();
            if (!AbilityPermitted
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)
                || (_movement.CurrentState == CharacterStates.MovementStates.Jumping))
            {
                return;
            }
            if (_inputManager.DashButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                switch(_dashType)
                {
                    case DashType.LerpPosition:
                        DashStart();
                        break;
                    case DashType.Flash:
                        FlashStart();
                        break;
                }               
            }
        }

        /// <summary>
        /// Starts a dash
        /// </summary>
        public virtual void DashStart()
        {
            if ( GameManager.Instance.PlayingTimeline) return;

            if (!Cooldown.Ready())
            {
                return;
            }

            _controller3D.SetLastGoundPosition(this.transform.position);

            Cooldown.Start();

            if(directionEnum==DashDirectionEnum.SowrdMan)
            {
                orientation.RotationMode = CharacterOrientation3D.RotationModes.MovementDirection;

            }


             float angle;
            _movement.ChangeState(CharacterStates.MovementStates.Dashing);
            _dashing = true;
            _dashTimer = 0f;
            _dashOrigin = this.transform.position;
            _controller.FreeMovement = false;
            DashFeedback?.PlayFeedbacks(this.transform.position);
            PlayAbilityStartFeedbacks();
            _dashStartedThisFrame = true;

            if(InputManager.Instance._primaryMovement.x==0&&InputManager.Instance._primaryMovement.y==0)
            {
                DashMode = DashModes.MousePosition;
            }
            else
            {
                DashMode = DashModes.MainMovement;
            }

            switch (DashMode)
            {
                case DashModes.MainMovement:
                    _dashDestination = ReturnMainMovementDashDestination(out angle);
                    break;

                case DashModes.Fixed:
                    _dashDestination = this.transform.position + DashDirection.normalized * DashDistance;
                    break;

                case DashModes.SecondaryMovement:
                    _inputDirection = _character.LinkedInputManager.SecondaryMovement;
                    _inputDirection.z = _inputDirection.y;
                    _inputDirection.y = 0;

                    angle = Vector3.SignedAngle(this.transform.forward, _inputDirection.normalized, Vector3.up);
                    _dashDestination = this.transform.position + DashDirection.normalized * DashDistance;
                    _dashAngle.y = angle;
                    _dashDestination = MMMaths.RotatePointAroundPivot(_dashDestination, this.transform.position, _dashAngle);

                    _controller.CurrentDirection = (_dashDestination - this.transform.position).normalized;
                    break;

                case DashModes.MousePosition:
                    _dashDestination = ReturnMousePositionDestination(out angle);
                    _controller.CurrentDirection = (_dashDestination - this.transform.position).normalized;
                    break;
            }
        }

        /// <summary>
        /// Stops the dash
        /// </summary>
        public virtual void DashStop()
        {
            Cooldown.Stop();
            _movement.ChangeState(CharacterStates.MovementStates.Idle);
            _dashing = false;
            _controller.FreeMovement = true;
            DashFeedback?.StopFeedbacks();
            StopStartFeedbacks();
            PlayAbilityStopFeedbacks();

            IsAttackAnim = false;
            //orientation.RotationMode = CharacterOrientation3D.RotationModes.MovementDirection;
        }

        /// <summary>
        /// On process ability, we move our character if we're currently dashing
        /// </summary>
        public override void ProcessAbility()
        {
            if ( GameManager.Instance.PlayingTimeline) return;
            
            base.ProcessAbility();
            Cooldown.Update();

            if(MonitorCDEvent!=null)
            {
                MonitorCDEvent(Cooldown.ReturnCDPercent());
            }

            if (_dashing)
            {
                if (_dashTimer < DashDuration)
                {
                    dashingFeedbakcs?.PlayFeedbacks();

                    _newPosition = Vector3.Lerp(_dashOrigin, _dashDestination, DashCurve.Evaluate(_dashTimer / DashDuration));
                    _dashTimer += Time.deltaTime;
                    _controller.MovePosition(_newPosition);
                }
                else
                {
                    DashStop();                   
                }
            }
        }

        /// <summary>
        /// Return dash time relevant data
        /// </summary>
        /// <param name="GetDashingTimeRelevant("></param>
        /// <returns></returns>
        public (float,float) GetDashingTimeRelevant()
        {
            return (_dashTimer, DashDuration);
        }
        
        /// <summary>
        /// Adds required animator parameters to the animator parameters list i)f they exist
        /// </summary>
        protected override void InitializeAnimatorParameters()
        {
            RegisterAnimatorParameter(_dashingAnimationParameterName, AnimatorControllerParameterType.Bool, out _dashingAnimationParameter);
            RegisterAnimatorParameter(_dashStartedAnimationParameterName, AnimatorControllerParameterType.Bool, out _dashStartedAnimationParameter);
        }

        /// <summary>
        /// At the end of each cycle, we send our Running status to the character's animator
        /// </summary>
        public override void UpdateAnimator()
        {
            if (directionEnum == DashDirectionEnum.SowrdMan)
            { 
                AnimatorStateInfo animInfo = _animator.GetCurrentAnimatorStateInfo(animIndex);
                foreach (var temp in animationClipName)
                {
                    if (animInfo.IsName(temp))
                    {
                        IsAttackAnim = true;
                        break;
                    }
                }
            }

            if(!_dashing)
            {
                IsAttackAnim=false;
            }

            if (!IsAttackAnim)
            {
                MMAnimatorExtensions.UpdateAnimatorBool(_animator, _dashingAnimationParameter,
                 (_movement.CurrentState == CharacterStates.MovementStates.Dashing), _character._animatorParameters);
                MMAnimatorExtensions.UpdateAnimatorBool(_animator, _dashStartedAnimationParameter,
                 _dashStartedThisFrame, _character._animatorParameters);
            }


            _dashStartedThisFrame = false;
        }

        /// <summary>
        /// 开始闪现
        /// </summary>
        public void FlashStart()
        {
            if (GameManager.Instance.PlayingTimeline) return;

            if (!Cooldown.Ready())
            {
                return;
            }

            _controller3D.SetLastGoundPosition(this.transform.position);

            Cooldown.Start();

            if (InputManager.Instance._primaryMovement.x == 0 && InputManager.Instance._primaryMovement.y == 0)
            {
                DashMode = DashModes.MousePosition;
            }
            else
            {
                DashMode = DashModes.MainMovement;
            }


            float angle;
            _movement.ChangeState(CharacterStates.MovementStates.Dashing);
            _controller.FreeMovement = false;

            switch(DashMode)
            {
                case DashModes.MainMovement:
                    _dashDestination = ReturnMainMovementDashDestination(out angle);
                    break;

                case DashModes.MousePosition:
                    _dashDestination = ReturnMousePositionDestination(out angle);
                    _controller.CurrentDirection = (_dashDestination - this.transform.position).normalized;
                    break;
            }

            RaycastHit hit = _rayCastCheckUtility.RayCastDiretion(this.transform.position, (_dashDestination - this.transform.position).normalized,
                DashDistance) ;
            Vector3 targetPosition;
            if(hit.collider!=null&&MMLayers.LayerInLayerMask(hit.collider.gameObject.layer,_preventFlashLayer))
            {
                targetPosition = hit.point;
            }
            else
            {
                targetPosition = _dashDestination;
            }
            StartCoroutine(FlashRelese(targetPosition));

            DashFeedback?.PlayFeedbacks();
        }
        /// <summary>
        /// 闪现释放
        /// </summary>
        /// <returns></returns>
        IEnumerator FlashRelese(Vector3 targetPostion)
        {
            Vector3 originModelScale = _model.transform.localScale;
            _model.transform.DOScale(0, 0.2f);
            yield return new WaitForSeconds(0.1f);
            this.transform.position = targetPostion;
            yield return new WaitForSeconds(0.1f);
            _model.transform.DOScale(originModelScale, 0.2f);
            Cooldown.Stop();
            _controller.FreeMovement = true;
            _movement.ChangeState(CharacterStates.MovementStates.Idle);
        }

        /// <summary>
        /// 返回移动方向的目标位移
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        Vector3 ReturnMainMovementDashDestination(out float angle)
        {
            angle = Vector3.SignedAngle(this.transform.forward, new Vector3(InputManager.Instance._primaryMovement.x, 0,
                      InputManager.Instance._primaryMovement.y).normalized
                      , Vector3.up);
            _dashDestination = this.transform.position + DashDirection.normalized * DashDistance;
            _dashAngle.y = angle;
            _dashDestination = MMMaths.RotatePointAroundPivot(_dashDestination, this.transform.position, _dashAngle);
            return _dashDestination;
        }

        /// <summary>
        /// 返回鼠标方向的目标位移
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        Vector3 ReturnMousePositionDestination(out float angle)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
            float distance;
            if (_playerPlane.Raycast(ray, out distance))
            {
                _inputDirection = ray.GetPoint(distance);
            }

            angle = Vector3.SignedAngle(this.transform.forward, (_inputDirection - this.transform.position).normalized, Vector3.up);
            _dashDestination = this.transform.position + DashDirection.normalized * DashDistance;
            _dashAngle.y = angle;
            _dashDestination = MMMaths.RotatePointAroundPivot(_dashDestination, this.transform.position, _dashAngle);
            return _dashDestination;
        }

        
    }
}