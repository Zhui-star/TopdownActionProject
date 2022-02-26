using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    [FeedbackPath("HTLibrary/GhostShadow")]
    public class GhostFeedbackShadow : MMFeedback
    {
        public GhostShadow[] ghostShadows;

        public Material ghostMaterial;

        protected override void CustomInitialization(GameObject owner)
        {
            base.CustomInitialization(owner);
            foreach(var temp in ghostShadows)
            {
                temp.ghostShadowmMaterial = ghostMaterial;
            }
        }
        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1)
        {
            PlayGhostShaodw();
        }

        void PlayGhostShaodw()
        {
            foreach(var temp in ghostShadows)
            {
                temp.MakeGhostShadow();
            }
        }

        private void OnEnable()
        {
            EventTypeManager.AddListener(HTEventType.CreatGhostShadow, PlayGhostShaodw);
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener(HTEventType.CreatGhostShadow, PlayGhostShaodw);
        }
    }

}
