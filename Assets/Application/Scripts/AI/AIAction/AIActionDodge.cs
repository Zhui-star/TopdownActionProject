using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// Dodge AI action It will dodge some bullet or character skill
    /// </summary>
    public class AIActionDodge : AIAction
    {
        public float _dodgeDistance;
        public float _dodgeDuration;
        private Vector3 _newDestination;
        public AnimationCurve _dodgeCurve;
        int _randomDirection;
        Transform _model;
        float _dodgeTimer;
        Vector3 _originPosition;
        TopDownController3D _controller;
        protected override void Initialization()
        {
            base.Initialization();
            _model = GetComponent<Character>().CharacterModel.transform;
            _controller = GetComponent<TopDownController3D>();
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            _randomDirection = Random.Range(0, 2);
            _newDestination = (_randomDirection == 0 ? _model.transform.right * _dodgeDistance : 
            -_model.transform.right * _dodgeDistance)+transform.position;
            _dodgeTimer = 0;
            _originPosition = transform.position;
        }

        public override void PerformAction()
        {
            if (_dodgeTimer < _dodgeDuration)
            {
                Vector3 newDestination = Vector3.Lerp(_originPosition, _newDestination, _dodgeCurve.Evaluate(_dodgeTimer / _dodgeDuration));
                _dodgeTimer += Time.deltaTime;
                _controller.MovePosition(newDestination);
            }
        }

    }
}

