using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using NaughtyAttributes;
using System;
namespace HTLibrary.Application
{
    public class SkillReleaseTrigger : MonoBehaviour
    {

        public List<SkillSetting> skillSetting = new List<SkillSetting>();

        public Transform shootPoint;
        public Transform selfSpawnPoint;

        bool onAttacking = false;
        public bool OnAttacking
        {
            get
            {
                return onAttacking;
            }
            private set
            {
                onAttacking = value;
            }
        }

        private CharacterMovement characterMovement;
        private CharacterDash3D dash;
        private Health _health;
        [HideInInspector]
        public Animator anim;
        private CharacterOrientation3D _orientation3D;

        bool dashing = false;
        [HideInInspector]
        public Transform model;
        Vector3 _dashDestination;
        SkillSetting currentSkill;
        Vector3 _newPosition;
        TopDownController3D _controller;
        float dashTimer = 0;

        [HideInInspector]
        public Dictionary<int, float> recoderCooldown = new Dictionary<int, float>();

        private Status status;

        public bool FreezeSkill { get; set; }

        [MMInformation("能够打断技能释放", MMInformationAttribute.InformationType.Info, false)]
        public bool CanInteruptSkillRelese = false;

        IEnumerator IReleseSkill;

        public event Action Skill01MonitorEvent;

        public event Action Skill02MonitorEvent;

        private float pause01Time;

        private float pause02Time;

        private GameObject _castEffct;

        public GameObject AddForcePositionGo;

        private string loopAniamtionName;


        //Jump
        ObjectGravity _objectGravity;
        bool jumpStart = false;
        float jumoTimer;
        CharacterController _CharacterController;
        private float startJumpTime = 0;
        private bool frezzingAnim = false;
        private float startFreezeTime = 0;

        //等待技能释放
        List<int> _WaitSkillList = new List<int>();
        Dictionary<int, GameObject> _waitSkillDicts = new Dictionary<int, GameObject>();

        /// <summary>
        /// 飞行
        /// </summary>

        Vector3 _targetPosition;
        Transform _targetAim;
        float _flyY = 0;
        MMStateMachine<FlyState> _flyState;
        Vector3 motion;
        float _downDecleration = 1;
        AIBrain _aiBrain;
        float _targetDuration = 0;
        float _targetPosY;
        float _upSpeed;
        
        public Vector3 SpawnDirection { get; set; }

        public enum FlyState
        {
            None,
            Up,
            Duration,
            Down
        }

        private void OnEnable()
        {
            GameManager.Instance.PausedGameEvent += PauseGameEvent;

            if (_health == null)
            {
                _health = GetComponent<Health>();
            }

            if (_health == null)
            {
                return;
            }

            _health.OnDeath += StopSkillRelese;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PausedGameEvent -= PauseGameEvent;
            }

            if (_health == null)
            {
                _health = GetComponent<Health>();
            }


            OnAttacking = false;

            if (_health == null)
            {
                return;
            }

            _health.OnDeath -= StopSkillRelese;

        }
        private void Start()
        {
            characterMovement = GetComponent<CharacterMovement>();

            if (anim == null)
            {
                anim = GetComponent<Character>()._animator;
            }

            if (model == null)
            {
                model = GetComponent<Character>().CharacterModel.transform;
            }

            _controller = GetComponent<TopDownController3D>();
            dash = GetComponent<CharacterDash3D>();
            status = GetComponent<Status>();

            _objectGravity = GetComponent<ObjectGravity>();
            _CharacterController = GetComponent<CharacterController>();

            _aiBrain = GetComponent<AIBrain>();
            _flyState = new MMStateMachine<FlyState>(this.gameObject, false);
            _flyState.ChangeState(FlyState.None);

            _orientation3D = GetComponent<CharacterOrientation3D>();
        }

        public void ReleseWaitSkill(int index)
        {
            recoderCooldown[index] = skillSetting[index].cooldown + Time.time;
            _WaitSkillList.Remove(index);

            if (_waitSkillDicts.ContainsKey(index))
            {
                _waitSkillDicts.Remove(index);
            }
        }

