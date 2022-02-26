using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    public class HTInvincible : MonoBehaviour
    {
        private Health owner;

        private CharacterController _characterController;
        private Collider _collider;

        private void OnDisable()
        {
            DamageEnable();
        }

        public void SetOwner(Health health)
        {
            this.owner = health;
            _characterController = owner.GetComponent<CharacterController>();
            _collider = owner.GetComponent<Collider>();

            DamageaDisable();
        }

        private void DamageaDisable()
        {
            if(owner!=null)
            {
                //this.owner.DamageDisabled();
                _characterController.enabled = false;
                _collider.enabled = false;
            }
        }

        private void DamageEnable()
        {
            if (owner != null)
            {
                //this.owner.DamageEnabled();
                _characterController.enabled = true;
                _collider.enabled = true;
            }
        }
    }

}
