using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    public class FollowRotation : MonoBehaviour
    {
        private Transform rotation;

        public void SetRotation(Transform rotation)
        {
            this.rotation = rotation;
        }

        private void FixedUpdate()
        {
            if(rotation!=null)
            {
                this.transform.rotation = rotation.rotation;
            }
        }
    }

}