        public void TriggerSkill(int index)
        {
            if (GameManager.Instance.PlayingTimeline) return;

            if (_WaitSkillList.Count > 0)
            {
                if (_WaitSkillList.Contains(index))
                {
                    if (EventTypeManager.ContainHTEventType(HTEventType.MarkArrow))
                    {
                        EventTypeManager.Broadcast(HTEventType.MarkArrow);
                    }

                    return;
                }
            }

            if (OnAttacking || FreezeSkill || (dash != null && dash._dashing) || !SkillPreparation(index)) return;

            if (index == 0 || index == 1)
            {
                anim.SetTrigger("StopWeaponRelese");//停止所有武器动画
            }

            IReleseSkill = ReleaseSkill(index);
            StartCoroutine(IReleseSkill);
        }

        IEnumerator ReleaseSkill(int skill)
        {

            if (dash != null)
            {
                if (!skillSetting[skill].permitDash)
                {

                    dash.AbilityPermitted = false;
                }

            }

            if (this.tag == Tags.Player)
            {
                _orientation3D.RotationMode = CharacterOrientation3D.RotationModes.WeaponDirection;
            }

            OnAttacking = true;

            currentSkill = skillSetting[skill];

            //TODO 播放音效

            if (skillSetting[skill].WaitSkillTrigger)
            {
                _WaitSkillList.Add(skill);
            }

            if (skillSetting[skill].whileAttack == WhileAttack.Immobile)
            {
                if (characterMovement != null)
                {
                    characterMovement.MovementForbidden = true;
                }
            }
            else
            {
                if (characterMovement != null)
                {
                    characterMovement.MovementForbidden = false;
                }
            }

            if (!string.IsNullOrEmpty(skillSetting[skill].animationName))
            {
                if (skillSetting[skill].IsLoop)
                {
                    loopAniamtionName = skillSetting[skill].animationName;
                    anim.SetBool(skillSetting[skill].animationName, true);
                }
                else
                {
                    anim.SetTrigger(skillSetting[skill].animationName);
                }
            }

            //Play timeline 
            if(!string.IsNullOrEmpty(skillSetting[skill]._playableAssetKey))
            {
                TimelineController controller= GetComponent<TimelineController>();
                if(controller!=null)
                {
                    controller.TimelinePlayNoPause(skillSetting[skill]._playableAssetKey);
                }else{
                   Debugs.LogWarning("Timeline Contoller ==null but you still to access it",Color.grey);
                }
            }

            yield return new WaitForEndOfFrame(); //Delay one frame change character forward

            if (_orientation3D != null)
            {
                _orientation3D.CharacterRotationAuthorized = false;
            }

            yield return new WaitForSeconds(skillSetting[skill].castTime);

            if (skillSetting[skill].castEffect != "")
            {
                GameObject castEff = PoolManagerV2.Instance.GetInst(skillSetting[skill].castEffect);
                this._castEffct = castEff;
                castEff.transform.rotation = selfSpawnPoint.rotation;
                Transform tempTrs = selfSpawnPoint;
                castEff.transform.position = tempTrs.position + (tempTrs.forward * skillSetting[skill].castEffectOffset.z) + (tempTrs.right * skillSetting[skill].castEffectOffset.x)
                     + (tempTrs.up * skillSetting[skill].castEffectOffset.y);

                SetHealthOwner(castEff);
            }

            // 技能前瑶 音效
            if (skillSetting[skill].castSoundEffect != null)
            {
                SoundManager.Instance.PlaySound(skillSetting[skill].castSoundEffect, transform.position, false);
            }

            if (skillSetting[skill].BeforeReleseSkillTime > 0)
            {
                if (anim != null)
                {
                    anim.enabled = false;
                }

                yield return new WaitForSeconds(skillSetting[skill].BeforeReleseSkillTime);

                if (anim != null)
                {
                    anim.enabled = true;
                }

            }

            yield return new WaitForSeconds(skillSetting[skill].skillDelay);

            if (skillSetting[skill].skillSpawn == SkillSpawn.FromPlayer)
            {

                if (skillSetting[skill].skillPrefab != "")
                {
                    GameObject shootPrefab = PoolManagerV2.Instance.GetInst(skillSetting[skill].skillPrefab);

                    shootPrefab.transform.rotation = shootPoint.rotation;
                    Transform tempTrs = shootPoint;
                    shootPrefab.transform.position = tempTrs.position + 
                    (tempTrs.forward * skillSetting[skill].skillPositionOffset.z) +
                     (tempTrs.right * skillSetting[skill].skillPositionOffset.x)
                        + (tempTrs.up * skillSetting[skill].skillPositionOffset.y);

                    if (SpawnDirection != Vector3.zero)  ///更改定制朝向
                    {
                        shootPrefab.transform.forward = SpawnDirection;
                        SpawnDirection = Vector3.zero;
                    }

                    SetOwenr(shootPrefab);
                    SetOwnerRotation(shootPrefab, shootPoint);

                    SpikeSkill spikeSkill = shootPrefab.GetComponent<SpikeSkill>();
                    if (spikeSkill != null)
                    {
                        SetSkillAddForceDirOwner(spikeSkill);
                    }

                    SetWaitSkillObj(skill, shootPrefab);
                    SetCantCoolDown(shootPrefab,skill);
                }

            }
            else if (skillSetting[skill].skillSpawn == SkillSpawn.SlefSpawn)
            {
                if (skillSetting[skill].skillPrefab != "")
                {
                    GameObject shootPrefab = PoolManagerV2.Instance.GetInst(skillSetting[skill].skillPrefab);

                    shootPrefab.transform.rotation = selfSpawnPoint.rotation;
                    Transform tempTrs = selfSpawnPoint;
                    shootPrefab.transform.position = tempTrs.position + (tempTrs.forward * skillSetting[skill].skillPositionOffset.z) +
                        (tempTrs.right * skillSetting[skill].skillPositionOffset.x)
                        + (tempTrs.up * skillSetting[skill].skillPositionOffset.y);

                    if (SpawnDirection != Vector3.zero)  ///更改定制朝向
                    {
                        shootPrefab.transform.forward = SpawnDirection;
                        SpawnDirection = Vector3.zero;
                    }

                    SetOwenr(shootPrefab);
                    SetOwnerRotation(shootPrefab, selfSpawnPoint);

                    SpikeSkill spikeSkill = shootPrefab.GetComponent<SpikeSkill>();
                    if (spikeSkill != null)
                    {
                        SetSkillAddForceDirOwner(spikeSkill);
                    }

                    SetWaitSkillObj(skill, shootPrefab);
                    SetCantCoolDown(shootPrefab,skill);
                }
            }
            else if (skillSetting[skill].skillSpawn == SkillSpawn.AtMouse)
            {
                if (skillSetting[skill].skillPrefab != "")
                {
                    GameObject shootPrefab = PoolManagerV2.Instance.GetInst(skillSetting[skill].skillPrefab);
                    shootPrefab.transform.position = RayCastCheckUtility.Instance.RayCastPoint();
                    SetOwenr(shootPrefab);
                    SetOwnerRotation(shootPrefab, shootPoint);

                    SpikeSkill spikeSkill = shootPrefab.GetComponent<SpikeSkill>();
                    if (spikeSkill != null)
                    {
                        SetSkillAddForceDirOwner(spikeSkill);
                    }

                    SetWaitSkillObj(skill, shootPrefab);
                    SetCantCoolDown(shootPrefab,skill);
                }
            }

            if (skillSetting[skill].soundEffect != null)
            {
                SoundManager.Instance.PlaySound(skillSetting[skill].soundEffect, transform.position, false);//播放音效
            }


            if (skillSetting[skill].CanJump)
            {
                StartJump();
            }


            //TODO 技能所带来的人物位移 
            if (skillSetting[skill].changePosition == ChangePosition.Dash)
            {
                DashForwardStart(skill);
            }

            if (skillSetting[skill].Fly)
            {
                StartFly(skill);
            }

            yield return new WaitForSeconds(skillSetting[skill].skillTime);

            if (characterMovement != null)
            {
                characterMovement.MovementForbidden = false;
            }

            if (skillSetting[skill].IsLoop)
            {
                anim.SetBool(skillSetting[skill].animationName, false);
            }

            if (dash != null)
            {
                dash.AbilityPermitted = true;
            }

            OnAttacking = false;

            if (this.tag == Tags.Player)
            {
                _orientation3D.CharacterRotationAuthorized = true;
                _orientation3D.RotationMode = CharacterOrientation3D.RotationModes.MovementDirection;
            }

            anim.ResetTrigger("StopWeaponRelese");//重置武器释放Trigger
        }

