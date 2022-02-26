using UnityEngine;
using HTLibrary.Application;
using HTLibrary.Utility;
using PixelCrushers.Wrappers;
using PixelCrushers.DialogueSystem;
namespace HTLibrary.Framework
{
    /// <summary>
    /// 管理游戏存储
    /// </summary>
    public class SaveManager : MonoSingleton<SaveManager>
    {

        public int LoadGameID { get; set; }
        [HideInInspector]
        public PlayerPrefsSavedGameDataStorer _dialogGameSveStore;

        private void Start()
        {
            _dialogGameSveStore=FindObjectOfType<PlayerPrefsSavedGameDataStorer>();    
        }

        public void InitialData()
        {
            HTDBManager.Instance.Initial();
            TalentSystemManager.Instance.Initial();
            LoadDialogPersistantData();
        }

        public void SaveGameTime()
        {
            PlayerPrefs.SetFloat(Consts.GameTime+LoadGameID,PlayerPrefs.GetFloat(Consts.GameTime+LoadGameID,0)+ Time.realtimeSinceStartup);
            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            SaveGameTime();
            SaveDialogPersistantData();
        }

        /// <summary>
        /// Load dialog data 
        /// </summary>
        private void LoadDialogPersistantData()
        {
            DialogueManager.ResetDatabase(DatabaseResetOptions.KeepAllLoaded);

            if(SaveSystem.HasSavedGameInSlot(LoadGameID))
            {
                SaveSystem.LoadFromSlot(LoadGameID);
            }
        }

        /// <summary>
        /// Save dialog data
        /// </summary>
        public void SaveDialogPersistantData()
        {
            SaveSystem.SaveToSlot(LoadGameID);
        }

    }

}
