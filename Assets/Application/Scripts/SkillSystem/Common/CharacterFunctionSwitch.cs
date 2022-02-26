using System.Collections;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using System;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    public class CharacterFunctionSwitch : MonoBehaviour
    {
        private Animator _anim;

        private Character _character;

        private CharacterMovement movement;

        private CharacterOrientation3D _orientation3D;

        private CharacterOrientation3D orientation;

        private CharacterHandleWeapon weaponHandle;

        private SkillReleaseTrigger skillReleseTrigger;

        private CharacterDash3D dash3D;

        private Status _status;

        private Health _health;

        AIBrain _aiBrain;

        MMSimpleObjectPooler poupObjectPool;
        [SerializeField] Transform poupTransform;

        IEnumerator IcontrollerBehaviour;
        IEnumerator IslowDownControllerBehaviour;
        IEnumerator IhealControllerBehaviour;
        IEnumerator IRadiculeController;
        IEnumerator IFrozenController;

        //是否正处于减速状态
        bool slowdowning = false;

        //是否处于眩晕状态
        bool frezzing = false;

        //冻结事件
        public event Action<bool> EventHandlerFrozen;

        public event Action StopSkillReleaseHandler;
        private void Start()
        {
            _character = GetComponent<Character>();
            movement = GetComponent<CharacterMovement>();
            orientation = GetComponent<CharacterOrientation3D>();
            weaponHandle = GetComponent<CharacterHandleWeapon>();
            skillReleseTrigger = GetComponent<SkillReleaseTrigger>();
            _status = GetComponent<Status>();
            _health = GetComponent<Health>();
            _aiBrain = GetComponent<AIBrain>();
            _orientation3D = GetComponent<CharacterOrientation3D>();
            poupObjectPool = GetComponent<MMSimpleObjectPooler>();

            _anim = _character._animator;
        }

        public void TurnOnSkillFunction()
        {
            if (skillReleseTrigger == null) return;
            skillReleseTrigger.FreezeSkill = false;
        }

        public void TurnOffSkillFuntion()
        {
            if (skillReleseTrigger == null) return;
            skillReleseTrigger.FreezeSkill = true;
            skillReleseTrigger.StopSkillRelese();
        }

        public void TurnOffMovement()
        {
            if (movement == null) return;
            movement.MovementForbidden = true;
            if (orientation == null) return;
            orientation.AbilityPermitted = true;
        }

        public void TunrOnMovement()
        {
            if (movement == null) return;
            movement.MovementForbidden = false;
            if (orientation == null) return;
            orientation.AbilityPermitted = false;
        }

        public void TurnOnAttack()
        {
            if (weaponHandle == null) return;
            weaponHandle.AbilityPermitted = true;
        }

        public void TurnOffAttack()
        {
            if (weaponHandle == null) return;
            weaponHandle.AbilityPermitted = false;
        }

        /// <summary>
        /// Turn off dash 3D component which mean is ability permit is false
        /// </summary>
        void TunrOffDashAbilities()
        {
            if(dash3D)
            {
                dash3D.AbilityPermitted=false;
            }
        }

        /// <summary>
        /// Reopen dash 3d ability permit
        /// </summary>
        void TunrOnDashAbilities()
        {
            if(dash3D)
            {
                dash3D.AbilityPermitted=true;
            }
        }

        public void Freeze(float second)
        {
            if (frezzing || SuccessResistAbnormal()) return;
            frezzing = true;
            if (IcontrollerBehaviour != null)
            {
                StopCoroutine(IcontrollerBehaviour);
                IcontrollerBehaviour = null;
            }
            _status._Status = CharacterStatus.Freeze;
            IcontrollerBehaviour = IFreeze(second);

            if (gameObject != null)
            {
                StopSkillReleaseHandler?.Invoke();
                StartCoroutine(IcontrollerBehaviour);

            }

        }
        

        private IEnumerator IFreeze(float second)
        {
            TurnOffAttack();
            TurnOffSkillFuntion();
            TurnOffMovement();
            TurnOffRotation();
            TunrOffDashAbilities();
            TurnOffAIBrain();
            TurnOffAnimation();
            yield return new WaitForSeconds(second);
            _status._Status = CharacterStatus.None;
            TurnOnAnimation();
            TurnOnAIBrain();
            TurnOnSkillFunction();
            TunrOnDashAbilities();
            TurnOnAttack();
            TunrOnMovement();
            TurnOnRotation();
            IcontrollerBehaviour = null;
            frezzing = false;
        }

        /// <summary>
        /// 使敌人减速
        /// </summary>
        /// <param name="second">减速多少秒</param>
        public void SlowDown(float second, float slowdownStep = 0.5f)
        {
            if (slowdowning || SuccessResistAbnormal()) return;
            slowdowning = true;
            if (IslowDownControllerBehaviour != null)
            {
                StopCoroutine(IslowDownControllerBehaviour);
                IslowDownControllerBehaviour = null;
            }

            IslowDownControllerBehaviour = ISlowDonw(second, slowdownStep);

            if (this.gameObject.activeInHierarchy)
            {
                StartCoroutine(IslowDownControllerBehaviour);
            }

        }

        private IEnumerator ISlowDonw(float second, float slowdownStep)
        {
            _status._Status = CharacterStatus.SlowDown;
            movement.additiveMoveSpeed -= slowdownStep;
            yield return new WaitForSeconds(second);
            movement.additiveMoveSpeed += slowdownStep;
            _status._Status = CharacterStatus.None;
            IslowDownControllerBehaviour = null;
            slowdowning = false;
        }

        private void OnDestroy()
        {
            if (slowdowning)
            {

                StopCoroutine(IslowDownControllerBehaviour);
                _status._Status = CharacterStatus.None;
                IslowDownControllerBehaviour = null;
                slowdowning = false;
            }

            if (frezzing)
            {
                frezzing = false;
                StopCoroutine(IcontrollerBehaviour);
                _status._Status = CharacterStatus.None;
                IcontrollerBehaviour = null;
            }
        }

        /// <summary>
        /// 治疗状态
        /// </summary>
        /// <param name="healNumber"></param>
        public void Heal(int healNumber)
        {
            if (_health != null)
            {
                if (IhealControllerBehaviour != null)
                {
                    StopCoroutine(IhealControllerBehaviour);
                }

                IhealControllerBehaviour = IHeal(healNumber);

                StartCoroutine(IhealControllerBehaviour);
            }
        }

        /// <summary>
        /// 携程治疗
        /// </summary>
        /// <param name="healNumber"></param>
        /// <returns></returns>
        private IEnumerator IHeal(int healNumber)
        {
            _status._Status = CharacterStatus.Heal;
            _health.CurrentHealth += healNumber;
            yield return new WaitForSeconds(1.0f);
            _status._Status = CharacterStatus.None;
        }



        /// <summary>
        /// 嘲讽机制
        /// </summary>
        /// <param name="target"></param>
        /// <param name="seconds"></param>
        public void Radicule(Transform target, float seconds)
        {
            if (SuccessResistAbnormal()) return;

            if (target != null && _aiBrain != null && !_character.IsBoss)
            {
                if (IRadiculeController != null)
                {
                    StopCoroutine(IRadiculeController);
                }
                IRadiculeController = IRadicule(target, seconds);

                StartCoroutine(IRadiculeController);
            }

        }

        private IEnumerator IRadicule(Transform target, float seconds)
        {
            _aiBrain.Target = target;
            _status._Status = CharacterStatus.Radicule;
            yield return new WaitForSeconds(seconds);
            _status._Status = CharacterStatus.None;

        }

        ///冻结物体
        public void Forzen(float second)
        {
            if (SuccessResistAbnormal()) return;
            if (IFrozenController != null)
            {
                StopCoroutine(IFrozenController);
            }

            _status._Status = CharacterStatus.Frozen;
            IFrozenController = IFrozen(second);

            StopSkillReleaseHandler?.Invoke();
            StartCoroutine(IFrozenController);
        }

        ///冻结定时
        private IEnumerator IFrozen(float second)
        {
            TurnOffAttack();
            TurnOffSkillFuntion();
            TurnOffMovement();
            TurnOffRotation();
            TunrOffDashAbilities();
            TurnOffAIBrain();
            TurnOffAnimation();
            EventHandlerFrozen?.Invoke(true);
            yield return new WaitForSeconds(second);
            _status._Status = CharacterStatus.None;
            EventHandlerFrozen?.Invoke(false);
            TurnOnAnimation();
            TurnOnAIBrain();
            TurnOnSkillFunction();
            TunrOnDashAbilities();
            TurnOnAttack();
            TunrOnMovement();
            TurnOnRotation();
        }

        ///关闭旋转
        public void TurnOffRotation()
        {
            _orientation3D.CharacterRotationAuthorized = false;
        }

        ///打开旋转
        public void TurnOnRotation()
        {
            _orientation3D.CharacterRotationAuthorized = true;
        }

        /// <summary>
        /// Off ai control
        /// </summary>
        private void TurnOffAIBrain()
        {
            if (_aiBrain == null) return;
            _aiBrain.BrainActive = false;
            _aiBrain.TimeInThisState = 0;
        }

        /// <summary>
        /// Open ai control
        /// </summary>
        private void TurnOnAIBrain()
        {
            if (_aiBrain == null) return;
            _aiBrain.BrainActive = true;
        }

        /// <summary>
        /// Exit current anim cip
        /// </summary>
        private void TurnOffAnimation()
        {
            _anim.SetTrigger("StopSkillRelease");
        }

        /// <summary>
        /// Restore anim paramter
        /// </summary>
        private void TurnOnAnimation()
        {
            _anim.ResetTrigger("StopSkillRelease");
        }
       
       /// <summary>
       /// Adjust wheather success resist abnormal state
       /// </summary>
       /// <returns></returns>
        bool SuccessResistAbnormal()
        {
            if (!_character.characterTable)
                return false;

            bool success = MathUtility.Percent((int)(_character.characterTable.abNormalResistChance +
            _character.characterTable.additiveNormalResistChance)); ;

            if (success)
            {
                GenerateGUIPoup();
            }

            return success;
        }

        /// <summary>
        /// Spawn a game object of poup which belong GUI
        /// </summary>
        void GenerateGUIPoup()
        {
            GameObject poupGo = poupObjectPool.GetPooledGameObject();
            HTDamagePoup poup = poupGo.GetComponent<HTDamagePoup>();

            poup.SetTarget(this.transform);
            poup.SetAbnormalColor();

            if (poupTransform)
            {
                poupGo.transform.position = poupTransform.position;

            }
            else
            {
                poupGo.transform.position = transform.position;
            }

            poupGo.SetActive(true);
           
        }
    }

}
