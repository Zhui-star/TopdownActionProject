using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    public class AIRoar : AIAction
    {
        CharacterOrientation3D oritentation3D;
        Animator anim;
        public AudioClip roarClip;
        public float relayPlayClipTime = 0.4f;
        protected override void Initialization()
        {
            base.Initialization();
            oritentation3D = GetComponent<CharacterOrientation3D>();
            anim = GetComponent<Character>().CharacterModel.GetComponent<Animator>();
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            oritentation3D.CharacterRotationAuthorized = false;
            oritentation3D.HTForceRotation(_brain.Target);
            anim.SetTrigger("Roar");
            Invoke("PlaySound", relayPlayClipTime);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            oritentation3D.CharacterRotationAuthorized = true;

        }

        void PlaySound()
        {

            if (roarClip != null)
            {
                SoundManager.Instance.PlaySound(roarClip, transform.position, false);
            }
        }


        public override void PerformAction()
        {
           
        }
    }

}
