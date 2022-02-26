using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 碰撞到物体给予一个力
    /// </summary>
    public class AddForceCollider : MonoBehaviourSimplify
    {

        public float force = 20;

        private void OnTriggerEnter(Collider other)
        {
            TopDownController3D controller = other.gameObject.GetComponent<TopDownController3D>();
            if(controller!=null)
            {
                Vector3 direction = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
                controller.Impact(direction, force);
            }
        }

        protected override void OnBeforeDestroy()
        {
        }

    
    }

}
