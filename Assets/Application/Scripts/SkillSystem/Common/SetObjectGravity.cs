using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 设置物体重力
    /// </summary>
    public class SetObjectGravity : MonoBehaviour
    {
        public float jumpValue = 3;
        public List<ObjectGravity> gravitys = new List<ObjectGravity>();

        private void OnEnable()
        {
            foreach(var temp in gravitys)
            {
                temp.SetJumpValue(jumpValue);
            }
        }
    }

}
