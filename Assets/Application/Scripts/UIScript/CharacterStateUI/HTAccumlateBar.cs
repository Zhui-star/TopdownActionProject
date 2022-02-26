using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    /// <summary>
    /// 蓄力条UI
    /// </summary>
    public class HTAccumlateBar : MonoBehaviour
    {
        public Slider _accumlateSlide;
        [HideInInspector]
        private CharacterHandleWeapon _handleWeapon;
        private CharacterManager _characterManager;
        private void Awake()
        {
            _characterManager = CharacterManager.Instance;
            _handleWeapon = _characterManager.GetCharacter("Player1").GetComponent<CharacterHandleWeapon>();
        }

        private void OnEnable()
        {
            EventTypeManager.AddListener(HTEventType.StartAccumlate, OpenSlider);
            EventTypeManager.AddListener(HTEventType.StopAccumlate, CloseSlider);
            _handleWeapon.WeaponAccumlateEvent += UpdateSlider;

            transform.localScale = Vector2.zero;
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener(HTEventType.StartAccumlate, OpenSlider);
            EventTypeManager.RemoveListener(HTEventType.StopAccumlate, CloseSlider);
            _handleWeapon.WeaponAccumlateEvent -= UpdateSlider;
        }

        /// <summary>
        /// 更新蓄力条
        /// </summary>
        /// <param name="percent"></param>
        void UpdateSlider(float percent)
        {
            _accumlateSlide.value = percent;
        }

        /// <summary>
        /// 打开蓄力条
        /// </summary>
        void OpenSlider()
        {
            transform.DOScale(1, 0.2f);
        }

        /// <summary>
        /// 关闭蓄力条
        /// </summary>
        void CloseSlider()
        {
            transform.DOScale(0, 0.2f);
        }

        /// <summary>
        /// 赋值组件
        /// </summary>
        /// <param name="handleWeapon"></param>
        public void SetComponent(CharacterHandleWeapon handleWeapon)
        {
            this._handleWeapon = handleWeapon;
        }

        /// <summary>
        /// 朝向相机
        /// </summary>
        void LookCamera()
        {
            Vector3 targetPosition = transform.position + Camera.main.transform.rotation * Vector3.forward;
            Vector3 targetOrient = Camera.main.transform.rotation * Vector3.up;
            transform.LookAt(targetPosition, targetOrient);
        }

        private void LateUpdate()
        {
            LookCamera();
        }
    }
}

