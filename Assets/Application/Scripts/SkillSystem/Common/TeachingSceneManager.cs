using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using UnityEngine.SceneManagement;
using HTLibrary.Utility;
using MoreMountains.FeedbacksForThirdParty;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    public class TeachingSceneManager : MonoBehaviour
    {
        private Health health;
        private int sceneIndex;
        //private MMAutoFocus focus;
        Character character;
     
        private IEnumerator Start()
        {
            GetScene();

            yield return new WaitUntil(() => CharacterManager.Instance.GetCharacter("Player1") != null);

            if (health==null)
            {

                character = CharacterManager.Instance.GetCharacter("Player1");//得到当前玩家控制的角色
                if (character)
                {
                    health = character.gameObject.GetComponent<Health>();
                }
            }


            health.OnDeath += CharacterDeathCallBack;
        }

        private void OnDestroy()
        {
            health.OnDeath -= CharacterDeathCallBack;
        }

        /// <summary>
        /// 角色死亡回调
        /// </summary>
        void CharacterDeathCallBack()
        {
            health.CurrentHealth = health.MaximumHealth;
            PlayerPrefs.SetInt(Consts.GameCurrentHP + SaveManager.Instance.LoadGameID, health.CurrentHealth); //新手教学复活时 应该满血
            PlayerPrefs.Save();

            LoadScenesUtility.Instance.LoadScenes(sceneIndex);
        }

        public void GetScene()
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
    }
}


