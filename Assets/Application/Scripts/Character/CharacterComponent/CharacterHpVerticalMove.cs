using UnityEngine;
using System.Collections;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    /// <summary>
    /// 角色血条上移
    /// </summary>
    public class CharacterHpVerticalMove : MonoBehaviour
    {
        public float _smoothTime;
        public Vector3 _targetPositionOffset;
        private Vector3 _orginalPosition;
        private Vector3 _targetPosition;
        private Vector3 _velocity;
        private void OnEnable()
        {
            EventTypeManager.AddListener<bool>(HTEventType.HPVerticalMove, VerticalMove);
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener<bool>(HTEventType.HPVerticalMove, VerticalMove);
        }
        /// <summary>
        /// 开始上移
        /// </summary>
        /// <param name="up"></param>
        private void VerticalMove(bool up)
        {
            StartCoroutine(SmoothVerticalMove(up));
        }

        /// <summary>
        /// 平滑移动
        /// </summary>
        /// <param name="up"></param>
        /// <returns></returns>
        private IEnumerator SmoothVerticalMove(bool up)
        {
            Vector3 targetPosition;
            if(up)
            {
                targetPosition = _targetPosition;
            }
            else
            {
                targetPosition = _orginalPosition;
            }
            float distance = Vector3.Distance(transform.localPosition, targetPosition);
            while(distance>=0.05f)
            {
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition,ref _velocity, _smoothTime);
                distance = Vector3.Distance(transform.localPosition, targetPosition);
                yield return null;
            }
        }

        private void Start()
        {
            _orginalPosition = transform.localPosition;
            _targetPosition = _orginalPosition + _targetPositionOffset;
        }
    }

}
