using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    public class HTSetLineRender : MonoBehaviour
    {
        public float acceleration;
        public float speed;
        Vector3 originScal;
        float originSpeed;
        private void OnEnable()
        {
            originScal = this.transform.localScale;
            originSpeed = speed;
        }
        public void SetBeam()
        {
            transform.localScale = new Vector3(Time.deltaTime *speed* this.transform
                .localScale.x, Time.deltaTime *speed* this.transform.localScale.y, this.transform.localScale.z);
             speed += Time.deltaTime * acceleration;
        }

        private void Update()
        {
            SetBeam();
        }

        private void OnDisable()
        {
            this.transform.localScale = originScal;
            speed = originSpeed;
        }

    }

}
