using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

namespace HTLibrary.Application
{
    public class BlackHole : MonoBehaviour
    {
        public float force;

        public LayerMask layerMask;

        public float maxDistance;

        private void OnTriggerStay(Collider other)
        {
            if (MMLayers.LayerInLayerMask(other.gameObject.layer, layerMask))
            {
                Attract(other);
            }
        }

        private void Attract(Collider other)
        {
            Vector3 dir = this.transform.position - other.transform.position;

            dir = new Vector3(dir.x, 0, dir.z);

            if (Vector3.Distance(other.transform.position, transform.position) > maxDistance)
            {
                TopDownController characterController = other.gameObject.GetComponent<TopDownController>();

                if (characterController != null)
                {
                    characterController.Impact(dir.normalized, force);
                }
            }


        }
    }

}
