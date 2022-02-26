using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using HTLibrary.Application;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    public class SpikeSkill : DamageOnTouch
    {

        [Header("眩晕机制/冻结机制")]
        public bool Freeze = false;
        public bool Frozen=false;
        public float FreezeSecond = 2.0f;
        [Header(" ")]
        public Vector3 KnockUpVector;

        public bool hideWithDamageable = false;

        [Header("减速机制")]
        public bool SlowDown = false;
        public float SlowDownSecond = 2.0f;
        public float SlowDownStep = -0.5f;

        [Header("伤害递增伴随着Time.deltatime")]
        public bool IsIncreaseDamage;
        public float IncreaseDamge;

        [Header("嘲讽机制")]
        public bool _radicule;
        public float _radiculeSeconds = 0;

        private bool setAdditiveDamageFinished = false;

        [Header("技能伤害额外百分比")]
        public float skillDamagePercent = 0;

        [Header("伤害是否跟随关卡强度")]
        public bool _followLevelStrength = false;

        [Header("是否是角色技能")]
        public bool _playerSkill = false;

        
        protected override void Awake()
        {
            base.Awake();
            InitialSkillDamage();
            InitialAIDamage();

            Character _character = CharacterManager.Instance.GetCharacter("Player1");
            if(_playerSkill)
            {
                characterconfigure = _character.characterTable;
            }

        }

        protected override  void OnEnable()
        {
            base.OnEnable();          
        }

        public void FreezeCharacter(Health health)
        {
            if(health!=null&&health.GetComponent<CharacterFunctionSwitch>()!=null)
            {
                health.GetComponent<CharacterFunctionSwitch>().Freeze(FreezeSecond);
            }
        }

        public void KnockUpCharacter(Health health)
        {
            //TODO 击飞
        }

        public void SlowDownCharacter(Health health)
        {
            if(health==null)
            {
                return;
            }

            CharacterFunctionSwitch characterFunctionSwich = health.GetComponent<CharacterFunctionSwitch>();

            if(characterFunctionSwich==null)
            {
                return;
            }

            health.GetComponent<CharacterFunctionSwitch>().SlowDown(SlowDownSecond,SlowDownStep);
        }

        protected override void OnCollideWithDamageable(Health health)
        {
            if(health.Invulnerable)return;
            
            base.OnCollideWithDamageable(health);

            if(Freeze)
            {
                FreezeCharacter(health);

                if(hideWithDamageable)
                {
                    this.gameObject.SetActive(false);
                }
            }

            if(Frozen)
            {
                FrozenCharacter(health);                
            }

            if(SlowDown)
            {
                SlowDownCharacter(health);
            }

            if(_radicule)
            {
                Transform targetTransform = Owner.transform.root;
                RadiculeCharacter(targetTransform, _radiculeSeconds, health);
            }
        }

        private void FixedUpdate()
        {
            if(IsIncreaseDamage)
            {
                DamageCaused +=(int)(IncreaseDamge * Time.deltaTime);
            }
        }

        /// <summary>
        /// 额外的伤害赋予
        /// </summary>
        /// <param name="damage"></param>
        public void SetAddtiveDamage(float damage)
        {
            if(!setAdditiveDamageFinished)
            {
                additiveDamage += (int)damage;
                setAdditiveDamageFinished = true;
            }

        }

        /// <summary>
        /// 嘲讽
        /// </summary>
        /// <param name="target"></param>
        /// <param name="seconds"></param>
        /// <param name="health"></param>
        public void RadiculeCharacter(Transform target,float seconds, Health health)
        {
            health.GetComponent<CharacterFunctionSwitch>().Radicule(target, seconds);
        }

        /// <summary>
        /// 初始化技能伤害
        /// </summary>
        void InitialSkillDamage()
        {
            int targetDamage = (int)(DamageCaused * skillDamagePercent);

            additiveDamage += targetDamage;
        }


        /// <summary>
        /// 初始化技能伤害
        /// </summary>
        void InitialAIDamage()
        {
            if(_followLevelStrength)
            {
                SetAddtiveDamage(LevelUnitManager.Instance.ReturnCurrentLevelStrength() * DamageCaused * 0.05f);
            }
        }

        ///冻结敌人
        private void FrozenCharacter(Health health)
        {
            if(health!=null&&health.GetComponent<CharacterFunctionSwitch>()!=null)
            {
                health.GetComponent<CharacterFunctionSwitch>().Forzen(FreezeSecond);
            }
        }

      
    }

}
