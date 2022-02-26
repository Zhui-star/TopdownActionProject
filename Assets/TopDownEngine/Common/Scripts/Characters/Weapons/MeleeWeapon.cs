using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using HTLibrary.Application;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.FeedbacksForThirdParty;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Weapons/Melee Weapon")]
    /// <summary>
    /// A basic melee weapon class, that will activate a "hurt zone" when the weapon is used
    /// </summary>
    public class MeleeWeapon : Weapon
    {
        /// the possible shapes for the melee weapon's damage area
        public enum MeleeDamageAreaShapes { Rectangle, Circle, Box, Sphere }

        [Header("Damage Area")]
        /// the shape of the damage area (rectangle or circle)
        public MeleeDamageAreaShapes DamageAreaShape = MeleeDamageAreaShapes.Rectangle;
        /// the size of the damage area
        public Vector3 AreaSize = new Vector3(1, 1);
        /// the offset to apply to the damage area (from the weapon's attachment position
        public Vector3 AreaOffset = new Vector3(1, 0);

        [Header("Damage Area Timing")]
        /// the initial delay to apply before triggering the damage area
        private float initialDelay = 0;

        [Header("普通攻击减少技能冷却")]
        private HTReduceCoolDown reduceCoolDown;

        [Header("动画几秒的时候攻击有效 百分比")]
        public float attackingDelay = 0.4f;
        public float InitialDelay
        {
            get
            {
                if (HTAttackSpeed == null)
                {
                    HTAttackSpeed.GetComponent<HTAttackSpeed>();
                }

                initialDelay = HTAttackSpeed.AnimSpeedPercent() /** HTAttackSpeed.BasicAttackSpeed*/ * attackingDelay;
                return initialDelay;
            }
            set
            {
                initialDelay = value;
            }
        }


        /// the duration during which the damage area is active
        public float activeDuration = 1;



        public float ActiveDuration
        {
            get
            {
                if (HTAttackSpeed == null)
                {
                    HTAttackSpeed = GetComponent<HTAttackSpeed>();
                }
                return activeDuration * HTAttackSpeed.AnimSpeedPercent();
            }
            set
            {
                activeDuration = value;
            }
        }

        [Header("Damage Caused")]
        /// the layers that will be damaged by this object
        public LayerMask TargetLayerMask;

        /// the kind of knockback to apply
        public DamageOnTouch.KnockbackStyles Knockback;
        /// The force to apply to the object that gets damaged
        public Vector2 KnockbackForce = new Vector2(10, 2);
        /// The duration of the invincibility frames after the hit (in seconds)

        public float InvincibilityDuration = 0.1f;
        /// if this is true, the owner can be damaged by its own weapon's damage area (usually false)
        public bool CanDamageOwner = false;
        private string[] CharacterName;

        protected Collider _damageAreaCollider;
        protected Collider2D _damageAreaCollider2D;
        protected bool _attackInProgress = false;
        protected Color _gizmosColor;
        protected Vector3 _gizmoSize;
        protected CircleCollider2D _circleCollider2D;
        protected BoxCollider2D _boxCollider2D;
        protected BoxCollider _boxCollider;
        protected SphereCollider _sphereCollider;
        protected Vector3 _gizmoOffset;
        protected DamageOnTouch _damageOnTouch;
        protected GameObject _damageArea;

        [Header("攻击时是否有移动(当没有移动时)")]
        public bool DashInAttack = false;
        public float TotalTime = 0;
        public float TotalDistination = 0;
        private Vector3 newDistination;
        private Character character;
        private Vector3 _newPosition;
        private bool dashing = false;
        private float dashTimer = 0;
        private float _currentTotalTime = 0;
        [SerializeField]
        private int _multipleDistination = 6;

        [Header("近战打击反馈")]
        public MMSimpleObjectPooler damageEffectObject;
        public MMFeedbacks _meleeWeaponFeedbakcs;

        [Header("角色属性配置表")]
        public CharacterConfig characterConfigure;


        private WeaponCrit _crit;
        private MMAutoFocus focus;
        [Header("剑士被动技能名字")]
        public string SwordManPassiveSkill;

        [Header("近战武器打到物体上")]
        public AudioClip swordClip;//近战打到物体上

        [Header("前方是否是的阻止前进的物体")]
        public LayerMask _targetLayerMask;

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();


            _crit = GetComponent<WeaponCrit>();

            if (_damageArea == null)
            {
                CreateDamageArea();
                DisableDamageArea();
            }
            if (Owner != null)
            {
                _damageOnTouch.Owner = Owner.gameObject;
            }

            if (damageEffectObject == null)
            {
                damageEffectObject = GetComponent<MMSimpleObjectPooler>();
           }

            reduceCoolDown = GetComponent<HTReduceCoolDown>();
            focus = FindObjectOfType<MMAutoFocus>();
            //passiveSkill = GetComponent<SwordmanPassiveSkill>();

            character = CharacterManager.Instance.GetCharacter("Player1");

            _meleeWeaponFeedbakcs?.Initialization();

        }

        /// <summary>
        /// Creates the damage area.
        /// </summary>
        protected virtual void CreateDamageArea()
        {
            _damageArea = new GameObject();
            _damageArea.name = this.name + "DamageArea";
            _damageArea.transform.position = this.transform.position;
            _damageArea.transform.rotation = this.transform.rotation;
            _damageArea.transform.SetParent(this.transform);
            _damageArea.layer = this.gameObject.layer;

            if (DamageAreaShape == MeleeDamageAreaShapes.Rectangle)
            {
                _boxCollider2D = _damageArea.AddComponent<BoxCollider2D>();
                _boxCollider2D.offset = AreaOffset;
                _boxCollider2D.size = AreaSize;
                _damageAreaCollider2D = _boxCollider2D;
                _damageAreaCollider2D.isTrigger = true;
            }
            if (DamageAreaShape == MeleeDamageAreaShapes.Circle)
            {
                _circleCollider2D = _damageArea.AddComponent<CircleCollider2D>();
                _circleCollider2D.transform.position = this.transform.position + this.transform.rotation * AreaOffset;
                _circleCollider2D.radius = AreaSize.x / 2;
                _damageAreaCollider2D = _circleCollider2D;
                _damageAreaCollider2D.isTrigger = true;
            }

            if ((DamageAreaShape == MeleeDamageAreaShapes.Rectangle) || (DamageAreaShape == MeleeDamageAreaShapes.Circle))
            {
                Rigidbody2D rigidBody = _damageArea.AddComponent<Rigidbody2D>();
                rigidBody.isKinematic = true;
                rigidBody.sleepMode = RigidbodySleepMode2D.NeverSleep;
            }

            if (DamageAreaShape == MeleeDamageAreaShapes.Box)
            {
                _boxCollider = _damageArea.AddComponent<BoxCollider>();
                _boxCollider.center = AreaOffset;
                _boxCollider.size = AreaSize;
                _damageAreaCollider = _boxCollider;
                _damageAreaCollider.isTrigger = true;
            }
            if (DamageAreaShape == MeleeDamageAreaShapes.Sphere)
            {
                _sphereCollider = _damageArea.AddComponent<SphereCollider>();
                _sphereCollider.transform.position = this.transform.position + this.transform.rotation * AreaOffset;
                _sphereCollider.radius = AreaSize.x / 2;
                _damageAreaCollider = _sphereCollider;
                _damageAreaCollider.isTrigger = true;
            }

            if ((DamageAreaShape == MeleeDamageAreaShapes.Box) || (DamageAreaShape == MeleeDamageAreaShapes.Sphere))
            {
                Rigidbody rigidBody = _damageArea.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;
            }

            _damageOnTouch = _damageArea.AddComponent<DamageOnTouch>();

            _damageOnTouch.hitDirection = this.transform;

            _damageOnTouch._crit = this._crit;

            _damageOnTouch.SetGizmoSize(AreaSize);
            _damageOnTouch.SetGizmoOffset(AreaOffset);
            _damageOnTouch.TargetLayerMask = TargetLayerMask;
            _damageOnTouch.characterconfigure = characterConfigure;
            _damageOnTouch.DamageCausedKnockbackType = Knockback;
            _damageOnTouch.DamageCausedKnockbackForce = KnockbackForce;
            _damageOnTouch.InvincibilityDuration = InvincibilityDuration;
            _damageOnTouch.MeleeWeaponFeedBack = _meleeWeaponFeedbakcs;
            if (!CanDamageOwner)
            {
                _damageOnTouch.IgnoreGameObject(Owner.gameObject);
            }
        }

        /// <summary>
        /// When the weapon is used, we trigger our attack routine
        /// 近战角色攻击
        /// </summary>
        public override void WeaponUse()
        {
            base.WeaponUse();
            StartCoroutine(MeleeWeaponAttack());

            CharacterName = focus.FocusTargets[0].name.Split('(');
            focus.FocusTargets[0].name = CharacterName[0];
            if (focus.FocusTargets[0].name == SwordManPassiveSkill)
            {
                SwordmanPassiveSkill._instance.PassiveSkillRelease();
            }
            if (focus.FocusTargets[0].name == "SwordLevel1" || focus.FocusTargets[0].name == "Beginner"  ||
                 focus.FocusTargets[0].name == "GreatSwordLevel2" 
                )
            {

                if (focus.FocusTargets[0].GetComponent<Character>()._animator.GetBool("Combo1") == true)
                {

                    SwordSlashController swordSlashController =
                        TrailAchieve._instance.beginnerSlashCombo1.gameObject.GetComponent<SwordSlashController>();

                    if (swordSlashController != null)
                    {
                        swordSlashController.SwordSlashLifeTimeControl();
                    }

                    TrailAchieve._instance.beginnerSlashCombo1.Play();
                }

                if (focus.FocusTargets[0].GetComponent<Character>()._animator.GetBool("Combo2") == true)
                {
                    SwordSlashController swordSlashController =
                         TrailAchieve._instance.beginnerSlashCombo2.gameObject.GetComponent<SwordSlashController>();

                    if (swordSlashController != null)
                    {
                        swordSlashController.SwordSlashLifeTimeControl();
                    }
                    TrailAchieve._instance.beginnerSlashCombo2.Play();
                }

                if (focus.FocusTargets[0].GetComponent<Character>()._animator.GetBool("Combo3") == true)
                {
                    SwordSlashController swordSlashController =
                        TrailAchieve._instance.beginnerPunchCombo3.gameObject.GetComponent<SwordSlashController>();

                    if (swordSlashController != null)
                    {
                        swordSlashController.SwordSlashLifeTimeControl();
                    }
                    TrailAchieve._instance.beginnerPunchCombo3.Play();
                }

            }
    

            ///Great sowrd men passive skill
            if (focus.FocusTargets[0].name == "GreatSwordLevel2")
            {
                if (EventTypeManager.ContainHTEventType(HTEventType.GreatSwordLevelPassiveSkill))
                {
                    EventTypeManager.Broadcast(HTEventType.GreatSwordLevelPassiveSkill);
                }
            }

        }
        /// <summary>
        /// Triggers an attack, turning the damage area on and then off
        /// </summary>
        /// <returns>The weapon attack.</returns>
        protected virtual IEnumerator MeleeWeaponAttack()
        {
            if (_attackInProgress) { yield break; }

            _attackInProgress = true;
            //  yield return new WaitForSeconds(DelaySudden);
            SuddenForceInSelf();
            DashStart();
            yield return new WaitForSeconds(InitialDelay);
            EnableDamageArea();
            TriggerGodController();
            yield return new WaitForSeconds(ActiveDuration);
            DisableDamageArea();
            _attackInProgress = false;
            DashForwardStop();
           // _comboWeapon._EnterFight = false;

        }

        /// <summary>
        /// 2020 2 9 When attack enemy sudden distance
        /// </summary>
        protected virtual void SuddenForceInSelf()
        {
            // _controller.Impact(transform.forward, SuddernForce);
        }

        /// <summary>
        /// Enables the damage area.
        /// </summary>
        protected virtual void EnableDamageArea()
        {
            if (_damageAreaCollider2D != null)
            {
                _damageAreaCollider2D.enabled = true;

            }

            if (_damageAreaCollider != null)
            {
                _damageAreaCollider.enabled = true;

                if (damageEffectObject != null)
                {
                    _damageOnTouch.hitEffect = damageEffectObject.GetPooledGameObject();

                  
                }

                if (swordClip != null)
                {
                    _damageOnTouch.swordClip = this.swordClip;
                }
            }

            if (reduceCoolDown != null)
            {
                if (reduceCoolDown.skillRelease == null)
                {
                    reduceCoolDown.skillRelease = Owner.gameObject.GetComponent<SkillReleaseTrigger>();
                }
                if (reduceCoolDown.skillRelease == null) return;
                reduceCoolDown.ReduceSkillCoolDown();
            }
        }


        /// <summary>
        /// Disables the damage area.
        /// </summary>
        protected virtual void DisableDamageArea()
        {
            if (_damageAreaCollider2D != null)
            {
                _damageAreaCollider2D.enabled = false;
            }
            if (_damageAreaCollider != null)
            {
                _damageAreaCollider.enabled = false;
            }
        }


        /// <summary>
        /// 冲刺开始
        /// </summary>
        private void DashStart()
        { 
            RaycastHit hit = RayCastCheckUtility.Instance.BoxCast(transform.position, 
                new Vector3(AreaSize.x,AreaSize.x,AreaSize.x)/2,_aimableWeapon.CurrentAim.normalized,200);

            if(hit.collider!=null&& MMLayers.LayerInLayerMask(hit.collider.gameObject.layer, _targetLayerMask))           
            {
                if(hit.distance<=AreaSize.z-10)
                {
                    return;
                }
            }

            if (DashInAttack)
            {
                dashing = true;
                dashTimer = 0;

                float distination;
                if(!_comboWeapon._EnterFight)
                {
                    _currentTotalTime = TotalTime;
                    distination = TotalDistination;
                }
                else
                {
                    _currentTotalTime = TotalTime * _multipleDistination;
                    distination = TotalDistination * _multipleDistination;
                    
                    if(hit.collider!=null&&hit.distance<distination+AreaSize.z)
                    {
                        _currentTotalTime = TotalTime;
                        distination = hit.distance - AreaSize.z+12;
                        distination = Mathf.Clamp(distination, 0, Mathf.Infinity);
                    }
                }

                newDistination = character.transform.position + (_aimableWeapon.CurrentAim.normalized* distination);
            }
        }

        /// <summary>
        /// 处理冲刺
        /// </summary>
        private void ProcessDash()
        {
            if (_characterMovement == null) return;

            if (DashInAttack && dashing)
            {
                if (dashTimer < _currentTotalTime)
                {
                    _newPosition = Vector3.Lerp(character.transform.position, newDistination, dashTimer / _currentTotalTime);
                    _controller.MovePosition(_newPosition);
                    dashTimer += Time.deltaTime;
                }
                else
                {
                    DashForwardStop();
                }
            }
        }

        /// <summary>
        /// 冲刺停止
        /// </summary>
        private void DashForwardStop()
        {
            dashing = false;
        }


        private void FixedUpdate()
        {
            ProcessDash();

            Debug.DrawRay(this.transform.position, _aimableWeapon.CurrentAim.normalized*5, Color.blue);
        }

        /// <summary>
        /// When selected, we draw a bunch of gizmos
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                DrawGizmos();
            }

        }

        protected virtual void DrawGizmos()
        {
            if (DamageAreaShape == MeleeDamageAreaShapes.Box)
            {
                Gizmos.DrawWireCube(this.transform.position + AreaOffset, AreaSize);
            }

            if (DamageAreaShape == MeleeDamageAreaShapes.Circle)
            {
                Gizmos.DrawWireSphere(this.transform.position + AreaOffset, AreaSize.x / 2);
            }

            if (DamageAreaShape == MeleeDamageAreaShapes.Rectangle)
            {
                MMDebug.DrawGizmoRectangle(this.transform.position + AreaOffset, AreaSize, Color.red);
            }

            if (DamageAreaShape == MeleeDamageAreaShapes.Sphere)
            {
                Gizmos.DrawWireSphere(this.transform.position + AreaOffset, AreaSize.x / 2);
            }
                      
        }


        /// <summary>
        /// 触发神明控制
        /// </summary>
        void TriggerGodController()
        {
            int currentWeaponIndex = _comboWeapon.GetCurrentWeaponIndex();

            if (EventTypeManager.ContainHTEventType(HTEventType.GodGolemController))
            {
                EventTypeManager.Broadcast<int>(HTEventType.GodGolemController, currentWeaponIndex);
            }
        }

    }
}
