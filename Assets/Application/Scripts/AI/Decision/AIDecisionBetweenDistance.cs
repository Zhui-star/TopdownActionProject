using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    /// <summary>
    /// 评估目标是否在2个距离之间
    /// </summary>
    public class AIDecisionBetweenDistance : AIDecision
    {
        public float firstDistance;
        public float secondDistance;

        /// <summary>
        /// 评估距离
        /// </summary>
        /// <returns></returns>
        protected bool EvaluateDistance()
        {
            if(_brain.Target==null)
            {
                return false;
            }

            float distance = Vector3.Distance(this.transform.position, _brain.Target.position);

            if(distance>=firstDistance&&distance<=secondDistance)
            {
                return true;
            }
            return false;
        }
        public override bool Decide()
        {
            return EvaluateDistance();
        }
    }
}

