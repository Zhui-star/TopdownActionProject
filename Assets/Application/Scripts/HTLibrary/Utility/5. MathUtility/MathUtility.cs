using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 数学运算工具
    /// </summary>
    public class MathUtility : MonoBehaviour
    {
        /// <summary>
        /// 当前概率是否有中奖
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool Percent(int percent)
        {
            int randomPercent = UnityEngine.Random.Range(0, 100);
            return randomPercent < percent;
        }

        /// <summary>
        /// 返回数组中的随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T GetRandomValueFrom<T>(params T[] values)
        {
            return values[UnityEngine.Random.Range(0, values.Length)];
        }
    }

}
