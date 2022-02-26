using UnityEngine;
using UnityEngine.Animations.Rigging;
using HTLibrary.Utility;
using System.Collections;
using NaughtyAttributes;
namespace HTLibrary.Application
{
    /// <summary>
    /// Using human type model to look at player character by control head rig
    /// </summary>
    public class HeadTracking : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        private Transform _characterTrs;
        public Rig _headRig;
        public Transform _target;

        public float _radius = 10.0f;
        float _radiusSqr;
        public float _retargetSpeed = 5f;

        [Range(0,360)]
        [SerializeField]
        private int _limitAngle;
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            InitialComponents();
            _radiusSqr = Mathf.Pow(_radius, 2);
        }

        void InitialComponents()
        {
            _characterTrs = GameObject.FindGameObjectWithTag(Tags.Player).transform;
        }

        void Update() => HeadLookAtTransfrom(_characterTrs);

        /// <summary>
        /// Every frame to detect player position and radius
        /// </summary>
        /// <param name="targetTransform"></param>
        void HeadLookAtTransfrom(Transform targetTransform)
        {
            if (!targetTransform) return;

            Transform tracking = null;

            Vector3 delta = targetTransform.position - transform.position;
             
             
            if(delta.sqrMagnitude<_radiusSqr)
            {
                Vector3 targetDirction = delta;
                targetDirction.y= transform.position.y;
                float angle = Vector3.Angle(transform.forward, targetDirction);
                if(angle<_limitAngle)
                {
                    tracking = targetTransform;
                }   
            }

            float rigWeight = 0;
            Vector3 targetPos = transform.position + (transform.forward * 2);

            if(tracking)
            {
                targetPos= tracking.position;
                targetPos.y = _target.position.y;
                rigWeight = 1;
            }

            _target.position = Vector3.Lerp(_target.position, targetPos, Time.deltaTime * _retargetSpeed);
            _headRig.weight = Mathf.SmoothStep(_headRig.weight, rigWeight, Time.deltaTime * 2);
           
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }


    }

}
