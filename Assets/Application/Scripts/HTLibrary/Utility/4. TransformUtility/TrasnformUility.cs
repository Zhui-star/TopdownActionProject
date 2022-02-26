using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Utility
{
    /// <summary>
    /// Transform的扩展工具
    /// </summary>
    public static class TrasnformUility
    {
        /// <summary>
        /// Transform 归一化
        /// </summary>
        /// <param name="transform"></param>
      public static void Identity(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// 设置X轴的值
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        public static void SetLocalPosX(this Transform transform, float x)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置Y轴的值
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        public static void SetLocalPosY(this Transform transform, float y)
        {
            var localPos = transform.localPosition;
            localPos.y = y;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置Z轴的值
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        public static void SetLocalPosZ(this Transform transform, float z)
        {
            var localPos = transform.localPosition;
            localPos.z = z;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置XY轴的值
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SetLocalPosXY(this Transform transform, float x, float y)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            localPos.y = y;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置XZ轴的值
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public static void SetLocalPosXZ(this Transform transform, float x, float z)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            localPos.z = z;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 设置YZ轴的值
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalPosYZ(this Transform transform, float y, float z)
        {
            var localPos = transform.localPosition;
            localPos.y = y;
            localPos.z = z;
            transform.localPosition = transform.localPosition;
        }

        /// <summary>
        /// 设置XYZ轴的值
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public static void SetLocalPosXYZ(this Transform transform, float x, float y,float z)
        {
            var localPos = transform.localPosition;
            localPos.y = y;
            localPos.z = z;
            localPos.x = x;
            transform.localPosition = localPos;
        }

        /// <summary>
        /// 添加孩子物体
        /// </summary>
        /// <param name="parentTrans"></param>
        /// <param name="childTrans"></param>
        public static void AddChild(this Transform parentTrans, Transform childTrans)
        {
            childTrans.SetParent(parentTrans);
        }
    }

}