        /// <summary>
        /// 停止技能释放
        /// </summary>
        public void StopSkillRelese()
        {
            if (CanInteruptSkillRelese||_health.CurrentHealth<=0)
            {
                if (IReleseSkill != null)
                {
                    StopCoroutine(IReleseSkill);
                }

                if (status != null)
                {
                    if (status._Status != CharacterStatus.Freeze&&status._Status!=CharacterStatus.Frozen)
                    {
                        characterMovement.MovementForbidden = false;
                        if (dash != null)
                        {
                            dash.AbilityPermitted = true;
                        }
                    }

                }

                OnAttacking = false;
                _orientation3D.CharacterRotationAuthorized = true;
                _orientation3D.RotationMode = CharacterOrientation3D.RotationModes.MovementDirection;


                if (dashing)
                {
                    DashForwardStop();
                }

                if (anim != null)
                {
                    anim.enabled = true;
                }

                if (!string.IsNullOrEmpty(loopAniamtionName))
                {
                    anim.SetBool(loopAniamtionName, false);
                }

                if (this._castEffct != null)
                {
                    if (_castEffct.activeInHierarchy)
                    {
                        _castEffct.SetActive(false);
                    }
                }

                anim.ResetTrigger("StopWeaponRelese");//重置武器释放Trigger
            }
        }

