using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HTLibrary.Utility;

namespace HTLibrary.Framework
{
    public enum SimplifyType
    {
        Test
    }
    /// <summary>
    /// MonoBehaviour 简便版使用工具
    /// 1. 添加自动延时功能
    /// 2. 添加事件分发
    /// </summary>
    public abstract class MonoBehaviourSimplify : MonoBehaviour
    {
        #region Delay
        /// <summary>
        /// 延时功能
        /// 1. 延时回调
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="onFinished"></param>
        public void Delay(float seconds, Action onFinished)
        {
            StartCoroutine(DelayCoroutine(seconds, onFinished));
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="onFinished"></param>
        /// <returns></returns>
        private IEnumerator DelayCoroutine(float seconds, Action onFinished)
        {
            yield return new WaitForSeconds(seconds);
            onFinished();
        }
        #endregion


        /// <summary>
        /// 注册MonoBehaviour事件
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="onMsgReceived"></param>
        public void RegisterMsg(object msgName, Action<object> onMsgReceived)
        {
            MessageDispatcherManagerUtility.Instance.GetDispatcher(MessageDispatcherType.MonoBehaviour).Register(msgName, onMsgReceived);
        }

        /// <summary>
        /// 发送关于MonoBehaviour 事件
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="data"></param>
        public void SendMsg(object msgName, object data)
        {
            MessageDispatcherManagerUtility.Instance.GetDispatcher(MessageDispatcherType.MonoBehaviour).Send(msgName, data);
        }

        /// <summary>
        /// 下载关于MonoBehaviour事件
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="onMsgReceived"></param>
        public void UnRegisterMsg(object msgName, Action<object> onMsgReceived)
        {
            MessageDispatcherManagerUtility.Instance.GetDispatcher(MessageDispatcherType.MonoBehaviour).UnRegister(msgName, onMsgReceived);
        }

        protected abstract void OnBeforeDestroy();

        protected virtual void OnDestroy()
        {
            OnBeforeDestroy();
        }

    }

}
