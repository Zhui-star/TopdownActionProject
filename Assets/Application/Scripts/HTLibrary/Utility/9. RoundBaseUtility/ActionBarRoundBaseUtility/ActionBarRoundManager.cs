using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 行动跳回合制的管理器
    /// 1. 设置
    /// 2. 返回
    /// </summary>
    public class ActionBarRoundManager:MonoSingleton<ActionBarRoundManager>
    {
        private CharacterType_Round playerType;
        
        private ActionBarRoundState state=ActionBarRoundState.Computer;
          
        public ActionBarRoundState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public CharacterType_Round PlayerType
        {
            get
            {
                return playerType;
            }
            set
            {
                playerType = value;
            }
        }

        public List<RoundCharacter> list_character = new List<RoundCharacter>();

        Dictionary<CharacterType_Round, RoundCharacter> dicts_character = new Dictionary<CharacterType_Round, RoundCharacter>();

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        public void SetState(ActionBarRoundState state)
        {
            this.State = state;
        }

        /// <summary>
        /// 返回状态
        /// </summary>
        /// <returns></returns>
        public ActionBarRoundState GetState()
        {
            return this.State;
        }

        /// <summary>
        /// 设置当前回合执行玩家的类型
        /// </summary>
        /// <param name="playerType"></param>
        public void SetPlayerType(CharacterType_Round playerType)
        {
            this.PlayerType = playerType;
        }

        /// <summary>
        /// 得到当前回合执行玩家的类型
        /// </summary>
        /// <returns></returns>
        public CharacterType_Round GetPlayerType()
        {
            return this.PlayerType;
        }

        /// <summary>
        /// 得到特定类型的角色类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RoundCharacter GetRoundChracterByType(CharacterType_Round type)
        {
            return dicts_character.TryGet<CharacterType_Round, RoundCharacter>(type);
        }

        /// <summary>
        /// 初始化
        /// 1. 字典
        /// </summary>
        private void Start()
        {
            foreach(var temp in list_character)
            {
                dicts_character.Add(temp.characterType, temp);
            }
        }
    }

}
