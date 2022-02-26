using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
namespace HTLibrary.Framework
{
    /// <summary>
    /// 非Mono类 的单例模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public  class Singleton<T> where T : Singleton<T>,new()
    {
        private static T mInstance;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new T();
                }

                return mInstance;
            }
        }

    }

}
