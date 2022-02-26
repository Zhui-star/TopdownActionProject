using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using HTLibrary.Application;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.FeedbacksForThirdParty;
using AmazingAssets.AdvancedDissolve;
namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This class manages the health of an object, pilots its potential health bar, handles what happens when it takes damage,
    /// and what happens when it dies.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/Core/Health")]
    public class Health : MonoBehaviour
    {
        [Header("Player dangerous when health below 10% max health")]
        public MMFeedbacks _dangerousHealthFeedbacks;
        [Space]
        [Space]
        /// the model to disable (if set so)
        public GameObject Model;


        private bool _restoreAnim = true;
        public bool RestoreHPAnim
        {
            get
            {
                return _restoreAnim;
            }
            set
            {
                _restoreAnim = value;
            }
        }
        /// the current health of the character
        [MMReadOnly]
        private int currentHealth;

        public int CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                value = value > MaximumHealth ? MaximumHealth : value;
                if (value > currentHealth && currentHealth >= 0 && RestoreHPAnim)
                {
                    HTDamagePoup _damagePoup = null;
                    if (_simpleObjectPooler != null)
                    {
                        GameObject go = _simpleObjectPooler.GetPooledGameObject();

                        _damagePoup = go.GetComponent<HTDamagePoup>();
                        if (_damagePoup != null)
                        {
                            _damagePoup.SetTarget(this.transform);
                        }

                        if (DamagePoupPosition != null)
                        {
                            go.transform.position = DamagePoupPosition.position;

                        }
                        else
                        {
                            go.transform.position = transform.position;
                        }

                        go.SetActive(true);
                    }

                    if (_damagePoup != null)
                    {
                        _damagePoup.SetHealDamageColor((value - currentHealth).ToString());
                    }

                }
                currentHealth = value;

                if (this.gameObject.tag == Tags.Player)
                {
                    if (currentHealth <= MaximumHealth * 0.3f) //dangerous health feedback e.p blood screen flash
                    {
                        if (_dangerousHealthFeedbacks != null)
                        {
                            if (!_dangerousHealthFeedbacks.IsPlaying)
                            {
                                _dangerousHealthFeedbacks?.PlayFeedbacks();
                            }
                        }


                    }
                    else
                    {
                        if (_dangerousHealthFeedbacks != null)
                        {
                            if (_dangerousHealthFeedbacks.IsPlaying)
                            {
                                _dangerousHealthFeedbacks?.StopFeedbacks();
                            }
                        }


                    }
                }

                UpdateHealthBar(RestoreHPAnim);//Update health bar ui
            }
        }
        /// If this is true, this object can't take damage
        [MMReadOnly]
        public bool Invulnerable = false;

        [Header("Health")]
        [MMInformation("Add this component to an object and it'll have health, will be able to get damaged and potentially die.",
            MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        /// the initial amount of health of the object
        public int InitialHealth = 10;

        [MMInformation("additive health", MMInformationAttribute.InformationType.Info, false)]
        public int additiveHealth;

        private int maximumHealth;
        /// the maximum amount of health of the object
        public int MaximumHealth
        {
            get
            {
                if (characterConfigure == null)
                {
                    return maximumHealth;
                }

                return (int)((characterConfigure.characterHP + 
                characterConfigure.additiveHP + additiveHealth) * _healthPercent);
            }

            set
            {
                maximumHealth = value;
            }
        }

        [Header("Damage")]
        [MMInformation("Here you can specify an effect and a sound FX to instantiate when the object gets damaged, " +
            "and also how long the object should flicker when hit (only works for sprites).", 
            MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        /// whether or not this object is immune to damage knockback
        public bool ImmuneToKnockback = false;
        /// the feedback to play when getting damage
        public MMFeedbacks DamageMMFeedbacks;
        public string _damageSpawnObject;
        public Vector3 _damageSpawnObjectOffset;
        GameObject damageSpawnObject;

        [Header("Death")]
        [MMInformation("Here you can set an effect to instantiate when the object dies, a force to apply to it (topdown controller required)," +
            " how many points to add to the game score, if the device should vibrate (only works on iOS and Android), " +
            "and where the character should respawn (for non-player characters only).",
            MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        public bool DestroyOnDeath = true;
        /// the time (in seconds) before the character is destroyed or disabled
        public float DelayBeforeDestruction = 0f;
        /// the points the player gets when the object's health reaches zero
        public int PointsWhenDestroyed;
        /// if this is set to false, the character will respawn at the location of its death, otherwise it'll be moved to its initial position (when the scene started)
        public bool RespawnAtInitialLocation = false;
        /// if this is true, the controller will be disabled on death
        public bool DisableControllerOnDeath = true;
        /// if this is true, the model will be disabled instantly on death (if a model has been set)
        public bool DisableModelOnDeath = true;
        /// if this is true, collisions will be turned off when the character dies
        public bool DisableCollisionsOnDeath = true;

        public bool DisableAI = true;

        public bool DisableSkillRelese = true;
        [MMInformation("Use to dissolve daed body.", MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        public bool _IsDeadDissolve = false;
        private bool _IsDissolving = false;

        private float _dissolveSpeed;
        float _dissolveTimer;
        public Material _dissolveMaterial;

        [SerializeField]
        private float _destroyDissolvePercent;

        [Space]

        /// the feedback to play when dying
        public MMFeedbacks DeathMMFeedbacks;

        // hit delegate
        public delegate void OnHitDelegate();
        public OnHitDelegate OnHit;

        // respawn delegate
        public delegate void OnReviveDelegate();
        public OnReviveDelegate OnRevive;

        // death delegate
        public delegate void OnDeathDelegate();
        public OnDeathDelegate OnDeath;

        protected Vector3 _initialPosition;
        [Space]
        [MMInformation("Model Render", MMInformationAttribute.InformationType.Info, false)]
        public Renderer _renderer;
        [Space]
        [HideInInspector]
        public Character _character;
        protected TopDownController _controller;
        protected MMHealthBar _healthBar;
        protected Collider2D _collider2D;
        protected Collider _collider3D;
        protected CharacterController _characterController;
        protected bool _initialized = false;
        protected Color _initialColor;
        protected AutoRespawn _autoRespawn;
        protected Animator _animator;
        protected AIBrain aibrain;
        protected CharacterSkill1 skillInput1;
        protected CharacterSkill2 skillInput2;
        protected CharacterSelection _characterSelection;


        [MMFInformation("additive defence", MMFInformationAttribute.InformationType.Info, false)]
        public int additiveDefence;
        //defence value
        private int defence;
        public int Defence
        {
            get
            {
                if (characterConfigure == null)
                {
                    return defence;
                }

                return (int)((characterConfigure.characterDefence + characterConfigure.additiveDefence + 
                additiveDefence) * _defencePercent);
            }
            set
            {
                defence = value;
            }
        }
        int _originalDefence;

        //dodge object
        private Dodge dodge { get; set; }

        public HTHealthBarUI _healthBarUI;
        public HTHealthBarUI _bosshealthBarUI;
        public Transform initialHPBarTransform;
        private MMAutoFocus focus;
        private Transform bossUIposTf;


        [Header("character properties table")]
        public CharacterConfig characterConfigure;

        [Header("HUD font game object")]
        public MMSimpleObjectPooler _simpleObjectPooler;
        public Transform DamagePoupPosition;

        [Header("Shielder game object name")]
        public string ShieldGuardPassiveSkillName;

        [Header("Blad Master game object name")]
        public string BladerMasterName;

        [Header("Property percent")]
        [MoreMountains.Tools.MMInformation("Inherite player properties", MMInformationAttribute.InformationType.Info, false)]
        public float _defencePercent = 1;
        public float _healthPercent = 1;

        private void Awake()
        {
            _dangerousHealthFeedbacks?.Initialization();
        }
        void InitialCurrentHP()
        {
            if (characterConfigure != null)
            {
                if (transform.tag == Tags.Player && PlayerPrefs.HasKey(Consts.GameCurrentHP + SaveManager.Instance.LoadGameID))
                {
                    RestoreHPAnim = false;

                    if(SaveManager.Instance.LoadGameID==0)//Test mode
                    {
                        CurrentHealth += MaximumHealth;
                        return;
                    }

                    CurrentHealth += PlayerPrefs.GetInt(Consts.GameCurrentHP + SaveManager.Instance.LoadGameID);
                    if(CharacterSelection.Instance.FrontCharacterMaxHealth!=0)
                    {
                        int differentHealth=MaximumHealth-CharacterSelection.Instance.FrontCharacterMaxHealth;
                         CurrentHealth+=differentHealth;//Add different health value front character and current character
                         CharacterSelection.Instance.FrontCharacterMaxHealth=0;

                        Debugs.LogInformation("Different health from front character "+differentHealth , Color.green);
                    }

                    Debugs.LogInformation("Initial Current Health:" + CurrentHealth + " MaxHealth:" + MaximumHealth,Color.white);
                }
                else if (transform.tag != Tags.Patner)
                {
                    RestoreHPAnim = false;
                    CurrentHealth += MaximumHealth;
                }
            }
        }

        /// <summary>
        /// On Start, we initialize our health
        /// </summary>
        protected virtual void Start()
        {
            InitialCharacter();
            Initialization();
        }

        protected virtual void OnEnable()
        {
            InitialNonCharacterTableHp();
        }

        /// <summary>
        /// Initial character component 
        /// </summary>
        void InitialCharacter()
        {
            _character = GetComponent<Character>();

            if (_character&& _character.IsBoss)
            {
                Transform GMPanelGo = UIManager.Instance.PeekPanel().transform;
                bossUIposTf = GMPanelGo.transform.Find("BossHPPosition").transform;
            }
        }

        /// <summary>
        /// Grabs useful components, enables damage and gets the inital color
        /// </summary>
        protected virtual void Initialization()
        {
          
            dodge = GetComponent<Dodge>();
            focus = FindObjectOfType<MMAutoFocus>();

            _characterSelection = CharacterSelection.Instance;

            if (_healthBarUI != null && _character&&!_character.IsBoss)
            {
                _healthBarUI.isBoss = _character.IsBoss;
                GameObject go = Instantiate(_healthBarUI.gameObject, initialHPBarTransform);
                go.transform.SetLocalPosXYZ(0, 0, 0);
                _healthBarUI = go.GetComponent<HTHealthBarUI>();
            }
            else if (_bosshealthBarUI != null && bossUIposTf != null && _character&& _character.IsBoss)
            {
                _bosshealthBarUI.isBoss = _character.IsBoss;
                _bosshealthBarUI.characterGo = gameObject;
                GameObject go = Instantiate(_bosshealthBarUI.gameObject, bossUIposTf);
                go.transform.SetLocalPosXYZ(0, 0, 0);
                _bosshealthBarUI = go.GetComponent<HTHealthBarUI>();
                if (characterConfigure.characterName != null)
                {
                    _bosshealthBarUI.SetTheNameOfCharacter(characterConfigure.characterName);
                }
            }

            if (Model != null)
            {
                Model.SetActive(true);
            }

            if (_renderer == null && gameObject.MMGetComponentNoAlloc<Renderer>() != null)
            {
                _renderer = GetComponent<Renderer>();

            }

            if (_renderer != null)
            {
                if (_renderer.material.HasProperty("_Color"))
                {
                    _initialColor = _renderer.material.color;
                }
            }

            // we grab our animator
            if (_character != null)
            {
                if (_character.CharacterAnimator != null)
                {
                    _animator = _character.CharacterAnimator;
                }
                else
                {
                    _animator = GetComponent<Animator>();
                }
            }
            else
            {
                _animator = GetComponent<Animator>();
            }

            if (_animator != null)
            {
                _animator.logWarnings = false;
            }

            _autoRespawn = GetComponent<AutoRespawn>();
            _healthBar = GetComponent<MMHealthBar>();
            _controller = GetComponent<TopDownController>();
            _characterController = GetComponent<CharacterController>();
            _collider2D = GetComponent<Collider2D>();
            _collider3D = GetComponent<Collider>();

            DamageMMFeedbacks?.Initialization(this.gameObject);
            DeathMMFeedbacks?.Initialization(this.gameObject);

            _initialPosition = transform.position;
            _initialized = true;

            //add move speed with level strength
            if (this.gameObject.tag == Tags.Enemies || (gameObject.tag == Tags.Patner && 
            GetComponent<PatnerController>()._summonType == SummonType.NonSummon))
            {
                additiveHealth += (int)(MaximumHealth * (LevelUnitManager.Instance.ReturnCurrentLevelStrength() * 0.05f));
                additiveDefence += (int)(Defence * (LevelUnitManager.Instance.ReturnCurrentLevelStrength() * 0.05f));
            }

            InitialCurrentHP();

            DamageEnabled();

            UpdateHealthBar(false);

            aibrain = GetComponent<AIBrain>();
            skillInput1 = GetComponent<CharacterSkill1>();
            skillInput2 = GetComponent<CharacterSkill2>();


        }

        /// <summary>
        /// Initial hp that this health have not character configure
        /// </summary>

        void InitialNonCharacterTableHp()
        {
            if (characterConfigure == null)
            {
                MaximumHealth = InitialHealth;
                CurrentHealth = InitialHealth;
            }

            if(Model)
            {
                Model.SetActive(true);
            }

            DamageEnabled();
        }

        /// <summary>
        /// Called when the object takes damage
        /// </summary>
        /// <param name="damage">The amount of health points that will get lost.</param>
        /// <param name="instigator">The object that caused the damage.</param>
        /// <param name="flickerDuration">The time (in seconds) the object should flicker after taking the damage.</param>
        /// <param name="invincibilityDuration">The duration of the short invincibility following the hit.</param>
        public virtual void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration, 
            System.Action callBack, bool isCrit = false)
        {
            // if the object is invulnerable, we do nothing and exit
            if (Invulnerable)
            {
                return;
            }

            // if we're already below zero, we do nothing and exit
            if ((CurrentHealth <= 0) && (InitialHealth != 0))
            {
                return;
            }

            // we decrease the character's health by the damage
            float previousHealth = CurrentHealth;

            HTDamagePoup _damagePoup = null;

            //damage -= Defence;//2020 2 13
            bool tempDodge = false;

            if (dodge != null)
            {
                tempDodge = dodge.SuccessDodge();
                damage = tempDodge ? 0 : damage - Defence;//2020 2 14
                damage = damage <= 0 ? 1 : damage;
            }

            //shielder passive skill 
            if (focus.FocusTargets[0].name == ShieldGuardPassiveSkillName)
            {
                if (ShieldGuardPassiveSkill._instance.isProtected == true)
                {
                    damage = damage - ((damage * 100) / 200);
                }
                else
                {
                }

            }

            if (_simpleObjectPooler != null)
            {
                GameObject go = _simpleObjectPooler.GetPooledGameObject();

                _damagePoup = go.GetComponent<HTDamagePoup>();
                if (_damagePoup != null)
                {
                    _damagePoup.SetTarget(this.transform);
                }

                if (DamagePoupPosition != null)
                {
                    go.transform.position = DamagePoupPosition.position;

                }
                else
                {
                    go.transform.position = transform.position;
                }

                go.SetActive(true);
            }

            if (dodge != null && tempDodge)
            {
                if (_damagePoup != null)
                {
                    _damagePoup.SetMissDamageColor();
                }
            }
            else
            {
                if (isCrit)
                {
                    if (_damagePoup != null)
                    {
                        _damagePoup.SetCritDamageColor(damage.ToString());
                    }

                }
                else
                {
                    if (_damagePoup != null)
                    {
                        _damagePoup.SetNormalDamageColor(damage.ToString());
                    }

                }
            }

            if (!string.IsNullOrEmpty(_damageSpawnObject)) //damage feedback particle e.p blood, small stone clip
            {
                damageSpawnObject = PoolManagerV2.Instance.GetInst(_damageSpawnObject);
                Vector3 damageSpawnDirection;
                damageSpawnDirection = transform.position - instigator.transform.position;
                damageSpawnDirection.y = 0;
                damageSpawnObject.transform.forward = damageSpawnDirection;
                damageSpawnObject.transform.position = this.transform.position + damageSpawnObject.transform.forward * _damageSpawnObjectOffset.z +
                    damageSpawnObject.transform.right * _damageSpawnObjectOffset.x + damageSpawnObject.transform.up * _damageSpawnObjectOffset.y;

            }

            CurrentHealth -= damage;

            if (instigator != null && instigator.GetComponent<DamageOnTouch>() != null)
            {
                GameObject damageSourceGo = instigator.GetComponent<DamageOnTouch>().Owner;

                if (damageSourceGo != null)
                {
                    //Suck blood brocast
                    if (damageSourceGo.tag == Tags.Player)
                    {

                        if (EventTypeManager.ContainHTEventType(HTEventType.SuckBlood))
                        {
                            EventTypeManager.Broadcast<int>(HTEventType.SuckBlood, damage);
                        }
                    }
                }

            }



            if (callBack != null)
            {
                callBack?.Invoke();
            }

            if (OnHit != null)
            {
                OnHit();

            }

            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }

            // we prevent the character from colliding with Projectiles, Player and Enemies
            if (invincibilityDuration > 0)
            {
                DamageDisabled();
                StartCoroutine(DamageEnabled(invincibilityDuration));
            }

            // we trigger a damage taken event
            MMDamageTakenEvent.Trigger(_character, instigator, CurrentHealth, damage, previousHealth);

            if (_animator != null && CurrentHealth > 0)
            {
                _animator.SetTrigger("Damage");
            }

            if (damage > 0)
            {
                DamageMMFeedbacks?.PlayFeedbacks(this.transform.position);
            }

            // we update the health bar
            UpdateHealthBar(true);

            // if health has reached zero
            if (CurrentHealth <= 0)
            {
                // we set its health to zero (useful for the healthbar)
                CurrentHealth = 0;

                Kill();
            }
        }

        /// <summary>
        /// Kills the character, vibrates the device, instantiates death effects, handles points, etc
        /// </summary>
        public virtual void Kill()
        {
            if (_character != null)
            {

                // we set its dead state to true
                _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
                _character.Reset();

                if (_character.CharacterType == Character.CharacterTypes.Player)
                {
                    TopDownEngineEvent.Trigger(TopDownEngineEventTypes.PlayerDeath, _character);
                }

            }
            CurrentHealth = 0;

            DamageDisabled();

            DeathMMFeedbacks?.PlayFeedbacks(this.transform.position);

            // Adds points if needed.
            if (PointsWhenDestroyed != 0)
            {
                // we send a new points event for the GameManager to catch (and other classes that may listen to it too)
                TopDownEnginePointEvent.Trigger(PointsMethods.Add, PointsWhenDestroyed);
            }

            if (_animator != null)
            {
                if (!_animator.enabled)
                    _animator.enabled = true;

                _animator.SetTrigger("Death");
            }
            // we make it ignore the collisions from now on
            if (DisableCollisionsOnDeath)
            {
                if (_collider2D != null)
                {
                    _collider2D.enabled = false;
                }
                if (_collider3D != null)
                {
                    _collider3D.enabled = false;
                }

                // if we have a controller, removes collisions, restores parameters for a potential respawn, and applies a death force
                if (_controller != null)
                {
                    _controller.CollisionsOff();
                }
            }

            OnDeath?.Invoke();

            if (DisableControllerOnDeath && (_controller != null))
            {
                _controller.enabled = false;
                //_controller.SetKinematic(true);
            }

            if (DisableControllerOnDeath && (_characterController != null))
            {
                _characterController.enabled = false;
            }

            if (DisableModelOnDeath && (Model != null))
            {
                Model.SetActive(false);
            }

            if (DisableAI && aibrain != null)
            {
                aibrain.enabled = false;
            }
            if (DisableSkillRelese && skillInput1 != null && skillInput2 != null)
            {
                skillInput1.enabled = false;
                skillInput2.enabled = false;
            }

            if (DelayBeforeDestruction > 0f)
            {
                if (_IsDeadDissolve) // Dissolve when character dead
                {
                    Invoke("StartDissolve", DelayBeforeDestruction * _destroyDissolvePercent);
                }
                if(_character&&_character.IsBoss && gameObject!=null)
                {
                    Destroy(_bosshealthBarUI.gameObject);
                }
                Invoke("DestroyObject", DelayBeforeDestruction);
            }
            else
            {
                // finally we destroy the object
                DestroyObject();
            }

            if (this.gameObject.tag == Tags.Enemies)//Enemy count -1
            {
                EventTypeManager.Broadcast<int>(HTEventType.CountAI, -1);
            }
        }

        /// <summary>
        /// Revive this object.
        /// </summary>
        public virtual void Revive()
        {
            if (!_initialized)
            {
                return;
            }

            if (_collider2D != null)
            {
                _collider2D.enabled = true;
            }
            if (_collider3D != null)
            {
                _collider3D.enabled = true;
            }
            if (_characterController != null)
            {
                _characterController.enabled = true;
            }
            if (_controller != null)
            {
                _controller.enabled = true;
                _controller.CollisionsOn();
                _controller.Reset();
            }
            if (_character != null)
            {
                _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
            }
            if (_renderer != null)
            {
                _renderer.material.color = _initialColor;
            }

            if (RespawnAtInitialLocation)
            {
                transform.position = _initialPosition;
            }
            if (_healthBar != null)
            {
                _healthBar.Initialization();
            }

            if (DisableSkillRelese && skillInput1 != null && skillInput2 != null)
            {
                skillInput1.enabled = true;
                skillInput2.enabled = true;
            }

            Initialization();

            if (OnRevive != null)
            {
                OnRevive();
            }
        }

        /// <summary>
        /// Destroys the object, or tries to, depending on the character's settings
        /// </summary>
        protected virtual void DestroyObject()
        {
            if (!DestroyOnDeath)
            {
                return;
            }

            if (_autoRespawn == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _autoRespawn.Kill();
            }


        }


        /// <summary>
        /// Update health UI bar
        /// </summary>
        /// <param name="show"></param>
        /// <param name="Anim">Is use anim to lerp the  health bar</param>
        public virtual void UpdateHealthBar( bool Anim = true)
        {
            if (_character&&!_character.IsBoss)
            {
                if (_healthBarUI != null)
                {
                    _healthBarUI.UpdateHealthBar(CurrentHealth, 0, MaximumHealth, Anim);
                }
            }
            else
            {
                if (_bosshealthBarUI != null)
                {
                    _bosshealthBarUI.UpdateHealthBar(CurrentHealth, 0, MaximumHealth);
                }
            }

            RestoreHPAnim = true;

            if (_character != null)
            {
                if (_character.CharacterType == Character.CharacterTypes.Player)
                {
                    //Furious talent event brocast
                    EventTypeManager.Broadcast(HTEventType.Furious, CurrentHealth, MaximumHealth);
                }
            }
        }

        /// <summary>
        /// Prevents the character from taking any damage
        /// </summary>
        public virtual void DamageDisabled()
        {
            Invulnerable = true;

        }

        /// <summary>
        /// Allows the character to take damage
        /// </summary>
        public virtual void DamageEnabled()
        {
            Invulnerable = false;

        }

        /// <summary>
        /// makes the character able to take damage again after the specified delay
        /// </summary>
        /// <returns>The layer collision.</returns>
        public virtual IEnumerator DamageEnabled(float delay)
        {
            yield return new WaitForSeconds(delay);
            Invulnerable = false;
        }

        /// <summary>
        /// On Disable, we prevent any delayed destruction from running
        /// </summary>
        protected virtual void OnDisable()
        {
            CancelInvoke();
        }

        protected virtual void OnDestroy()
        {
            if (this.transform.tag == Tags.Player)
            {
                if (PlayerPrefs.GetInt(Consts.GameCurrentHP + SaveManager.Instance.LoadGameID) == 666666)
                {
                    PlayerPrefs.DeleteKey(Consts.GameCurrentHP + SaveManager.Instance.LoadGameID);
                    return;
                }

                PlayerPrefs.SetInt(Consts.GameCurrentHP + SaveManager.Instance.LoadGameID, CurrentHealth);
                PlayerPrefs.Save();

                Debugs.LogInformation("OnDestroy Current Health:" + CurrentHealth + " MaxHealth:" + MaximumHealth, Color.green);
            }
        }

        /// <summary>
        /// Reuduece defence with percent
        /// </summary>
        /// <param name="percent"></param>
        public void ReduceDefenceByPercent(float percent)
        {
            if (_originalDefence != 0)
            {
                return;
            }

            int targetStep = (int)(Defence * percent);

            _originalDefence = additiveDefence;

            additiveDefence -= targetStep;
        }

        /// <summary>
        /// Restore be reduced defecne
        /// </summary>
        public void RestoreDefence()
        {
            additiveDefence = _originalDefence;
            _originalDefence = 0;
        }

        /// <summary>
        /// Start dissloving
        /// </summary>
        void StartDissolve()
        {
            _renderer.material = _dissolveMaterial;
            _dissolveSpeed = Random.Range(0.01f, 0.02f);
            _IsDissolving = true;
            _dissolveTimer = 0;
        }


        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if (_IsDissolving)
            {
                float clip = Mathf.Lerp(0, 1, _dissolveTimer);
                _dissolveTimer += _dissolveSpeed;
                AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(_renderer.material,
                    AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
            }
        }
    }
}
