using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Utility
{
    public class AnimatorSetUp
    {
        public float speedDampTime = 0.1f;
        public float angularSpeedDampTime = 0.7f;
        public float angleResponseTime = 1f;

        private Animator anim;
        HashIDs hash;

        public AnimatorSetUp(Animator anim)
        {
            this.anim = anim;
            this.hash = HashIDs.Instance;

        }

        public void Setup(float speed, float angle)
        {
            float angularSpeed = angle / angleResponseTime;

            anim.SetFloat(hash.speedFloat, speed, speedDampTime, Time.deltaTime);
            anim.SetFloat(hash.angularSpeedFloat, angularSpeed, angularSpeedDampTime, Time.deltaTime);

        }
    }
}
