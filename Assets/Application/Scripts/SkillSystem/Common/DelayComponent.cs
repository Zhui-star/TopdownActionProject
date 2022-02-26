using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.Feedbacks;
namespace HTLibrary.Application
{
    public class DelayComponent : MonoBehaviour
    {
        public MonoBehaviour component;
        public Collider _collider;
        public float time;
        public MMFeedbacks feedbacks;
        private void OnEnable()
        {
            Invoke("OpenComponent", time);
        }

        private void Start()
        {
            feedbacks?.Initialization();
        }

        private void OnDisable()
        {
            if (component != null)
            {
                component.enabled = false;
            }
            if (_collider != null)
            {
                _collider.enabled = false;
            }
        }

        private void OpenComponent()
        {
            if(component!=null)
            {
                component.enabled = true;
            }
            if(_collider!=null)
            {
                _collider.enabled = true;
            }
            feedbacks?.PlayFeedbacks(transform.position);
        }
    }

}
