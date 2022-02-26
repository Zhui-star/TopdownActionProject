
using UnityEngine;
using UnityEngine.UI;
using HTLibrary.Utility;
using HTLibrary.Framework;
using DG.Tweening;
namespace HTLibrary.Application
{
    public class SaveItemUI : MonoBehaviour
    {
        public int id;

        public GameObject EmptyGo;

        public Text timeText;

        public Text nameText;

        public GameObject EditBtn;
        public GameObject DeleteBtn;

        public GameObject NameDecidePanel;
        public GameObject ConfirmDeletePanel;
        public SavePanel savePanel;

        public CanvasGroup _savePanelCanvas;
        private void OnEnable()
        {
            savePanel.UpdateUIEvent += UpdateUI;
        }

        private void OnDisable()
        {
            savePanel.UpdateUIEvent -= UpdateUI;
        }

        private void Start()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            if (PlayerPrefs.HasKey(Consts.Name + id))
            {
                EmptyGo.SetActive(false);
                timeText.text = GetTimeString();
                nameText.text = PlayerPrefs.GetString(Consts.Name + id);
                EditBtn.SetActive(true);
                DeleteBtn.SetActive(true);
            }
            else
            {
                EmptyGo.SetActive(true);
                timeText.text = "";
                nameText.text = "";
                EditBtn.SetActive(false);
                DeleteBtn.SetActive(false);
            }
        }

        public void EnterGameClick()
        {
            if (PlayerPrefs.HasKey(Consts.Name+id))
            {
                _savePanelCanvas.interactable=false;
                SaveManager.Instance.LoadGameID = id;
                SaveManager.Instance.InitialData();
                //TODO 加载到最近的一个大厅
                // LevelUnitManager.Instance.CurrentLevelIndex = 5;
                // LoadScenesUtility.Instance.LoadScenes(6);

                //TODO 测试修改
                LevelUnitManager.Instance.CurrentLevelIndex = 1;
                LoadScenesUtility.Instance.LoadScenes(1);
              
            }
            else
            {
                SaveManager.Instance.LoadGameID = id;
                EditNameClick();           
            }
        }

        public void DeleteSaveClick()
        {
            SaveManager.Instance.LoadGameID = id;
            ConfirmDeletePanel.transform.DOScale(1, 0.25f);
        }

        public void EditNameClick()
        {
            SaveManager.Instance.LoadGameID = id;
            savePanel.DecideNameBtnText();
            NameDecidePanel.transform.DOScale(1, 0.25f);
        }

        private string GetTimeString()
        {
            float gameTime = PlayerPrefs.GetFloat(Consts.GameTime+id, 0);
            int hours = (int)gameTime / 3600;
            int minutes = ((int)gameTime - hours * 3600) / 60;
            int second = (int)gameTime - hours * 3600 - minutes * 60;
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, second);
        }
    }

}
