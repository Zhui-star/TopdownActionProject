using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    public class HTRestoreHP : MonoBehaviour
    {
        public int value;

        private GameObject owner;

        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
            RestoreHP();
        }

        private void RestoreHP()
        {
            Health health = owner.GetComponent<Health>();

            if (health!=null)
            {
                health.CurrentHealth += value;
                if(health.CurrentHealth>health.MaximumHealth)
                {
                    health.CurrentHealth = health.MaximumHealth;
                }

                health.UpdateHealthBar(true);
            }
        }


    }

}
