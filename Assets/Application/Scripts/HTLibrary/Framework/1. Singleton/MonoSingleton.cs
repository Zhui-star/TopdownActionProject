using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Framework
{
    /// <summary>
    /// 单例模板类
    /// 继承MonoBehaviour
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T:MonoBehaviour
    {
        private static T m_instance;

        /// <summary>
        /// 如果单例为空
        /// 1. 搜索相应类型Object对象
        /// 2. 如果搜索不到Object对象 创造一个对象赋上T 组件
        /// </summary>
        public static T Instance
        {
            get
            {
                if(m_instance==null)
                {
                    m_instance = FindObjectOfType<T>();


                    if(FindObjectsOfType<T>().Length>1)
                    {
                        Debug.LogWarning("More than 1");
                        return m_instance;
                    }
                    
                        if (m_instance == null)
                        {
                            var instanceName = typeof(T).Name;
                            try
                            {
                                var instanceObj = new GameObject(instanceName);

                                m_instance = instanceObj.AddComponent<T>();
                            }
                            catch
                            {
                                return null;
                            }
                       
                        }
                 
                }
                return m_instance;
            }
        }

    }
}

