using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using DG.Tweening;
namespace HTLibrary.Application
{
    /// <summary>
    /// when player has low health it will be ref
    /// </summary>
    [FeedbackPath("HTLibrary/Dangerous health feedback")]
    public class BloodScreenFlash : MMFeedback
    {
        public float _flashDuration;
        private Image _bloodImg;

        protected override void CustomInitialization(GameObject owner)
        {
            base.CustomInitialization(owner);
            _bloodImg = GameObject.Find("UICanvas").transform.Find("BloodScreen").GetComponent<Image>();
        }

        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1)
        {
            if (_bloodImg != null)
            {
                _bloodImg.enabled = true;
                _bloodImg.DOColor(new Color(_bloodImg.color.r, _bloodImg.color.g, _bloodImg.color.b, 0.8f), _flashDuration * 0.5f);
                Invoke("RestoreColor", _flashDuration * 0.5f);
            }

        }

        void RestoreColor()
        {
            _bloodImg.DOColor(new Color(_bloodImg.color.r, _bloodImg.color.g, _bloodImg.color.b, 0f), _flashDuration * 0.5f);
        }

        protected override void CustomStopFeedback(Vector3 position, float attenuation = 1)
        {
            base.CustomStopFeedback(position, attenuation);
            _bloodImg.enabled = false;
        }

    }

}
