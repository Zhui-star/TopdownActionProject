using MoreMountains.Feedbacks;
using UnityEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// Implement camera distance change 
    /// </summary>
    [FeedbackPath("HTLibrary/CameraDisatanceShake")]
    public class CameraDistacneShake : MMFeedback
    {
        private HTCameraController _cameraController;
        public float _targetDistance;
        public float _transitionDuration;
        public float _duration;
        protected override void CustomInitialization(GameObject owner)
        {
            base.CustomInitialization(owner);
            _cameraController = FindObjectOfType<HTCameraController>();
        }
        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1)
        {
            _cameraController.SetCameraDistance(_targetDistance, _transitionDuration, _duration);
        }

    }
}

