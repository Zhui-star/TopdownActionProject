using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doodah.Components.Progression;
using System;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 角色战斗中的经验成长
    /// </summary>
    public class CharacterXP : MonoBehaviour
    {
        Progression progression;
        public event Action<float> ExperienceUpdateEvent;

        private void Start()
        {
            progression = GetComponent<Progression>();

            if (PlayerPrefs.HasKey(Consts.GameLevel + SaveManager.Instance.LoadGameID))
            {
                int level = PlayerPrefs.GetInt(Consts.GameLevel + SaveManager.Instance.LoadGameID);

                for (int i = 1; i <= level; i++)
                {
                    progression.Level = i;
                }
            }

            if (PlayerPrefs.HasKey(Consts.GameExp + SaveManager.Instance.LoadGameID))
            {
                AddExperience(PlayerPrefs.GetFloat(Consts.GameExp + SaveManager.Instance.LoadGameID));
            }
            progression.InitalFinished = true;
        }


        public float GetExperiencePercent()
        {
            if(progression==null)
            {
                progression = GetComponent<Progression>();
            }
            
            return progression.Experience / progression.GetFloor();
        }

        public float GetCurrentExperience()
        {
            return progression.Experience;
        }

        public float GetFloorExperience()
        {
            return progression.GetFloor();
        }

        public int GetLevel()
        {
            return progression.Level;
        }

        /// <summary>
        /// 是否是最大等级 2020 2 20
        /// </summary>
        /// <returns></returns>
        public bool IsMaxLevel()
        {
            return progression.IsMaxLevel();
        }

        /// <summary>
        /// 增加经验
        /// </summary>
        /// <param name="value"></param>
        public void AddExperience(float value)
        {
            if (CharacterAddExpState._instance != null)
            {
                value = CharacterAddExpState._instance.AddExp(value);
            }

            progression.AddExperience(value);

            ExperienceUpdateEvent?.Invoke(value);

            PlayerPrefs.SetFloat(Consts.GameExp+SaveManager.Instance.LoadGameID, GetCurrentExperience());
        }
    }

}