        void SetOwenr(GameObject shootPrefab)
        {
            FollowOwner owner = shootPrefab.GetComponent<FollowOwner>();

            if (owner != null)
            {
                owner.SetOwner(this.transform);
            }

            AttackSpeedBuff _owner1 = shootPrefab.GetComponent<AttackSpeedBuff>();

            if (_owner1 != null)
            {
                _owner1.SetOwner(this.gameObject);
            }

            PenSpBuff _owner2 = shootPrefab.GetComponent<PenSpBuff>();

            if (_owner2 != null)
            {
                _owner2.SetOwner(this.gameObject);
            }

            HTColliderFeedBack htColliderFeedback = shootPrefab.GetComponent<HTColliderFeedBack>();

            if (htColliderFeedback != null)
            {
                htColliderFeedback.SetOwner(this.gameObject);
            }

            HTRestoreHP hpRestore = shootPrefab.GetComponent<HTRestoreHP>();
            if (hpRestore != null)
            {
                hpRestore.SetOwner(this.gameObject);
            }

            MarkArrow markArrow = shootPrefab.GetComponent<MarkArrow>();
            if (markArrow != null)
            {
                int index = skillSetting.IndexOf(currentSkill);
                markArrow.SetOwner(this.gameObject.transform, index);
            }
        }


        void SetOwnerRotation(GameObject shootPrefab, Transform rotation)
        {
            FollowRotation followRotation = shootPrefab.GetComponent<FollowRotation>();
            if (followRotation != null)
            {
                followRotation.SetRotation(rotation);
            }
        }

        void SetHealthOwner(GameObject castEffect)
        {
            HTInvincible invincible = castEffect.GetComponent<HTInvincible>();
            if (invincible != null)
            {
                invincible.SetOwner(GetComponent<Health>());
            }
        }

