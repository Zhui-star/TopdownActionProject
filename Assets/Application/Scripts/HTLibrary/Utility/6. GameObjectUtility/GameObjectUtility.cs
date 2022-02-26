using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObjectUtility扩展工具
/// 简化显示和隐藏
/// </summary>
namespace HTLibrary.Utility
{
    public static class GameObjectUtility 
    {
        public static void Show(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        public static void Hide(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        public static void Show(this Transform transform)
        {
            transform.gameObject.SetActive(true);
        }

        public static void Hide(this Transform transform)
        {
            transform.gameObject.SetActive(false);
        }

        public static void Show(this MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(true);
        }

        public static void Hide(this MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(false);
        }
    }

}
