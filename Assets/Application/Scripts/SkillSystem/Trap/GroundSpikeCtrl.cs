using MoreMountains.Tools;
using UnityEngine;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    /// <summary>
    /// Ground spike trap behaviour controller
    /// </summary>
    public class GroundSpikeCtrl : MonoBehaviour
    {
        /// <summary>
        /// Trap behaviour state
        /// </summary>
        public enum TrapState
        {
            Idle,
            InitialDelay,
            TrapUse,
            UseEnd
        }

        //state
        private MMStateMachine<TrapState> trapState=new MMStateMachine<TrapState>(null,false);

        [Header("Behaviour Time")]
        [SerializeField] private float trapUseTime;
        private float timer;

        [Space]
        [Header("Target")]
        [SerializeField] private LayerMask targetLayerMask;

        [Space]
        [Header("Particle")]
        [SerializeField] private ParticleSystem trapParticle;

        // damage area component
        private DamageOnTouch damageArea;
        private Collider damageCollider;

        [Space]
        [Header("Mark Area")]
        [SerializeField] private Light markLight;
        private float originLightIntensity;
        [SerializeField] private float flickSpeed;
        private float flickTimer;
        [SerializeField] private float flickDuration;


        /// <summary>
        /// Initial 
        /// </summary>
        private void Start()
        {
            InitialData();
            ReferenceComponent();
        }

        void InitialData()
        {
            trapState.ChangeState(TrapState.Idle);
            timer = 0;
            originLightIntensity = markLight.intensity;

        }

        void ReferenceComponent()
        {
            damageArea = GetComponent<DamageOnTouch>();
            damageCollider=GetComponent<BoxCollider>();
        }

        /// <summary>
        /// Update trap behaviour by state
        /// </summary>
        private void Update()
        {

            switch (trapState.CurrentState)
            {
                case TrapState.InitialDelay:
                    InitialDelay();
                    break;
                case TrapState.TrapUse:
                    TrapUse();
                    break;
                case TrapState.UseEnd:
                    UseEnd();
                    break;
            }
        }

        /// <summary>
        /// Start trap behaviour
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {

            if (trapState.CurrentState != TrapState.Idle || !damageArea) return;

            if (MMLayers.LayerInLayerMask(other.gameObject.layer, targetLayerMask))
            {
                trapState.ChangeState(TrapState.InitialDelay);
                timer = 0;
            }

        }

        /// <summary>
        /// Initial delay and make mark tips which flick light intensity
        /// </summary>
        void InitialDelay()
        {
            if (timer >= flickDuration)
            {
                trapState.ChangeState(TrapState.TrapUse);
                timer = 0;

                trapParticle.Play();
                markLight.intensity=0;

                Invoke("EnableDamageArea", 0.1f);
            }
            else
            {
                if (flickTimer>=flickSpeed)
                {           
                    markLight.intensity = markLight.intensity == 0 ? originLightIntensity : 0;
                    flickTimer=0;
                }

                flickTimer+=Time.deltaTime;
                timer += Time.deltaTime;
            }
        }

        /// <summary>
        /// Trap making damage 
        /// </summary>
        void TrapUse()
        {
            if (timer >= trapUseTime)
            {
                trapState.ChangeState(TrapState.UseEnd);
                timer = 0;
            }

            timer += Time.deltaTime;
        }

        /// <summary>
        /// Trap behaviour end
        /// </summary>
        void UseEnd()
        {
            timer = 0;
            flickTimer=0;
            markLight.intensity=originLightIntensity;
            trapState.ChangeState(TrapState.Idle);
            damageArea.enabled = false;
        }

        /// <summary>
        /// Enable damage on touch component after specific time 
        /// </summary>

        void EnableDamageArea()
        {
            damageArea.enabled = true;

            //Refrash collider
            damageCollider.enabled=false;
            damageCollider.enabled=true;
        }


    }

}