        /// <summary>
        /// 设置技能伤害区域的拥有者
        /// </summary>
        /// <param name="skill"></param>
        void SetSkillAddForceDirOwner(SpikeSkill skill)
        {
            if (AddForcePositionGo == null) return;
            skill.Owner = AddForcePositionGo;
        }
        private void DashForwardStart(int skill)
        {
            _controller.SetLastGoundPosition(this.transform.position);
            dashTimer = 0;
            dashing = true;
            _dashDestination = this.transform.position + model.forward * skillSetting[skill].changeValue;
        }

        private void ProcessForwardDash()
        {
            if (dashing)
            {
                if (dashTimer < currentSkill.changeTimer && OnAttacking)
                {
                    _newPosition = Vector3.Lerp(this.transform.position, _dashDestination, dashTimer / currentSkill.changeTimer);
                    _controller.MovePosition(_newPosition);
                    dashTimer += Time.deltaTime;
                }
                else
                {
                    DashForwardStop();
                }

            }

        }

        private float jumpZpeed;

        /// <summary>
        /// 开始跳跃
        /// </summary>
        private void StartJump()
        {
            _objectGravity.GravityActive = true;
            if (_objectGravity != null)
            {
                _objectGravity.SetJumpValue(currentSkill.JumpForce);
            }

            jumpStart = true;
            startJumpTime = Time.time;

            if (this.tag == Tags.Player)
            {

                Plane playerPlane = new Plane(Vector3.up, Vector3.zero);
                playerPlane.SetNormalAndPosition(Vector3.up, this.transform.position);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance;
                Vector3 targetPosition = Vector3.zero;
                if (playerPlane.Raycast(ray, out distance))
                {
                    targetPosition = ray.GetPoint(distance);
                }

                float distance2 = Vector3.Distance(targetPosition, transform.position);
                jumpZpeed = distance2 / currentSkill.JumpTime;

                jumpZpeed = Mathf.Clamp(jumpZpeed, 0, currentSkill.JumpZSpeed);
            }

        }

        void ProcessJump()
        {
            if (jumpStart)
            {
                if (jumoTimer < currentSkill.JumpTime && OnAttacking)
                {
                    _CharacterController.Move(model.forward * Time.deltaTime * jumpZpeed);
                    jumoTimer += Time.deltaTime;

                    if (!frezzingAnim && Time.time - startJumpTime > currentSkill.StartFrezzeAnim)
                    {
                        anim.enabled = false;
                        frezzingAnim = true;
                        startFreezeTime = Time.time;
                    }

                    if (frezzingAnim)
                    {
                        if (Time.time - startFreezeTime > currentSkill.FrezzeAnimDuration)
                        {
                            anim.enabled = true;
                        }
                    }
                }
                else
                {
                    StopJump();
                }
            }
        }

        void StopJump()
        {
            _objectGravity.GravityActive = false;
            jumpStart = false;
            jumoTimer = 0;
            frezzingAnim = false;
            startJumpTime = 0;
            startFreezeTime = 0;
        }


        private void FixedUpdate()
        {
            if (GameManager.Instance.PlayingTimeline) return;
            ProcessForwardDash();
            ProcessJump();
            ProcessFly();

            if (Skill01MonitorEvent != null)
            {
                Skill01MonitorEvent();
            }

            if (Skill02MonitorEvent != null)
            {
                Skill02MonitorEvent();
            }

        }

        private void DashForwardStop()
        {
            dashing = false;
        }

