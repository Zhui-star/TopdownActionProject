using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    /// <summary>
    /// 物体隐藏所激活的事件
    /// </summary>
    public class ObjectDisableEvent : MonoBehaviour
    {
        [Header("物体隐藏之后是否会孵化新的物体")]
        public string objectPoolName;
        public Vector3 spawnerOffset;

        [Header("物体隐藏反馈")]
        public MMFeedbacks disableFeedbacks;

        private void OnEnable()
        {
            disableFeedbacks?.Initialization(this.gameObject);
        }

        private void OnDisable()
        {
            SpawnObject();

            disableFeedbacks?.PlayFeedbacks();
        }

        /// <summary>
        /// 隐藏物体之后孵化出新的物体
        /// </summary>
        void SpawnObject()
        {
            if (!string.IsNullOrEmpty(objectPoolName))
            {
                GameObject _object = PoolManagerV2.Instance.GetInst(objectPoolName);
                _object.transform.position = this.transform.position;
                _object.transform.rotation = this.transform.rotation;

            }
        }

    }


}
