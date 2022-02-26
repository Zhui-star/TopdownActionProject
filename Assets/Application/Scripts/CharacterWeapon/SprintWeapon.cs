using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using NaughtyAttributes;
namespace HTLibrary.Application
{
    /// <summary>
    /// 冲刺武器  （用来 平A+冲刺 的一种解决方案)
    /// </summary>
    public class SprintWeapon :MonoBehaviour
    {
        private GameObject _damageArea;
        private CharacterDash3D dash3D;
        private bool _attackInprogress;

        [Header("动画")]
        private Animator _ownerAnimator;
        public string SingleUseAnimationParameter;

        [Header("伤害区域")]
        [ReadOnly][SerializeField]private float _activeDuration;
        public float _initialDelay;
        public Vector3 _areaSize;
        public Vector3 _areaOffset;
        private BoxCollider _boxCollider;
        private DamageOnTouch _damageOnTouch;
        private WeaponCrit _wepaonCrit;
        public LayerMask _targetLayerMask;
        public DamageOnTouch.KnockbackStyles _knockBack;
        public Vector3 _kncokBackForce;
        public float _InvincibilityDuration;
        
        [Header("数据")]
        public CharacterConfig _characterConfigure;

        [Header("反馈")]
        public MMFeedbacks _sprintWeaponFeedBack;
        public MMSimpleObjectPooler _damageObject;
        public AudioClip _hitObjectClip;

        //Limit behaviour when character sprint attack   (Component)
        private CharacterOrientation3D _orientation;
        private CharacterMovement movement;

        /// <summary>
        /// 初始化组件
        /// </summary>
        public void Initialization()
        {
            _wepaonCrit = GetComponent<WeaponCrit>();
            
            CreateDamgeArea();
        }

        /// <summary>
        /// 创造伤害区域
        /// </summary>
        private void CreateDamgeArea()
        {
            _damageArea = new GameObject();
            _damageArea.name = this.name + "DamageArea";
            _damageArea.transform.position = this.transform.position;
            _damageArea.transform.rotation = this.transform.rotation;
            _damageArea.transform.SetParent(this.transform);
            _damageArea.layer = this.gameObject.layer;

            _boxCollider = _damageArea.AddComponent<BoxCollider>();
            _boxCollider.center = _areaOffset;
            _boxCollider.size = _areaSize;
            _boxCollider.isTrigger = true;
            _boxCollider.enabled = false;

            Rigidbody rigidBody = _damageArea.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;

            _damageOnTouch = _damageArea.AddComponent<DamageOnTouch>();
            _damageOnTouch.hitDirection = this.transform;
            _damageOnTouch._crit = this._wepaonCrit != null ? this._wepaonCrit : null;

            _damageOnTouch.TargetLayerMask = _targetLayerMask;
            _damageOnTouch.characterconfigure = _characterConfigure;

            _damageOnTouch.DamageCausedKnockbackType = _knockBack;
            _damageOnTouch.DamageCausedKnockbackForce = _kncokBackForce;

        }

        /// <summary>
        /// 武器使用
        /// </summary>
        public  void WeaponUse()
        {           
            StartCoroutine(SprintWeaponAttack());
        }

        /// <summary>
        /// 启动伤害区
        /// </summary>

        private void EnableDamageArea()
        {
            if (_boxCollider != null)
            {
                _boxCollider.enabled = true;

                _damageOnTouch.hitEffect = _damageObject.GetPooledGameObject();

                if (_hitObjectClip != null)
                {
                    _damageOnTouch.swordClip = _hitObjectClip;
                }
            }
        }

        /// <summary>
        /// 禁用伤害区域
        /// </summary>
        private void DisableDamageArea()
        {
            if(_boxCollider!=null)
            {
                _boxCollider.enabled = false;
            }
        }

        /// <summary>
        /// 武器攻击过程
        /// </summary>
        /// <returns></returns>
        private IEnumerator SprintWeaponAttack()
        {
            if (_attackInprogress)
            {
                yield break;
            }

            (float dashTimer,float dashDuration) dashTimeRelevant=dash3D.GetDashingTimeRelevant();
            _activeDuration=dashTimeRelevant.dashDuration-dashTimeRelevant.dashTimer;
            if(_activeDuration<=0)
                yield break;

            _attackInprogress = true;
            _orientation.RotationMode = CharacterOrientation3D.RotationModes.WeaponDirection;
            yield return new WaitForEndOfFrame();
            _orientation.CharacterRotationAuthorized=false;
            movement.MovementForbidden=true;

            _ownerAnimator.SetBool(SingleUseAnimationParameter, true);

            yield return new WaitForSeconds(_initialDelay);

            _sprintWeaponFeedBack?.PlayFeedbacks();

            EnableDamageArea();
            yield return new WaitForSeconds(_activeDuration);
            DisableDamageArea();
            _ownerAnimator.SetBool(SingleUseAnimationParameter, false);

            _orientation.CharacterRotationAuthorized=true;
            movement.MovementForbidden=false;

            _orientation.RotationMode = CharacterOrientation3D.RotationModes.MovementDirection;
            _attackInprogress = false;
        }

        /// <summary>
        /// Debug 选中显示伤害区域
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            if(!UnityEngine.Application.isPlaying)
            {
                DrawGizmoz();
            }
        }

        /// <summary>
        /// 绘制伤害区域
        /// </summary>
        protected virtual void DrawGizmoz()
        {
            Gizmos.DrawWireCube(this.transform.position + _areaOffset, _areaSize);
        }

        public void SetOwner(CharacterHandleWeapon handleWeapon)
        {
            _ownerAnimator= handleWeapon.CharacterAnimator;
            _orientation = handleWeapon.GetComponent<CharacterOrientation3D>();
            movement=handleWeapon.GetComponent<CharacterMovement>();
            dash3D=handleWeapon.GetComponent<CharacterDash3D>();
        }

    }

}