        public bool SkillPreparation(int skill)
        {
            if(skillSetting[skill]._cantCoolDown)
                return false;
            if (recoderCooldown.ContainsKey(skill))
            {
                float nextfire = recoderCooldown.TryGet<int, float>(skill);
                if (Time.time >= nextfire)
                {
                    recoderCooldown.Remove(skill);
                    recoderCooldown.Add(skill, skillSetting[skill].cooldown + Time.time);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                recoderCooldown.Add(skill, skillSetting[skill].cooldown + Time.time);
                return true;
            }
        }

        /// <summary>
        /// 技能百分比接口
        /// </summary>
        /// <param name="skillID">技能ID</param>
        /// <returns></returns>
        public float SkillCoolDownPercent(int skillID)
        {
            if(skillSetting[skillID]._cantCoolDown)
            {
                return 0;
            }

            if (_WaitSkillList.Contains(skillID))
            {

                return 0;
            }

            if (recoderCooldown.ContainsKey(skillID))
            {
                return (recoderCooldown.TryGet<int, float>(skillID) - Time.time) / skillSetting[skillID].cooldown;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 减少指定技能ID 的技能冷却
        /// </summary>
        /// <param name="skillID"></param>
        /// <param name="value"></param>
        public void ReduceSkillCooldown(int skillID, float value)
        {
            if (recoderCooldown.ContainsKey(skillID))
            {
                recoderCooldown[skillID] -= value;
            }
        }

        /// <summary>
        /// 刷新指定ID的技能冷却
        /// </summary>
        /// <param name="skillID"></param>
        public void RefreshSkillCooldown(int skillID)
        {
            if (recoderCooldown.ContainsKey(skillID))
            {
                float currentValue = recoderCooldown.TryGet<int, float>(skillID);
                ReduceSkillCooldown(skillID, currentValue);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (currentSkill == null) return;
            if (currentSkill.preventThrough)
            {
                if (MMLayers.LayerInLayerMask(collider.gameObject.layer, currentSkill.layerMask))
                {
                    DashForwardStop();
                }
            }
        }

        void PauseGameEvent(bool pause)
        {
            if (pause)
            {
                pause01Time = Time.time;
                pause02Time = Time.time;

            }
            else
            {
                if (recoderCooldown.ContainsKey(0))
                {
                    float lifeTime = recoderCooldown.TryGet<int, float>(0);
                    lifeTime += Time.time - pause01Time;
                    recoderCooldown[0] = lifeTime;
                }

                if (recoderCooldown.ContainsKey(1))
                {
                    float lifeTime = recoderCooldown.TryGet<int, float>(1);
                    lifeTime += Time.time - pause02Time;
                    recoderCooldown[1] = lifeTime;
                }
            }
        }


        /// <summary>
        /// 开始飞行
        /// </summary>
        /// <param name="skill"></param>
        void StartFly(int skill)
        {
            if (_flyState == null)
            {
                _flyState = new MMStateMachine<FlyState>(this.gameObject, false);
            }

            if (_objectGravity != null)
            {
                _objectGravity.GravityActive = false;
            }

            _controller.GravityActive = false;

            if (_aiBrain != null)
            {
                _targetAim = _aiBrain.Target;
            }

            _targetPosY = transform.position.y + currentSkill.TargetHeight;

            _flyState.ChangeState(FlyState.Up);
            _flyY = 1;
            _upSpeed = currentSkill.UpSpeed;
        }


        /// <summary>
        /// 处理飞行
        /// </summary>
        void ProcessFly()
        {
            if (transform.position.y >= _targetPosY && _flyState.CurrentState == FlyState.Up)
            {
                _flyState.ChangeState(FlyState.Duration);
                _targetDuration = currentSkill.FlyDuration + Time.time;
            }

            if (_flyState.CurrentState == FlyState.Duration && _targetDuration - Time.time <= 0)
            {
                _flyState.ChangeState(FlyState.Down);
            }

            if (_flyState.CurrentState == FlyState.Down)
            {
                StopFly();
            }

            switch (_flyState.CurrentState)
            {
                case FlyState.None:
                    break;
                case FlyState.Up:
                    _upSpeed -= currentSkill.UpDeclerationMultiple * Time.deltaTime;
                    motion = new Vector3(0, _flyY, 0) * _upSpeed * Time.deltaTime;
                    _CharacterController.Move(motion);
                    break;
                case FlyState.Duration:
                    _flyY = 0;
                    _targetPosition = new Vector3(_targetAim.position.x, transform.position.y, _targetAim.position.z);
                    _newPosition = Vector3.Lerp(this.transform.position, _targetPosition, Time.deltaTime * currentSkill.FollowTargetSpeed);
                    _controller.MovePosition(_newPosition);
                    break;
                case FlyState.Down:
                    _flyY = -1 * _downDecleration;
                    motion = new Vector3(0, _flyY, 0) * currentSkill.DownSpeed * Time.deltaTime;
                    _downDecleration += Time.deltaTime * currentSkill.DownDecelerationMultiple;
                    _CharacterController.Move(motion);
                    break;
            }
        }

        /// <summary>
        /// 停止飞行
        /// </summary>
        void StopFly()
        {
            if (_CharacterController.collisionFlags != 0)
            {
                _flyState.ChangeState(FlyState.None);
                if (_objectGravity != null)
                {
                    _objectGravity.GravityActive = true;
                }
                _controller.GravityActive = true;
                _flyY = 0;
                _downDecleration = 1;

            }
        }


        /// <summary>
        /// 添加等待技能字典所对应技能物体
        /// </summary>
        /// <param name="index"></param>
        /// <param name="skillObj"></param>
        void SetWaitSkillObj(int index, GameObject skillObj)
        {
            if (!_WaitSkillList.Contains(index))
            {
                return;
            }

            if (!_waitSkillDicts.ContainsKey(index))
            {
                _waitSkillDicts.Add(index, skillObj);
            }
        }

        /// <summary>
        /// 得到等待技能进度
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetWaitSkillTimePercent(int index)
        {
            GameObject skillObj = _waitSkillDicts.TryGet<int, GameObject>(index);

            if (skillObj == null)
            {
                return 0;
            }

            AutoHide _autoHide = skillObj.GetComponent<AutoHide>();
            if (_autoHide != null)
            {
                return _autoHide.GetLifeTimePercent();
            }

            return 0;
        }

        /// <summary>
        /// Prevent when hit obstacle 
        /// </summary>
        /// <param name="hit"></param>

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_flyState.CurrentState == FlyState.Down)
            {
                if (MoreMountains.Tools.MMLayers.LayerInLayerMask(hit.gameObject.layer, currentSkill.layerMask))
                {

                    if (this.transform.position.x > 0)
                    {
                        this.transform.position = new Vector3(this.transform.position.x - 5.0f, this.transform.position.y, this.transform.position.z);
                    }
                    else
                    {
                        this.transform.position = new Vector3(this.transform.position.x + 5.0f, this.transform.position.y, this.transform.position.z);
                    }

                    if (this.transform.position.z > 0)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 5.0f);
                    }
                    else
                    {
                        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 5.0f);
                    }
                }
            }
        }
        
        /// <summary>
        /// Set current skill can't enter cool down 
        /// </summary>
        /// <param name="skillGo"></param>
        private void SetCantCoolDown(GameObject skillGo,int skillIndex)
        {
            CallSkill callSkill=skillGo.GetComponent<CallSkill>();
            if(callSkill)
            {
                currentSkill._cantCoolDown=true;
                callSkill.CorresPondingSkillID=skillIndex;
                Debugs.LogInformation("Cant Enter cool down skill slot index:"+skillIndex,Color.yellow);
            }
        }

        /// <summary>
        /// Set id is skill id can enter cool down
        /// </summary>
        /// <param name="skill"></param>
        public void RestoreCoolDown(int skillID)
        {
            skillSetting[skillID]._cantCoolDown=false;
            recoderCooldown[skillID] = skillSetting[skillID].cooldown + Time.time;

            Debugs.LogInformation("Skill slot index:"+skillID+" cant enter cool down: "+skillSetting[skillID]._cantCoolDown,
            Color.red);
        }

        /// <summary>
        /// Get skill of skillid is can't enter cooldown
        /// </summary>
        /// <param name="skillID"></param>
        /// <returns></returns>
        public bool GetCantCoolDown(int skillID)
        {
            return skillSetting[skillID]._cantCoolDown;
        }

    }


}
