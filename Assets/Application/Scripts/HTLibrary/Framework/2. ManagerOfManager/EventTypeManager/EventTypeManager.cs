using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Framework
{
    /// <summary>
    /// 事件管理局
    /// 1. 管理事件派发枚举管理
    /// To do:其他事件管理
    /// </summary>
    public enum HTEventType
    {
        ShowText,
        CountAI, //技术当前场景的AI怪
        Furious, //狂怒天赋
        AddIdleDefence, //招架天赋
        SuckBlood, //吸血天赋
        PatnerCommend_ResetTarget,
        ShowTabInfo,//Tab查看现在的信息面板
        GreatSwordLevelPassiveSkill,
        HideGreatPlayer,
        GodGolemController,//神明在上！
        MarkArrow,
        EndWaitSkill,
        SummonerPassiveSkill, //召唤师被动
        AddPatner,//添加伙伴
        RemovePatner,//移除伙伴
        StartAccumlate,//开始蓄力
        StopAccumlate,//停止蓄力
        CreatGhostShadow,//产生鬼隐
        HPVerticalMove//血条上升 比如被释放技能时被某些物体所遮挡

    }

    public class EventTypeManager
    {

        private static Dictionary<HTEventType, Delegate> m_EventTable = new Dictionary<HTEventType, Delegate>();

        private static void OnListenerAdding(HTEventType HTEventType, Delegate callBack)
        {
            if (!m_EventTable.ContainsKey(HTEventType))
            {
                m_EventTable.Add(HTEventType, null);
            }
            Delegate d = m_EventTable[HTEventType];
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，" +
                    "当前事件所对应的委托是{1}，要添加的委托类型为{2}", HTEventType, d.GetType(), callBack.GetType()));
            }
        }
        private static void OnListenerRemoving(HTEventType HTEventType, Delegate callBack)
        {
            if (m_EventTable.ContainsKey(HTEventType))
            {
                Delegate d = m_EventTable[HTEventType];
                if (d == null)
                {
                    throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", HTEventType));
                }
                else if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，" +
                        "当前委托类型为{1}，要移除的委托类型为{2}", HTEventType, d.GetType(), callBack.GetType()));
                }
            }
            else
            {
                //  throw new Exception(string.Format("移除监听错误：没有事件码{0}", HTEventType));
            }
        }
        private static void OnListenerRemoved(HTEventType HTEventType)
        {
            if (m_EventTable[HTEventType] == null)
            {
                m_EventTable.Remove(HTEventType);
            }
        }
        //no parameters
        public static void AddListener(HTEventType HTEventType, CallBack callBack)
        {
            OnListenerAdding(HTEventType, callBack);
            m_EventTable[HTEventType] = (CallBack)m_EventTable[HTEventType] + callBack;
        }
        //Single parameters
        public static void AddListener<T>(HTEventType HTHTEventType, CallBack<T> callBack)
        {
            OnListenerAdding(HTHTEventType, callBack);
            m_EventTable[HTHTEventType] = (CallBack<T>)m_EventTable[HTHTEventType] + callBack;
        }
        //two parameters
        public static void AddListener<T, X>(HTEventType HTHTEventType, CallBack<T, X> callBack)
        {
            OnListenerAdding(HTHTEventType, callBack);
            m_EventTable[HTHTEventType] = (CallBack<T, X>)m_EventTable[HTHTEventType] + callBack;
        }
        //three parameters
        public static void AddListener<T, X, Y>(HTEventType HTHTEventType, CallBack<T, X, Y> callBack)
        {
            OnListenerAdding(HTHTEventType, callBack);
            m_EventTable[HTHTEventType] = (CallBack<T, X, Y>)m_EventTable[HTHTEventType] + callBack;
        }
        //four parameters
        public static void AddListener<T, X, Y, Z>(HTEventType HTHTEventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerAdding(HTHTEventType, callBack);
            m_EventTable[HTHTEventType] = (CallBack<T, X, Y, Z>)m_EventTable[HTHTEventType] + callBack;
        }
        //five parameters
        public static void AddListener<T, X, Y, Z, W>(HTEventType HTHTEventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerAdding(HTHTEventType, callBack);
            m_EventTable[HTHTEventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[HTHTEventType] + callBack;
        }

        //no parameters
        public static void RemoveListener(HTEventType HTEventType, CallBack callBack)
        {
            OnListenerRemoving(HTEventType, callBack);
            m_EventTable[HTEventType] = (CallBack)m_EventTable[HTEventType] - callBack;
            OnListenerRemoved(HTEventType);
        }
        //single parameters
        public static void RemoveListener<T>(HTEventType HTEventType, CallBack<T> callBack)
        {
            OnListenerRemoving(HTEventType, callBack);
            m_EventTable[HTEventType] = (CallBack<T>)m_EventTable[HTEventType] - callBack;
            OnListenerRemoved(HTEventType);
        }
        //two parameters
        public static void RemoveListener<T, X>(HTEventType HTEventType, CallBack<T, X> callBack)
        {
            OnListenerRemoving(HTEventType, callBack);
            m_EventTable[HTEventType] = (CallBack<T, X>)m_EventTable[HTEventType] - callBack;
            OnListenerRemoved(HTEventType);
        }
        //three parameters
        public static void RemoveListener<T, X, Y>(HTEventType HTEventType, CallBack<T, X, Y> callBack)
        {
            OnListenerRemoving(HTEventType, callBack);
            m_EventTable[HTEventType] = (CallBack<T, X, Y>)m_EventTable[HTEventType] - callBack;
            OnListenerRemoved(HTEventType);
        }
        //four parameters
        public static void RemoveListener<T, X, Y, Z>(HTEventType HTEventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerRemoving(HTEventType, callBack);
            m_EventTable[HTEventType] = (CallBack<T, X, Y, Z>)m_EventTable[HTEventType] - callBack;
            OnListenerRemoved(HTEventType);
        }
        //five parameters
        public static void RemoveListener<T, X, Y, Z, W>(HTEventType HTEventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerRemoving(HTEventType, callBack);
            m_EventTable[HTEventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[HTEventType] - callBack;
            OnListenerRemoved(HTEventType);
        }


        //no parameters
        public static void Broadcast(HTEventType HTEventType)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(HTEventType, out d))
            {
                CallBack callBack = d as CallBack;
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", HTEventType));
                }
            }
        }
        //single parameters
        public static void Broadcast<T>(HTEventType HTEventType, T arg)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(HTEventType, out d))
            {
                CallBack<T> callBack = d as CallBack<T>;
                if (callBack != null)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", HTEventType));
                }
            }
        }
        //two parameters
        public static void Broadcast<T, X>(HTEventType HTEventType, T arg1, X arg2)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(HTEventType, out d))
            {
                CallBack<T, X> callBack = d as CallBack<T, X>;
                if (callBack != null)
                {
                    callBack(arg1, arg2);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", HTEventType));
                }
            }
        }
        //three parameters
        public static void Broadcast<T, X, Y>(HTEventType HTEventType, T arg1, X arg2, Y arg3)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(HTEventType, out d))
            {
                CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", HTEventType));
                }
            }
        }
        //four parameters
        public static void Broadcast<T, X, Y, Z>(HTEventType HTEventType, T arg1, X arg2, Y arg3, Z arg4)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(HTEventType, out d))
            {
                CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", HTEventType));
                }
            }
        }
        //five parameters
        public static void Broadcast<T, X, Y, Z, W>(HTEventType HTEventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(HTEventType, out d))
            {
                CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", HTEventType));
                }
            }
        }

        /// <summary>
        /// 事件表是否包含当前类型事件
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static bool ContainHTEventType(HTEventType ht)
        {
            return m_EventTable.ContainsKey(ht);
        }
    }
}

