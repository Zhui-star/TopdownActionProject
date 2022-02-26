using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 当物体消失时自动回到原位
    /// </summary>
    public class HTAutoResetPosition : MonoBehaviour
    {
        private Vector3 originalPosition;

        private void OnEnable()
        {
            originalPosition = transform.localPosition;
           
        }

        private void OnDisable()
        {
            transform.localPosition = originalPosition;
        }
    }

}
