using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;

namespace HTLibrary.Application
{
    public class FollowOwner : MonoBehaviour
    {
        private Transform _owner;
        public Transform owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }

        [Header("Wheather stop current skill when forzen or frezze character state")]
        [SerializeField]
        private bool _stopReleaseWhenFrozen = false;

        private void OnEnable()
        {
            if (owner != null && _stopReleaseWhenFrozen)
            {
                owner.GetComponent<CharacterFunctionSwitch>().StopSkillReleaseHandler += DisactiveGO;
            }
        }

        private void OnDisable()
        {
            if (owner != null && _stopReleaseWhenFrozen)
            {
                owner.GetComponent<CharacterFunctionSwitch>().StopSkillReleaseHandler -= DisactiveGO;
            }
        }

        public void SetOwner(Transform owenr)
        {
            this.owner = owenr;
            offset = this.owner.position - transform.position;
        }

        Vector3 offset;

        private void FixedUpdate()
        {
            if (owner == null) return;
            this.transform.position = owner.position - offset;
        }

        /// <summary>
        /// Straight disactive go
        /// </summary>
        private void DisactiveGO()
        {
            this.gameObject.SetActive(false);
        }
    }

}
