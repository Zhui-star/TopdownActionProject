using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Application;

namespace HTLibrary.Application
{
    public class DelayHideComponent : MonoBehaviour
    {
        public float time;
        public MonoBehaviour comoponent;
        public Collider _collider;
        private void OnEnable()
        {
            Invoke("HideComponent", time);
        }

        private void OnDisable()
        {
            OpenComponent();
        }

        private void HideComponent()
        {
            comoponent.enabled = false;

            if(_collider!=null)
            {
                _collider.enabled = false;
            }           
        }

        private void OpenComponent()
        {
            comoponent.enabled = true;

            if (_collider != null)
            {
                _collider.enabled = true;
            }
        }
    }

}
