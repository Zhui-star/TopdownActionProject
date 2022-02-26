using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 角色状态
    /// </summary>
    public abstract class CharacterState 
    {
       public int level;

        public CharacterState()
        {

        }
        public CharacterState(int level)
        {
            this.level = level;
        }

        /// <summary>
        /// 状态获得初始化
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// 状态结束 退出
        /// </summary>

        public abstract void OnExit();

        /// <summary>
        /// 状态每帧实施
        /// </summary>

        public virtual void Process()
        {

        }
    }

}
