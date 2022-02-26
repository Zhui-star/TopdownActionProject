
using UnityEngine;
using MoreMountains.Tools;
using DG.Tweening;
using System.Collections;
using MoreMountains.Feedbacks;
namespace HTLibrary.Application
{
    /// <summary>
    /// AI flash action , flash position is brain target position
    /// </summary>
    public class AIActionFlash : AIAction
    {
        public  MMFeedbacks _flashFeedBacks;
        private CharacterController _characterController;
        Vector3 _targetPosition;
        public LayerMask _boundaryMask;
        public Vector3 _targetPositionOffset;
        protected override void Initialization()
        {
            base.Initialization();
            _flashFeedBacks?.Initialization();
            _characterController=GetComponent<CharacterController>();
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            StartCoroutine(ProcessFlash());
        }
        public override void PerformAction()
        {

        }


        /// <summary>
        /// Flashing 
        /// </summary>
        /// <returns></returns>
        IEnumerator ProcessFlash()
        {
            _flashFeedBacks?.PlayFeedbacks();
            transform.DOScale(0, 0.25f);
            yield return new WaitForSeconds(0.25f);
            _targetPosition = _brain.Target.transform.position+ (Vector3.forward*_characterController.radius*_targetPositionOffset.z);
            Ray ray=new Ray(transform.position,_targetPosition);
            RaycastHit hit;
            if(Physics.Linecast(_targetPosition,transform.position,out hit,_boundaryMask))
            {
                _targetPosition=hit.point;
            }
            transform.position=_targetPosition;                       
            yield return new WaitForSeconds(0.25f);
            transform.DOScale(1, 0.25f);
        }

    }

}
