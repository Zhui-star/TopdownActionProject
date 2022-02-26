using UnityEngine;
using HTLibrary.Framework;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

namespace HTLibrary.Application
{
    public class BulletFly : MonoBehaviour
    {
        private Rigidbody _rigidBody;

        public float Speed;

        public float Acceleration;


        Vector3 _movement;

        public float startTime = 0.2f;
        private float timer;
        private float originSpeed;

        private Vector3 _shootDirection;

        [Header("阻碍飞行的层")]
        public LayerMask layerMask;
        bool invaliableFly = false;
        [Space]
        [Header("Aim to shoot")]
        public bool _aimTarget = false;
        private Transform _destinationTransform;

        public string _markGameObjectName;

        public Vector3 _markGoOffset;

        private PoolManagerV2 _poolManager;
        public Vector3 _targetPositionOffeset;
        [MMInformation("When gameobject onEnable, it will present on top of target",
        MMInformationAttribute.InformationType.Info, false)]
        public bool _IsRestBulletPosition;
        [Space]
        [Header("Follow target")]

        public bool _IsFollowTarget;
        public float _smoothFollowSpeed = 5;
        Vector3 _targetDirection;
        Quaternion _targetQuaternion;
        public float _delayFollowOwner=0.2f;
        private float _startFollowTime;
        [Space]
        [Header("Target set")]
        public string _targetTag;
        public float _radius;
        private Transform _target;
        public LayerMask _targetMask;
        
        private void Awake()
        {
            _poolManager = PoolManagerV2.Instance;
        }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();

        }

        void Movement()
        {
            _movement = _aimTarget ? _shootDirection : transform.forward * (Speed / 10) * Time.deltaTime;
            if (_rigidBody != null)
            {
                _rigidBody.MovePosition(this.transform.position + _movement);
            }

            // We apply the acceleration to increase the speed
            Speed += Acceleration * Time.deltaTime;
        }

        private void FixedUpdate()
        {

            if (timer > startTime)
            {
                if (invaliableFly) return;
                Movement();
            }
            else
            {
                timer += Time.deltaTime;
            }

        }

        private void LateUpdate()
        {
            if(Time.time<_startFollowTime)return;
            FollowTarget(_target);
        }

        private void OnEnable()
        {
            originSpeed = Speed;
            SetAimDirectionStartPositionAndMarkAimArea();
            SetFollowTarget();
        }

        private void OnDisable()
        {
            timer = 0;
            invaliableFly = false;
            Speed = originSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (MMLayers.LayerInLayerMask(other.gameObject.layer, layerMask))
            {
                invaliableFly = true;
            }
        }

        /// <summary>
        /// this gameobject direction will forward target, smooth transition
        /// </summary>
        /// <param name="target"></param>
        void FollowTarget(Transform target)
        {
            if (_IsFollowTarget && target)
            {
                _targetDirection = target.position - transform.position;
                _targetDirection.y = 0;
                _targetQuaternion = Quaternion.LookRotation(_targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, _targetQuaternion, _smoothFollowSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Set target
        /// </summary>
        void SetFollowTarget()
        {
            if (_IsFollowTarget)
            {
                Collider[] targets = Physics.OverlapSphere(transform.position, _radius, _targetMask);

                if (targets.Length > 0)
                {
                    Transform shortestTarget = targets[0].transform;
                    foreach (var eachTarget in targets)
                    {
                        if (Vector3.Distance(eachTarget.transform.position, transform.position) <
                        Vector3.Distance(shortestTarget.position, transform.position))
                        {
                            shortestTarget = eachTarget.transform;
                        }
                    }

                    _target = shortestTarget;
                }
            _startFollowTime=Time.time+_delayFollowOwner;
            }
        }

        /// <summary>
        /// Set aim shoot direction, set start postion, mark aim area e.p red spot light range
        /// </summary>
        void SetAimDirectionStartPositionAndMarkAimArea()
        {
            // Is aim target when shoot
            if (_aimTarget)
            {
                GameObject[] _targets = GameObject.FindGameObjectsWithTag(_targetTag);
                _destinationTransform = _targets[0].GetComponent<Character>().CharacterModel.transform;

                if (_IsRestBulletPosition)
                {
                    Vector3 targetPostion = _targets[0].transform.position + _destinationTransform.transform.forward * _targetPositionOffeset.z;
                    targetPostion.y = transform.position.y;
                    transform.position = targetPostion;
                }

                _shootDirection =
                (_targets[0].transform.position + _destinationTransform.transform.forward * _targetPositionOffeset.z + Vector3.up * _targetPositionOffeset.y) - transform.position;
                _shootDirection = _shootDirection.normalized;
                if (!string.IsNullOrEmpty(_markGameObjectName))
                {
                    GameObject _markGo = _poolManager.GetInst(_markGameObjectName);
                    _markGo.transform.position = _targets[0].transform.position + _destinationTransform.transform.forward * _targetPositionOffeset.z;
                    _markGo.transform.position += _markGoOffset;
                }

            }
        }
    }

}
