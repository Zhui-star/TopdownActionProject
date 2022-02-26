using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    public class ParticleSystemEvent : MonoBehaviour
    {
        private ParticleSystem system;
        private void Awake()
        {
            system = GetComponent<ParticleSystem>();
        }
        private void OnEnable()
        {
          //  GameManager.Instance.PausedGameEvent += PausedGameEvent;
        }

        private void OnDisable()
        {
            //if(GameManager.Instance!=null)
            //{
            //    GameManager.Instance.PausedGameEvent -= PausedGameEvent;
            //}
        }

        void PausedGameEvent(bool pause)
        {
            if(pause)
            {
                system.Pause(true);
            }
            else
            {
                system.Play(true);
            }
            
        }
    }
}

