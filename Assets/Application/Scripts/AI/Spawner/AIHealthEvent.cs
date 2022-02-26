using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    [System.Serializable]
    public struct HealthEvent
    {
        public float _healthPercent;
        public UnityEvent _unityEvent;
    }
    /// <summary>
    /// 怪物血量事件
    /// </summary>
    public class AIHealthEvent : MonoBehaviour
    {
        public List<HealthEvent> _healthEvent = new List<HealthEvent>();

        private Health _health;

        int index = 0;
        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            _health.OnHit += TriggerHealthEvent;
        }

        private void OnDisable()
        {
            _health.OnHit -= TriggerHealthEvent;
        }

        private void Start()
        {
            InitialData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitialData()
        {
            index = 0;
        }


        /// <summary>
        /// 触发血量事件
        /// </summary>
        void TriggerHealthEvent()
        {
            bool getIndex = false;
         
            for(int i=index;i<_healthEvent.Count;i++)
            {
                if(_healthEvent[i]._healthPercent > (_health.CurrentHealth / (float)(_health.MaximumHealth)))
                {
                    index = i;
                    getIndex = true;
                }
            }

            if(!getIndex)
            {
                return;
            }

           
            _healthEvent[index]._unityEvent?.Invoke();
            getIndex = false;
            _healthEvent.RemoveAt(index);
        }
    }
}


