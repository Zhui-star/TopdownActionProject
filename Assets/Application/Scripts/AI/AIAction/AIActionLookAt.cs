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
    /// AI看向一个目标
    /// </summary>
    public class AIActionLookAt : AIAction
    {
        CharacterOrientation3D oritentation3D;
        CharacterHandleWeapon handleWeapon;
        
        protected override void Initialization()
        {
            base.Initialization();
            oritentation3D = GetComponent<CharacterOrientation3D>();
            handleWeapon = GetComponent<CharacterHandleWeapon>();
        }

        public override void PerformAction()
        {
            LookAtTarget();
          
        }

        /// <summary>
        ///看向敌人
        /// </summary>
        void LookAtTarget()
        {
            oritentation3D.HTForceRotation(_brain.Target);
        }

        public override void OnExitState()
        {
            base.OnExitState();
            oritentation3D.CharacterRotationAuthorized = true;

            if(handleWeapon==null)
                return;
            WeaponLaserSight laserSight = handleWeapon.CurrentWeapon.gameObject.GetComponent<WeaponLaserSight>();

            if (laserSight == null) return;
            LineRenderer lineRender = handleWeapon.CurrentWeapon.gameObject.gameObject.GetComponent<LineRenderer>();
            lineRender.enabled = false;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            oritentation3D.CharacterRotationAuthorized = false;

            WeaponLaserSight laserSight=  handleWeapon.CurrentWeapon.gameObject.GetComponent<WeaponLaserSight>();

            if (laserSight == null) return;

            if(laserSight.PerformRaycast==false)
            {
                laserSight.PerformRaycast = true;
            }
            else
            {
                LineRenderer lineRender= handleWeapon.CurrentWeapon.gameObject.gameObject.GetComponent<LineRenderer>();
                lineRender.enabled = true;

            }
        }
    }

}
