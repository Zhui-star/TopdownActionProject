using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

namespace HTLibrary.Application
{
    public class ChangeCharacterTechingInteractive : ButtonActivated
    {


        protected override void OnTriggerEnter(Collider collidingObject)
        {
            base.OnTriggerEnter(collidingObject);
        }

        protected override void OnTriggerExit(Collider collidingObject)
        {
            base.OnTriggerExit(collidingObject);
        }

        /// <summary>
        /// Trigger Interactive
        /// </summary>
        public override void TriggerButtonAction()
        {
            base.TriggerButtonAction();
            UIManager.Instance.PushPanel(UIPanelType.CharacterSelectPanel);
        }
    }
}

