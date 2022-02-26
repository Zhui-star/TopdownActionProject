using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using System;
using MoreMountains.Feedbacks;
using HTLibrary.Application;
using HTLibrary.Framework;
using MoreMountains.FeedbacksForThirdParty;
using HTLibrary.Utility;
namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this component to an object and it will cause damage to objects that collide with it. 
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/Damage/DamageOnTouch")]
    public class DamageOnTouch : MonoBehaviour
    {
        /// the possible ways to add knockback : noKnockback, which won't do nothing, set force, or add force
        public enum KnockbackStyles { NoKnockback, AddForce }
        /// the possible knockback directions
        public enum KnockbackDirections { BasedOnOwnerPosition, BasedOnSpeed }

        [Header("Targets")]
        [MMInformation("This component will make your object cause damage to objects that collide with it. Here you can define what layers will be affected by the damage (for a standard enemy, choose Player), how much damage to give, and how much force should be applied to the object that gets the damage on hit. You can also specify how long the post-hit invincibility should last (in seconds).", MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        // the layers that will be damaged by this object
        public LayerMask TargetLayerMask;
        /// set this to true to have your object teleport to the impact point on death. Useful for fast moving stuff like projectiles.
        public bool PerfectImpact = false;

        [HideInInspector]
        //Additive damge value generayly be used skill
        public int additiveDamage;

        [Header("Damage Caused")]
        /// The amount of health to remove from the player's health
        private int damageCause;
        public int DamageCaused
        {
            get
            {
                if (characterconfigure == null)
                {
                    return damageCause;
                }

                return additiveDamage + (int)characterconfigure.additiveAttack + (int)characterconfigure.characterAttack;
            }
            set
            {
                damageCause = value;
            }
        }
        /// the type of knockback to apply when causing damage
        public KnockbackStyles DamageCausedKnockbackType = KnockbackStyles.AddForce;
        /// The direction to apply the knockback 
        public KnockbackDirections DamageCausedKnockbackDirection;
        /// The force to apply to the object that gets damaged
        public Vector3 DamageCausedKnockbackForce = new Vector3(10, 10, 0);
        /// The duration of the invincibility frames after the hit (in seconds)
        public float InvincibilityDuration = 0.5f;

        [Header("Damage Taken")]
        [MMInformation("After having applied the damage to whatever it collided with, you can have this object hurt itself. A bullet will explode after hitting a wall for example. Here you can define how much damage it'll take every time it hits something, or only when hitting something that's damageable, or non damageable. Note that this object will need a Health component too for this to be useful.", MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        /// The amount of damage taken every time, whether what we collide with is damageable or not
        public int DamageTakenEveryTime = 0;
        /// The amount of damage taken when colliding with a damageable object
        public int DamageTakenDamageable = 0;
        /// The amount of damage taken when colliding with something that is not damageable
        public int DamageTakenNonDamageable = 0;
        /// the type of knockback to apply when taking damage
        public KnockbackStyles DamageTakenKnockbackType = KnockbackStyles.NoKnockback;
        /// The direction to apply the knockback 
        public KnockbackDirections DamagedTakenKnockbackDirection;
        /// The force to apply to the object that gets damaged
        public Vector3 DamageTakenKnockbackForce = Vector3.zero;
        /// The duration of the invincibility frames after the hit (in seconds)
        public float DamageTakenInvincibilityDuration = 0.5f;


        [Header("Feedbacks")]
        public MMFeedbacks HitDamageableFeedback;
        public MMFeedbacks HitNonDamageableFeedback;
        public MMFeedbacks TriggerEnterFeedBack;
        public string FeedBackObjectName;
        [MMInformation("Is continue damage target",MMInformationAttribute.InformationType.Info,false)]
        public bool collidingFeedBack = false; //Is coninue damage target
        public MMFeedbacks MeleeWeaponFeedBack { get; set; }

        [MMReadOnly]
        /// the owner of the DamageOnTouch zone
        public GameObject Owner;

        // storage		
        protected Vector3 _lastPosition, _velocity, _knockbackForce;
        protected float _startTime = 0f;
        protected Health _colliderHealth;
        protected TopDownController _topDownController;
        protected TopDownController _colliderTopDownController;
        protected Rigidbody _colliderRigidBody;
        protected Health _health;
        protected List<GameObject> _ignoredGameObjects;
        protected Vector3 _collisionPoint;
        protected Vector3 _knockbackForceApplied;
        protected CircleCollider2D _circleCollider2D;
        protected BoxCollider2D _boxCollider2D;
        protected SphereCollider _sphereCollider;
        protected BoxCollider _boxCollider;
        protected Color _gizmosColor;
        protected Vector3 _gizmoSize;
        protected Vector3 _gizmoOffset;
        protected Transform _gizmoTransform;
        private MMAutoFocus _focus;
        private string[] CharacterName;
        public ParticleSystem elementalistParticle;

        [HideInInspector]
        public GameObject hitEffect;
        [HideInInspector]
        public Transform hitDirection;

        [Header("character properties table")]
        public CharacterConfig characterconfigure;
        public WeaponCrit _crit;
        [Header("Archer passive skill name in object pool")]
        public string ArcherPassiveName;


        [Header("Is can throught target (ponertrate)")]
        public bool canDamageThrough = false;

        [HideInInspector]
        public AudioClip swordClip;

        bool playingClip = false;

        private PoolManagerV2 _poolManager;
        LevelUnitManager _levelUnitManager;

        [Header("Trap damage percent")]
        [Range(0, 1)]
        public float _trapDamageStep = 0.05f;
        //ShieldGuardPassiveSkill guardPassiveSkill;

        private Projectile _projectile;
        private WeaponAccumlateResult _weaponAccumlateRes;
        public bool Accumlate { get; set; }
        private float _accumlateMultiple;

        [Header("Is keep detect If yes it will coninue make damage in sometime")]
        public bool _keepDetect=true;
        /// <summary>
        /// Initialization
        /// </summary>
        protected virtual void Awake()
        {
            _ignoredGameObjects = new List<GameObject>();
            _health = GetComponent<Health>();
            _topDownController = GetComponent<TopDownController>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _boxCollider = GetComponent<BoxCollider>();
            _sphereCollider = GetComponent<SphereCollider>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _focus = FindObjectOfType<MMAutoFocus>();
            _gizmosColor = Color.red;
            _gizmosColor.a = 0.25f;

            _poolManager = PoolManagerV2.Instance;

            _levelUnitManager = LevelUnitManager.Instance;

            InitializeFeedbacks();

            _projectile = GetComponent<Projectile>();


        }

        protected virtual void Start()
        {
            InitialTrapDamage();
        }

        public virtual void InitializeFeedbacks()
        {
            HitDamageableFeedback?.Initialization(this.gameObject);
            HitNonDamageableFeedback?.Initialization(this.gameObject);
            TriggerEnterFeedBack?.Initialization(this.gameObject);
        }

        public virtual void SetGizmoSize(Vector3 newGizmoSize)
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _boxCollider = GetComponent<BoxCollider>();
            _sphereCollider = GetComponent<SphereCollider>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _gizmoSize = newGizmoSize;
        }

        public virtual void SetGizmoOffset(Vector3 newOffset)
        {
            _gizmoOffset = newOffset;
        }

        /// <summary>
        /// OnEnable we set the start time to the current timestamp
        /// </summary>
        protected virtual void OnEnable()
        {
            _startTime = Time.time;
        }

        /// <summary>
        /// During last update, we store the position and velocity of the object
        /// </summary>
        protected virtual void Update()
        {
            ComputeVelocity();
            CharacterName = _focus.FocusTargets[0].name.Split('(');
            _focus.FocusTargets[0].name = CharacterName[0];
        }

        /// <summary>
        /// Adds the gameobject set in parameters to the ignore list
        /// </summary>
        /// <param name="newIgnoredGameObject">New ignored game object.</param>
        public virtual void IgnoreGameObject(GameObject newIgnoredGameObject)
        {
            _ignoredGameObjects.Add(newIgnoredGameObject);
        }

        /// <summary>
        /// Removes the object set in parameters from the ignore list
        /// </summary>
        /// <param name="ignoredGameObject">Ignored game object.</param>
        public virtual void StopIgnoringObject(GameObject ignoredGameObject)
        {
            _ignoredGameObjects.Remove(ignoredGameObject);
        }

        /// <summary>
        /// Clears the ignore list.
        /// </summary>
        public virtual void ClearIgnoreList()
        {
            _ignoredGameObjects.Clear();
        }

        /// <summary>
        /// Computes the velocity based on the object's last position
        /// </summary>
        protected virtual void ComputeVelocity()
        {
            _velocity = (_lastPosition - (Vector3)transform.position) / Time.deltaTime;
            _lastPosition = transform.position;
        }

        /// <summary>
        /// When a collision with the player is triggered, we give damage to the player and knock it back
        /// </summary>
        /// <param name="collider">what's colliding with the object.</param>
        public virtual void OnTriggerStay2D(Collider2D collider)
        {
            Colliding(collider.gameObject);
        }

        /// <summary>
        /// On trigger enter 2D, we call our colliding endpoint
        /// </summary>
        /// <param name="collider"></param>S
        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            Colliding(collider.gameObject);
        }

        /// <summary>
        /// On trigger stay, we call our colliding endpoint
        /// </summary>
        /// <param name="collider"></param>
        public virtual void OnTriggerStay(Collider collider)
        {
            if(_keepDetect)
            {
              Colliding(collider.gameObject);
            }
        }

        /// <summary>
        /// On trigger enter, we call our colliding endpoint
        /// </summary>
        /// <param name="collider"></param>
        public virtual void OnTriggerEnter(Collider collider)
        {

            if (MMLayers.LayerInLayerMask(collider.gameObject.layer, TargetLayerMask) && swordClip != null)
            {
                SoundManager.Instance.PlaySound(swordClip, transform.position, false);
            }

            if (MMLayers.LayerInLayerMask(collider.gameObject.layer, TargetLayerMask))
            {
                TriggerEnterFeedBack?.PlayFeedbacks();

                if (!string.IsNullOrEmpty(FeedBackObjectName))
                {
                    GameObject feedBackObj = _poolManager.GetInst(FeedBackObjectName);
                    feedBackObj.transform.SetPositionAndRotation(collider.ClosestPointOnBounds(transform.position), collider.transform.rotation);
                    if(collidingFeedBack) // Is coninue damage target
                    {
                        hitEffect = feedBackObj;
                        hitDirection = collider.transform;
                    }

                    if (_projectile != null)
                    {
                        if (_projectile.Accumlate)
                        {
                            switch (_projectile._weaponAccumlateReuslt)
                            {
                                case WeaponAccumlateResult.MultipleScale:
                                    feedBackObj.transform.localScale *= _projectile._accumalateMultiple;
                                    feedBackObj.GetComponent<DamageOnTouch>().Accumlate = true;
                                    feedBackObj.GetComponent<DamageOnTouch>()._weaponAccumlateRes = _projectile._weaponAccumlateReuslt;
                                    feedBackObj.GetComponent<DamageOnTouch>()._accumlateMultiple = _projectile._accumalateMultiple;
                                    break;
                            }
                        }
                    }
                }

                MeleeWeaponFeedBack?.PlayFeedbacks();
            }

            Colliding(collider.gameObject);
        }



        /// <summary>
        /// When colliding, we apply damage
        /// </summary>
        /// <param name="collider"></param>
        protected virtual void Colliding(GameObject collider)
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            // if the object we're colliding with is part of our ignore list, we do nothing and exit
            if (_ignoredGameObjects.Contains(collider))
            {
                return;
            }

            // if what we're colliding with isn't part of the target layers, we do nothing and exit
            if (!MMLayers.LayerInLayerMask(collider.layer, TargetLayerMask))
            {

                return;
            }

            // if we're on our first frame, we don't apply damage
            if (Time.time == 0f)
            {
                return;
            }

            _collisionPoint = this.transform.position;
            _colliderHealth = collider.gameObject.MMGetComponentNoAlloc<Health>();

            ElementalistPassiveSkillRelease(collider, collider.transform.position);

            if (hitEffect != null)
            {
                if (collider.tag != Tags.Player || !_colliderHealth.Invulnerable)
                {
                    Collider tempCollider = collider.GetComponent<Collider>();
                    if (tempCollider == null)
                    {
                        hitEffect.transform.position = collider.transform.position + Vector3.up;
                    }
                    else
                    {
                        hitEffect.transform.position = tempCollider.ClosestPointOnBounds(this.transform.position);
                    }

                    hitEffect.transform.rotation = hitDirection.rotation;
                    hitEffect.SetActive(true);
                }
            }

            // if what we're colliding with is damageable
            if (_colliderHealth != null)
            {
                if (_colliderHealth.CurrentHealth > 0)
                {
                    OnCollideWithDamageable(_colliderHealth);

                    if (Owner != null)
                    {
                        if(Owner.gameObject.name== "ElementMageLevel2")
                        {
                            if (EventTypeManager.ContainHTEventType(HTEventType.SummonerPassiveSkill))
                            {
                                Vector3 direction =_colliderHealth.transform.position - Owner.transform.position;
                                EventTypeManager.Broadcast<Vector3>(HTEventType.SummonerPassiveSkill,direction);
                            }
                        }                
                    }

                }
            }
            // if what we're colliding with can't be damaged
            else
            {
                OnCollideWithNonDamageable();
            }

        }

        /// <summary>
        /// Describes what happens when colliding with a damageable object
        /// </summary>
        /// <param name="health">Health.</param>
        protected virtual void OnCollideWithDamageable(Health health)
        {

            // if what we're colliding with is a TopDownController, we apply a knockback force
            _colliderTopDownController = health.gameObject.MMGetComponentNoAlloc<TopDownController>();
            _colliderRigidBody = health.gameObject.MMGetComponentNoAlloc<Rigidbody>();

            if ((_colliderTopDownController != null) && (DamageCausedKnockbackForce != Vector3.zero) && (!_colliderHealth.Invulnerable) && (!_colliderHealth.ImmuneToKnockback))
            {
                _knockbackForce.x = DamageCausedKnockbackForce.x;
                _knockbackForce.y = DamageCausedKnockbackForce.y;

                Vector3 tempKncokForce = _knockbackForce;
                if (_colliderTopDownController.weight > 0)
                {
                    if (tempKncokForce.x != 0)
                    {
                        tempKncokForce.x -= _colliderTopDownController.weight;
                        tempKncokForce.x = tempKncokForce.x < 0 ? 0 : tempKncokForce.x;
                    }
                    if (tempKncokForce.y != 0)
                    {
                        tempKncokForce.y -= _colliderTopDownController.weight;
                        tempKncokForce.y = tempKncokForce.y < 0 ? 0 : tempKncokForce.y;
                    }
                    if (tempKncokForce.z != 0)
                    {
                        tempKncokForce.z -= _colliderTopDownController.weight;
                        tempKncokForce.z = tempKncokForce.z < 0 ? 0 : tempKncokForce.z;
                    }


                }

                if (DamageCausedKnockbackDirection == KnockbackDirections.BasedOnSpeed)
                {
                    Vector3 totalVelocity = _colliderTopDownController.Speed + _velocity;
                    _knockbackForce = Vector3.RotateTowards(DamageCausedKnockbackForce, totalVelocity.normalized, 10f, 0f);
                }
                if (DamagedTakenKnockbackDirection == KnockbackDirections.BasedOnOwnerPosition)
                {
                    if (Owner == null) { Owner = this.gameObject; }
                    Vector3 relativePosition = _colliderTopDownController.transform.position - Owner.transform.position;

                    _knockbackForce = Vector3.RotateTowards(DamageCausedKnockbackForce, relativePosition.normalized, 10f, 0f);

                }

                if ((_projectile != null && _projectile.Accumlate == true) || Accumlate)
                {
                    WeaponAccumlateResult rest = _projectile == null ? _weaponAccumlateRes : _projectile._weaponAccumlateReuslt;
                    float tempMultiple = _projectile == null ? _accumlateMultiple : _projectile._accumalateMultiple;

                    switch (rest)
                    {
                        case WeaponAccumlateResult.None:
                            break;
                        case WeaponAccumlateResult.MultipleScale:
                            tempKncokForce *= tempMultiple;
                            break;
                    }


                }

                if (DamageCausedKnockbackType == KnockbackStyles.AddForce)
                {
                    _colliderTopDownController.Impact(_knockbackForce.normalized, tempKncokForce.magnitude);
                }


            }


            HitDamageableFeedback?.PlayFeedbacks(this.transform.position);


            // we apply the damage to the thing we've collided with

            int tempDamageCaused = DamageCaused;

            bool isCrit = false;
            //ArcherPassiveSkill._instance.enabled = false;
            if (_crit != null)
            {
                tempDamageCaused = (int)_crit.GetDamageCrit(DamageCaused);
            }
            if (tempDamageCaused > DamageCaused)
            {
                isCrit = true;
                ArcherPassiveSkillRelease();

            }

            if ((_projectile != null && _projectile.Accumlate == true) || Accumlate)
            {

                WeaponAccumlateResult rest = _projectile == null ? _weaponAccumlateRes : _projectile._weaponAccumlateReuslt;
                float tempMultiple = _projectile == null ? _accumlateMultiple : _projectile._accumalateMultiple;

                switch (rest)
                {
                    case WeaponAccumlateResult.None:
                        break;
                    case WeaponAccumlateResult.MultipleScale:
                        tempDamageCaused *= (int)tempMultiple;
                        break;
                }


            }

            //Call patner retarget
            if (EventTypeManager.ContainHTEventType(HTEventType.PatnerCommend_ResetTarget))
            {

                if (Owner != null && Owner.tag == Tags.Player)
                {
                    EventTypeManager.Broadcast<Transform>(HTEventType.PatnerCommend_ResetTarget, health.gameObject.transform);
                }

            }

            _colliderHealth.Damage(tempDamageCaused, gameObject, InvincibilityDuration, InvincibilityDuration, BerserkerPassiveSkillRelease, isCrit);

            if (canDamageThrough) return;
            if (DamageTakenEveryTime + DamageTakenDamageable > 0)
            {
                SelfDamage(DamageTakenEveryTime + DamageTakenDamageable);
            }

        }

        //Archer passive skill release
        public void ArcherPassiveSkillRelease()
        {
            if (_focus.FocusTargets[0].name == "ArcherLevel1") 
            {
                ArcherPassiveSkill._instance.ArcherPassiveSkillHappen = true;
            }
        }

        //Barserker passive skill (It increase character attackSpeed)
        public void BerserkerPassiveSkillRelease()
        {
            if (_focus.FocusTargets[0].name == "BarserkerLeve2"/*"Archer"*/)
            {
                if (BerserkerPassiveSkillControl._instance.particleSystems[0].isStopped)
                    BerserkerPassiveSkillControl._instance.particleSystems[0].Play();
                else if (BerserkerPassiveSkillControl._instance.particleSystems[1].isStopped)
                    BerserkerPassiveSkillControl._instance.particleSystems[1].Play();
                else if (BerserkerPassiveSkillControl._instance.particleSystems[2].isStopped)
                    BerserkerPassiveSkillControl._instance.particleSystems[2].Play();
                else if (BerserkerPassiveSkillControl._instance.particleSystems[3].isStopped)
                    BerserkerPassiveSkillControl._instance.particleSystems[3].Play();
                else if (BerserkerPassiveSkillControl._instance.particleSystems[4].isStopped)
                    BerserkerPassiveSkillControl._instance.particleSystems[4].Play();
            }
        }

        //Elementalist passive skill
        public void ElementalistPassiveSkillRelease(GameObject gameobject, Vector3 position)
        {
  
            if (_focus.FocusTargets[0].name == "Archer") 
            {
                ElementalistPassiveSkill._instance.ElementalistPassiveSkillCore();
                if (ElementalistPassiveSkill._instance.isRelease == true)
                {
                    elementalistParticle = Instantiate(ElementalistPassiveSKillParticleControl._instance.ElementalistParticleSystem,
                        gameobject.transform) as ParticleSystem;
                    elementalistParticle.Play();
                }
            }
        }

        /// <summary>
        /// Describes what happens when colliding with a non damageable object
        /// </summary>
        protected virtual void OnCollideWithNonDamageable()
        {
            if (DamageTakenEveryTime + DamageTakenNonDamageable > 0)
            {
                SelfDamage(DamageTakenEveryTime + DamageTakenNonDamageable);
            }

            HitNonDamageableFeedback?.PlayFeedbacks(this.transform.position);
        }

        /// <summary>
        /// Applies damage to itself
        /// </summary>
        /// <param name="damage">Damage.</param>
        protected virtual void SelfDamage(int damage)
        {
            if (_health != null)
            {

                _health.Damage(damage, gameObject, 0f, DamageTakenInvincibilityDuration, null);

                if ((_health.CurrentHealth <= 0) && PerfectImpact)
                {
                    this.transform.position = _collisionPoint;
                }
            }

            // if what we're colliding with is a TopDownController, we apply a knockback force
            if (_topDownController != null)
            {
                Vector2 totalVelocity = _colliderTopDownController.Speed + _velocity;
                Vector2 knockbackForce = Vector3.RotateTowards(DamageCausedKnockbackForce, totalVelocity.normalized, 10f, 0f);

                if (DamageTakenKnockbackType == KnockbackStyles.AddForce)
                {
                    _topDownController.AddForce(knockbackForce);
                }
            }
        }

        /// <summary>
        /// draws a cube or sphere around the damage area
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = _gizmosColor;

            if (_boxCollider2D != null)
            {
                if (_boxCollider2D.enabled)
                {
                    MMDebug.DrawGizmoCube(this.transform,
                                            _gizmoOffset,
                                            _boxCollider2D.size,
                                            false);
                }
                else
                {
                    MMDebug.DrawGizmoCube(this.transform,
                                            _gizmoOffset,
                                            _boxCollider2D.size,
                                            true);
                }
            }

            if (_circleCollider2D != null)
            {
                if (_circleCollider2D.enabled)
                {
                    Gizmos.DrawSphere((Vector2)this.transform.position + _circleCollider2D.offset, _circleCollider2D.radius);
                }
                else
                {
                    Gizmos.DrawWireSphere((Vector2)this.transform.position + _circleCollider2D.offset, _circleCollider2D.radius);
                }
            }

            if (_boxCollider != null)
            {
                if (_boxCollider.enabled)
                {
                    MMDebug.DrawGizmoCube(this.transform,
                                            _gizmoOffset,
                                            _boxCollider.size,
                                            false);
                }
                else
                {
                    MMDebug.DrawGizmoCube(this.transform,
                                            _gizmoOffset,
                                            _boxCollider.size,
                                            true);
                }
            }

            if (_sphereCollider != null)
            {
                if (_sphereCollider.enabled)
                {
                    Gizmos.DrawSphere(this.transform.position, _sphereCollider.radius);
                }
                else
                {
                    Gizmos.DrawWireSphere(this.transform.position, _sphereCollider.radius);
                }
            }
        }



        /// <summary>
        /// Initial Trap damage value
        /// </summary>
        void InitialTrapDamage()
        {
            if (this.tag == Tags.Trap)
            {
                int targetDamage = (int)(DamageCaused * _trapDamageStep * _levelUnitManager.ReturnCurrentLevelStrength());

                additiveDamage += targetDamage;
            }

        }

        /// <summary>
        /// Accumlate over e.p magician accumlate normal attack
        /// </summary>
        private void OnDisable()
        {
            Accumlate = false;
        }


    }
}
