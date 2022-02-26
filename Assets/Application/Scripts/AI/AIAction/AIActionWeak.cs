using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 怪物的虚弱行为
    /// </summary>
    public class AIActionWeak : AIAction
    {
        private Character character;
        public GameObject weakEffect;
        private CharacterOrientation3D orientation;

        protected override void Initialization()
        {
            base.Initialization();
            character = GetComponent<Character>();
            orientation = GetComponent<CharacterOrientation3D>();

        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            weakEffect.SetActive(true);
            character._animator.SetBool("Weak", true);
            orientation.CharacterRotationAuthorized = false;
            orientation.HTForceRotation(_brain.Target);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            weakEffect.SetActive(false);
            character._animator.SetBool("Weak", false);
        }

        public override void PerformAction()
        {
            orientation.CharacterRotationAuthorized = true;
        }
    }

}
