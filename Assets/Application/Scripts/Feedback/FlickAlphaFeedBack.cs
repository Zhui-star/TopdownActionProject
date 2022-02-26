using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;
namespace HTLibrary.Application
{
    [FeedbackPath("HTLibrary/FlickAlpha")]
    public class FlickAlphaFeedBack : MMFeedback
    {
        public Renderer _render;

        public float _flickSpeed;

        public float _flickAlpha;

        public float _initialAlpha;

        public float _flickDuration;

        public float _stepSecond;

        public bool _reset;
        float targetAlpha;
        protected override void CustomInitialization(GameObject owner)
        {
            base.CustomInitialization(owner);
        }

        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1)
        {
            if(Active&&_render!=null)
            {
                StartCoroutine(Flicker(_render, _initialAlpha, _flickAlpha, _flickDuration, _flickSpeed,_stepSecond));
            }
        }

        protected override void CustomReset()
        {
            base.CustomReset();

            if(_reset)
            {
                if (_render != null&&_render.material.HasProperty("_Opacity"))
                {
                    _render.material.SetFloat("_Opacity", _initialAlpha);
                }
            }
        }

        /// <summary>
        /// 开始渐变
        /// </summary>
        /// <param name="render"></param>
        /// <param name="initialAlpha"></param>
        /// <param name="flickAlpha"></param>
        /// <param name="flickDuration"></param>
        /// <param name="flickSpeed"></param>
        /// <param name="stepSecond"></param>
        /// <returns></returns>
        private IEnumerator Flicker(Renderer render,float initialAlpha, float flickAlpha,float flickDuration,float flickSpeed,float stepSecond)
        {
            if(_render==null)
            {
                yield break;
            }

            if(!_render.material.HasProperty("_Opacity"))
            {
                yield break;
            }

            if(initialAlpha==flickAlpha)
            {
                yield break;
            }

            float flickStop = FeedbackTime + flickDuration;
            targetAlpha = initialAlpha;
            while(FeedbackTime<flickStop)
            {
                targetAlpha = Mathf.Lerp(targetAlpha, flickAlpha, flickSpeed * Time.deltaTime);
                _render.material.SetFloat("_Opacity", targetAlpha);
                yield return new WaitForSeconds(stepSecond);
            }
        }


    }

}
