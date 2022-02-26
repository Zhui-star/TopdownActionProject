using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{

    public enum MessageDispatcherType
    {
        ShowText,
        MonoBehaviour
    }

    /// <summary>
    /// 消息分发机制管理类
    /// 初始化注册消息分发局
    /// 得到不同类型的消息分发对象
    /// </summary>
    public class MessageDispatcherManagerUtility :MonoSingleton<MessageDispatcherManagerUtility>
    {
        Dictionary<MessageDispatcherType, MessageDispatcherUtility> messageDipatcherDict = new Dictionary<MessageDispatcherType, MessageDispatcherUtility>();

        /// <summary>
        /// 注册消息分发部门
        /// </summary>
        private void Awake()
        {
            messageDipatcherDict.Add(MessageDispatcherType.ShowText, new MessageDispatcherUtility());
            messageDipatcherDict.Add(MessageDispatcherType.MonoBehaviour, new MessageDispatcherUtility());
        }

        /// <summary>
        /// 得到对应类型的消息部门
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MessageDispatcherUtility GetDispatcher(MessageDispatcherType type)
        {
            return messageDipatcherDict.TryGet<MessageDispatcherType, MessageDispatcherUtility>(type);
        }

    }
}

