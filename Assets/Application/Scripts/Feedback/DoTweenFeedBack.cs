using MoreMountains.Feedbacks;
using UnityEngine;
using DG.Tweening;
namespace HTLibrary.Application
{
    public enum DoTweenType
    {
        None,
        Position,
        Scale,
        Rotation
    }

    /// <summary>
    /// Dotween 反馈
    /// </summary>
    [FeedbackPath("HTLibrary/DoTweenFeedback")]
    public class DoTweenFeedBack : MMFeedback
    {
        public Transform _targetTransform;
        public float _animTime = 0.25f;
        public DoTweenType _dotweenType;

        public Vector3 _targetVector;
        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1)
        {
            if(_targetTransform==null)return;

            switch (_dotweenType)
            {
                case DoTweenType.Scale:
                    _targetTransform.DOScale(_targetVector, _animTime);
                    break;

            }
        }

    }

}
