using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class HTInitialPosition : MonoBehaviour
    {
        public Vector3 InitialPosition;
        private bool isUpdate;

        private void OnEnable()
        {
            isUpdate = false;
        }
        private void Start()
        {
            isUpdate = false;
            transform.localPosition = InitialPosition;
        }
        
        private void Update()
        {
            if(isUpdate==false)
            {
                transform.localPosition = InitialPosition;
                isUpdate = true;
            }

        }
    }

}
