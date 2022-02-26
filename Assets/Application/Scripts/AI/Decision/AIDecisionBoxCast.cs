using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// Boxcast to detect target of decision
    /// </summary>
    public class AIDecisionBoxCast : AIDecision
    {
        public Vector3 _extension;
        public float _detectDistance;
        private Transform _model;
        public LayerMask _targetLayerMask;

        public override void Initialization()
        {
            base.Initialization();
            _model = GetComponent<Character>().CharacterModel.transform;

        }

        public override bool Decide()
        {
            bool hit = Physics.BoxCast(transform.position, _extension / 2,
            _model.forward, Quaternion.LookRotation(_model.forward), _detectDistance, _targetLayerMask);
            return hit;
        }


    }

}