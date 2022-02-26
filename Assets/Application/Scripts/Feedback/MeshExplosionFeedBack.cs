using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
namespace HTLibrary.Application
{
    /// <summary>
    /// 物体网格爆炸反馈
    /// </summary>
    [FeedbackPath("HTLibrary/MeshExplosion")]
    public class MeshExplosionFeedBack : MMFeedback
    {
        public MeshExploder _meshExploder;

        protected override void CustomPlayFeedback(Vector3 position, float attenuation = 1)
        {
            TriggerExplosion();
        }

        /// <summary>
        /// 触发爆炸
        /// </summary>
        void TriggerExplosion()
        {
            _meshExploder.Explode();
        }
    }

}
