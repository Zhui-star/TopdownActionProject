using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using HTLibrary.Application;
using MoreMountains.Tools;
namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// When played, this feedback will activate the Wiggle method of a MMWiggle object based on the selected settings, wiggling either its position, rotation, scale, or all of these.
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback lets you trigger position, rotation and/or scale wiggles on an object equipped with a MMWiggle component, for the specified durations.")]
    [FeedbackPath("GameObject/Wiggle")]
    public class MMFeedbackWiggle : MMFeedback
    {
        public MMWiggle TargetWiggle;
        [Header("Position")]
        public bool WigglePosition = true;
        public float WigglePositionDuration;

        [Header("Rotation")]
        public bool WiggleRotation;
        public float WiggleRotationDuration;

        [Header("Scale")]
        public bool WiggleScale;
        public float WiggleScaleDuration;

        public bool _autoDetectPlayer=false;

        protected override void CustomInitialization(GameObject owner)
        {
            base.CustomInitialization(owner);

            //自动检测角色MMWiggle组件
            if(_autoDetectPlayer)               
            {
                Character player=CharacterManager.Instance.GetCharacter("Player1");
                MMWiggle wiggle= player.CharacterModel.GetComponent<MMWiggle>();
                if(wiggle!=null)
                {
                    TargetWiggle=wiggle;
                }
            }
        }

        
        /// <summary>
        /// On Play we trigger the desired wiggles
        /// </summary>
        /// <param name="position"></param>
        /// <param name="attenuation"></param>
        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1.0f)
        {
            if (Active && (TargetWiggle != null))
            {
                if (WigglePosition)
                {
                    TargetWiggle.WigglePosition(WigglePositionDuration);
                }
                if (WiggleRotation)
                {
                    TargetWiggle.WiggleRotation(WiggleRotationDuration);
                }
                if (WiggleScale)
                {
                    TargetWiggle.WiggleScale(WiggleScaleDuration);
                }
            }
        }
    }
}
