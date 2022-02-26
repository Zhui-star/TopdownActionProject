using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 合成材料
    /// </summary>
    [Serializable]
    public class Formular
    {
        public string name;

        public int[] materialIDs;

        public int ResID;

        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool IsMatch(List<int> ids)
        {
            List<int> ownIDs = new List<int>(ids);

            foreach(var temp in materialIDs)
            {
               if(!ownIDs.Contains(temp))
                {
                    return false;
                }
                else
                {
                    ownIDs.Remove(temp);
                }
            }

            return ownIDs.Count == 0;
        }
    }

}
