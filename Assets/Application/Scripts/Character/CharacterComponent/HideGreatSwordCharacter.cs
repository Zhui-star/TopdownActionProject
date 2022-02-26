
using UnityEngine;
using HTLibrary.Framework;
using DG.Tweening;
using System.Collections;
namespace HTLibrary.Application
{
    /// <summary>
    /// 鬼斩技能所需隐藏角色脚本
    /// </summary>
    public class HideGreatSwordCharacter : MonoBehaviourSimplify
    {
        public Collider _collider;
        public CharacterController _characterController;
        public GameObject _model;
        public GameObject _healthBar;
        private Vector3 _originalScale;
        private Vector3 _healthBarScale;
        private void Start()
        {
            _originalScale = _model.transform.localScale;
            _healthBarScale = _healthBar.transform.localScale;
           
        }

        private void OnEnable()
        {
            EventTypeManager.AddListener<bool>(HTEventType.HideGreatPlayer, TurnState);
        }


        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state"></param>

        void TurnState(bool state)
        {
            StartCoroutine(ITurnState(state));
        }

        IEnumerator ITurnState(bool state)
        {
            if(state)
            {
                _model.transform.DOScale(_originalScale, 0.1f);
                _healthBar.transform.DOScale(_healthBarScale, 0.1f);
                _collider.enabled = state;
                _characterController.enabled = state;
            }

            yield return new WaitForSeconds(0.3f);
            if (!state)
            {
                _model.transform.DOScale(0, 0.1f);
                _healthBar.transform.DOScale(0, 0.1f);
                _collider.enabled = state;
                _characterController.enabled = state;
            }
        
        }
        

        private void OnDisable()
        {
            EventTypeManager.RemoveListener<bool>(HTEventType.HideGreatPlayer, TurnState);
        }

        protected override void OnBeforeDestroy()
        {

        }
    }

}
