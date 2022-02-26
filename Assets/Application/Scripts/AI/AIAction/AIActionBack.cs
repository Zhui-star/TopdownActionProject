using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// AI 害怕的往后跑
    /// </summary>
    public class AIActionBack : AIAction
    {
        protected CharacterMovement _characterMovement;
        private Transform _target;
        public LayerMask _targerLayerMask;
        public float _radius = 1.0f;
        Vector2 _forceDirection;

        public float _cooldownDuration = 1.0f;
        private float _targetCoolTimer = 0;
        Collider[] colliders;

        protected override void Initialization()
        {
            base.Initialization();
            _characterMovement = this.gameObject.GetComponent<CharacterMovement>();
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
        }

        public override void OnExitState()
        {
            base.OnExitState();
            _characterMovement.SetHorizontalMovement(0f);
            _characterMovement.SetVerticalMovement(0f);

        }
        // GameObject go;
        public override void PerformAction()
        {
            _forceDirection = Force();
            _characterMovement.SetMovement(_forceDirection);
        }
    
        /// <summary>
        /// 逃走方向
        /// </summary>
        /// <returns></returns>
        private Vector2 Force()
        {

            Vector2 targetDirection = Vector2.zero;


            if (_targetCoolTimer == 0 || Time.time >= _targetCoolTimer)
            {
                colliders= Physics.OverlapSphere(transform.position, _radius, _targerLayerMask);
                _targetCoolTimer=Time.time+_targetCoolTimer;  //10  5   15
            }


            if (colliders != null && colliders.Length > 0)
            {
                _target = colliders[0].transform;
                targetDirection = this.transform.position - _target.position;
            }

            return targetDirection.normalized;
        }


        /// <summary>
        /// 绘画出圈圈
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }

}
