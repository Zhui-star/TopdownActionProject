using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// This feedback will play the associated particles system on play, and stop it on stop
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will simply play the specified ParticleSystem (from your scene) when played.")]
    [FeedbackPath("Particles/Particles Play")]
    public class MMFeedbackParticles : MMFeedback
    {
        [Header("Bound Particles")]
        /// the particle system to play with this feedback
        public ParticleSystem BoundParticleSystem;
        /// if this is true, the particles will be moved to the position passed in parameters
        public bool MoveToPosition = false;

        [Header("特效细节处理 ")]
        public bool _detachParent = false;
        private Vector3 _orginPostion;
        private Vector3 _orginScale;
        /// <summary>
        /// On init we stop our particle system
        /// </summary>
        /// <param name="owner"></param>
        protected override void CustomInitialization(GameObject owner)
        {
            base.CustomInitialization(owner);
            BoundParticleSystem?.Stop();

            if(BoundParticleSystem!=null)
            {
                _orginPostion = BoundParticleSystem.transform.localPosition;
                _orginScale = BoundParticleSystem.transform.localScale;
            }
        }

        /// <summary>
        /// On play we play our particle system
        /// </summary>
        /// <param name="position"></param>
        /// <param name="attenuation"></param>
        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1.0f)
        {
            if (!Active)
            {
                return;
            }
            if (MoveToPosition)
            {
                BoundParticleSystem.transform.position = position;
            }

            if(_detachParent)
            {
                BoundParticleSystem.transform.SetParent(transform.parent);
                BoundParticleSystem.transform.localPosition = _orginPostion;
                BoundParticleSystem.transform.localRotation = Quaternion.Euler(0, 0, 0);
                BoundParticleSystem.transform.localScale = _orginScale;
                BoundParticleSystem.transform.SetParent(null);
            }

            BoundParticleSystem?.Play();
        }
        
        /// <summary>
        /// On Stop, stops the particle system
        /// </summary>
        /// <param name="position"></param>
        /// <param name="attenuation"></param>
        protected override void CustomStopFeedback(Vector3 position, float attenuation = 1.0f)
        {
            if (!Active)
            {
                return;
            }

            BoundParticleSystem?.Stop();
        }

        /// <summary>
        /// On Reset, stops the particle system 
        /// </summary>
        protected override void CustomReset()
        {
            base.CustomReset();
            BoundParticleSystem?.Stop();
        }
    }
}
