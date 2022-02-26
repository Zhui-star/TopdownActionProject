
using System.Collections;
using System.Collections.Generic;

namespace HTLibrary.Utility
{
    /// <summary>
    /// Dictionary 静态扩展工具
    /// 方便查找Key所指定的Value
    /// </summary>
    public static class DictionaryUtility 
    {
        /// <summary>
        /// 寻找Key 对应的Value
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="Tvalue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Tvalue TryGet<Tkey,Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
        {
            Tvalue value;
            dict.TryGetValue(key, out value);

            return value;
        }
    }
}