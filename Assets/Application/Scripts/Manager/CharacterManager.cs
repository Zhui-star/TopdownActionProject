using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using MoreMountains.TopDownEngine;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class CharacterManager :MonoSingleton<CharacterManager>
    {

        private Dictionary<string, Character> charactertDicts = new Dictionary<string, Character>();

        /// <summary>
        /// 添加角色管理
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="character"></param>
        public void AddCharater(string playerID,Character character)
        {
            if (string.IsNullOrEmpty(playerID)|| charactertDicts.ContainsKey(playerID))
                return;

            charactertDicts.Add(playerID, character);

        }

        /// <summary>
        /// 移除角色管理
        /// </summary>
        /// <param name="playerID"></param>
        public void RemoverCharacter(string playerID)
        {
            if (string.IsNullOrEmpty(playerID)||!charactertDicts.ContainsKey(playerID))
            {
                return;
            }
              
            charactertDicts.Remove(playerID);
        }

        /// <summary>
        /// 得到角色通过PlayerID 例如: GetCharacter("Player1")
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public Character GetCharacter(string playerID)
        {
            return charactertDicts.TryGet<string, Character>(playerID);
        }

        private void OnDestroy()
        {
            charactertDicts.Clear();
        }
    }

}
