using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    public enum PatnerType
    {
        None,
        DPS,
        Tank,
        Support
    }

    /// <summary>
    /// 伙伴命令
    /// </summary>
    public class PatnerCommender : MonoBehaviourSimplify
    {
        List<PatnerController> _patnerControllers = new List<PatnerController>();

        SaveManager _saveManager;

        private void Awake()
        {
            _saveManager = SaveManager.Instance;
        }

        private void OnEnable()
        {
            EventTypeManager.AddListener<PatnerController>(HTEventType.AddPatner, AddPatnerController);
            EventTypeManager.AddListener<PatnerController>(HTEventType.RemovePatner, RemovePatnerController);
        }

        

        private void Start()
        { 
            EventTypeManager.AddListener<Transform>(HTEventType.PatnerCommend_ResetTarget, ResetTarget);
        }

        /// <summary>
        /// 重置命令
        /// </summary>
        /// <param name="newTarget"></param>

        void ResetTarget(Transform newTarget)
        {

            foreach (var _patner in _patnerControllers)
            {
                _patner.ResetTarget(newTarget);
            }
        }

        protected override void OnBeforeDestroy()
        {
            EventTypeManager.RemoveListener<Transform>(HTEventType.PatnerCommend_ResetTarget, ResetTarget);
        }

        /// <summary>
        /// 添加伙伴控制器
        /// </summary>
        /// <param name="_controller"></param>
        private void AddPatnerController(PatnerController _controller)
        {
            _patnerControllers.Add(_controller);
        }

        /// <summary>
        /// 移除伙伴控制器
        /// </summary>
        /// <param name="_controller"></param>
        private void RemovePatnerController(PatnerController _controller)
        {
            _patnerControllers.Remove(_controller);
        }

        /// <summary>
        /// 保存伙伴数据
        /// </summary>
        void SavePatnerData()
        {
            string dataInfo = "";
            for (int i = 0; i < _patnerControllers.Count; i++)
            {
                Health health = _patnerControllers[i].GetComponent<Health>();
                if (health != null)
                {
                    dataInfo += i + "," + health.CurrentHealth + ":";
                }
            }

            Debug.Log("伙伴信息: " + dataInfo);

            PlayerPrefs.SetString(_saveManager.LoadGameID+ Consts.PatnerData, dataInfo);

        }

        /// <summary>
        /// 加载伙伴数据
        /// </summary>
        void LoadPatnerData()
        {
            if(!PlayerPrefs.HasKey(_saveManager.LoadGameID+Consts.PatnerData))
            {
                return;
            }

            string patnerDatasStr = PlayerPrefs.GetString(_saveManager.LoadGameID+Consts.PatnerData);
            
            if(string.IsNullOrEmpty(patnerDatasStr))
            {
                return;
            }

            string[] patnerDatas= patnerDatasStr.Split(':');
            int i = 0;
            foreach(var patnerDataStr in patnerDatas)
            {
                if(string.IsNullOrEmpty(patnerDataStr))
                {
                    continue;
                }

                string[] patnerData = patnerDataStr.Split(',');
                int currentHealth = int.Parse(patnerData[1]);
                Health health = _patnerControllers[i].GetComponent<Health>();
                health.RestoreHPAnim = false;
                health.CurrentHealth = currentHealth;
                i++;
            }
             
        }

        private void OnDisable()
        {
            EventTypeManager.RemoveListener<PatnerController>(HTEventType.AddPatner, AddPatnerController);
            EventTypeManager.RemoveListener<PatnerController>(HTEventType.RemovePatner, RemovePatnerController);
        }
    }

}
