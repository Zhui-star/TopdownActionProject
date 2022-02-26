using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 消息机制封装工具
    /// 注册消息
    /// 卸载消息
    /// 发送消息给指定接受者
    /// </summary>
    public class MessageDispatcherUtility
    {
        static Dictionary<object, Action<object>> mRegisteredMsgs = new Dictionary<object, Action<object>>();

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="onMsgReceived"></param>

        public  void Register(object msgName, Action<object> onMsgReceived)
        {
            if (!mRegisteredMsgs.ContainsKey(msgName))
            {
                mRegisteredMsgs.Add(msgName, _ => { });
            }

            mRegisteredMsgs[msgName] += onMsgReceived;
        }

        /// <summary>
        /// 卸载全部消息
        /// </summary>
        /// <param name="msgName"></param>
        public  void UnRegisterAll(object msgName)
        {
            mRegisteredMsgs.Remove(msgName);
        }

        /// <summary>
        /// 卸载消息
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="onMsgReceived"></param>
        public  void UnRegister(object msgName, Action<object> onMsgReceived)
        {
            if (mRegisteredMsgs.ContainsKey(msgName))
            {
                mRegisteredMsgs[msgName] -= onMsgReceived;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="data"></param>
        public  void Send(object msgName, object data)
        {
            if (mRegisteredMsgs.ContainsKey(msgName))
            {
                mRegisteredMsgs[msgName](data);
            }
        }
    }

}
