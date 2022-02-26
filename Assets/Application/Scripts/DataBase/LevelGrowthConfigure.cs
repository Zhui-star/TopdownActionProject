using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace HTLibrary.Application
{
    /// <summary>
    /// 角色成长配置文件
    /// </summary>
    public class LevelGrowthConfigure :ScriptableObject
    {
        [ReorderableList]
        public List<LevelGrowthUnit> LevelGrowthList = new List<LevelGrowthUnit>();

        /// <summary>
        /// 返回指定等级成长值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public LevelGrowthUnit ReturnGrowthData(int level)
        {
            foreach(var temp in LevelGrowthList)
            {
                if(temp.Level.Equals(level))
                {
                    return temp;
                }
            }
            return null;
        }
    }

}
