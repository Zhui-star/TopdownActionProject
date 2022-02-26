using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

namespace HTLibrary.Application
{
    public class SaveBtn3Ctrl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private string timeTotal;
        private bool isFirstGame;
        private SaveData save;

        public Text timeText;
        public Text nameText;

        public GameObject emptyText;
        public GameObject saveChoosePanel;
        public GameObject nameDecidePanel;
        public GameObject decideButton1;
        public GameObject decideButton2;
        public GameObject decideButton3;
        public GameObject editButton1;
        public GameObject editButton2;
        public GameObject editButton3;
        public GameObject YesButton1;
        public GameObject YesButton2;
        public GameObject YesButton3;
        public GameObject NoButton1;
        public GameObject NoButton2;
        public GameObject NoButton3;

        public GameObject savePanelEditButton;
        public GameObject savePanelDeleteButton;

        public InputField inputField1;

        public static SaveBtn3Ctrl _instance;

        private void Awake()
        {
            ReadData();
        }

        private void Start()
        {

            _instance = this;
            ReadData();

            if (PlayerPrefs.HasKey("Name3")&& PlayerPrefs.HasKey("TimeTotal3"))
            {
                nameText.text = PlayerPrefs.GetString("Name3");
                timeText.text = PlayerPrefs.GetString("TimeTotal3");
                emptyText.SetActive(false);
                savePanelEditButton.SetActive(true);
                savePanelDeleteButton.SetActive(true);

            }
            else
            {
                savePanelEditButton.SetActive(false);
                savePanelDeleteButton.SetActive(false);

            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * 0.97f, 0.2f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
           transform.DOPunchPosition(new Vector3(5f, 0, 0), 1f,3,0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOLocalMove(new Vector3(0, -155, 0), 0.5f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, 0.2f);
        }


        public void OnClick()
        {
            // decideButton3.SetActive(true);
            //editButton3.SetActive(false);
            decideButton1.SetActive(false);
            decideButton2.SetActive(false);
            decideButton3.SetActive(true);
            editButton1.SetActive(false);
            editButton2.SetActive(false);
            editButton3.SetActive(true);
            YesButton1.SetActive(false);
            YesButton2.SetActive(false);
            YesButton3.SetActive(true);
            NoButton1.SetActive(false);
            NoButton2.SetActive(false);
            NoButton3.SetActive(true);

            if (isFirstGame)
            {
                nameDecidePanel.transform.DOScale(Vector3.one, 0.3f);
            }
            else
            {
                if (emptyText.activeSelf == true)
                {
                    saveChoosePanel.SetActive(true);
                    saveChoosePanel.transform.DOScale(Vector3.one, 0.3f);
                }
                else
                {
                    StartGame();
                }
            }
        }

        //设置游戏运行时间
        public void SetGameTime(SaveData save)
        {
            PlayerPrefs.SetString("TimeTotal3", "游戏时间  " + string.Format("{0:D2}:{1:D2}:{2:D2}", save.GetHour(), save.GetMinute(), save.GetSecond()));
        }

        //删除存档
        public void DeleteData()
        {
            PlayerPrefs.DeleteKey("TimeTotal3");
            PlayerPrefs.DeleteKey("Name3");
            timeText.text = null;
            nameText.text = null;
            emptyText.SetActive(true);
            savePanelEditButton.SetActive(false);
            savePanelDeleteButton.SetActive(false);

        }


        //确定名称
        public void NameDecide()
        {
            if (inputField1.text.Trim().Length == 0)
            {
                inputField1.transform.DOShakePosition(1, 10);
            }
            else
            {
                PlayerPrefs.SetString("Name3", inputField1.text);
                nameDecidePanel.transform.DOScale(Vector3.zero, 0.2f);
                emptyText.SetActive(false);
                nameText.text = PlayerPrefs.GetString("Name3");
                savePanelEditButton.SetActive(true);
                savePanelDeleteButton.SetActive(true);

                ReadData();
                if (save != null)
                {
                    timeText.text = "游戏时间  " + string.Format("{0:D2}:{1:D2}:{2:D2}", save.GetHour(), save.GetMinute(), save.GetSecond());
                    Invoke("StartGame", 1);
                    Debug.Log("I have save!!!");
                }
                else
                {
                    timeText.text = "游戏时间  " + "00:00:00";
                    Invoke("StartCG", 1);

                }
            }
        }

        //进行修改名字
        public void NameEdit()
        {
            editButton1.SetActive(false);
            editButton2.SetActive(false);
            editButton3.SetActive(true);
            //nameDecidePanel.SetActive(true);
            decideButton3.SetActive(false);
            inputField1.text = PlayerPrefs.GetString("Name3");
            editButton3.SetActive(true);
            nameDecidePanel.transform.DOScale(Vector3.one, 0.3f);
        }

        /// <summary>
        /// SaveChoosePanel
        /// </summary>
        //存储为新存档
        public void YesButtonControl()
        {
            File.Delete(UnityEngine.Application.dataPath + "/GameData3.txt");
            isFirstGame = true;
            saveChoosePanel.transform.DOScale(Vector3.zero, 0.1f);
            nameDecidePanel.transform.DOScale(Vector3.one, 0.3f);
        }

        //载入之前的存档
        public void NoButtonControl()
        {
            saveChoosePanel.transform.DOScale(Vector3.zero, 0.1f);
            nameDecidePanel.transform.DOScale(Vector3.one, 0.3f);
        }

        //修改名字确认
        public void EditNameDecide()
        {
            nameText.text = inputField1.text;
            PlayerPrefs.SetString("Name3", inputField1.text);
            nameDecidePanel.transform.DOScale(Vector3.zero, 0.2f);
        }

        //取消修改
        public void CancelFirstPanel()
        {
            nameDecidePanel.transform.DOScale(Vector3.zero, 0.2f);
        }

        //进行游戏 不进入新手教程
        public void StartGame()
        {
            PlayerPrefs.SetString("SavePath", UnityEngine.Application.dataPath + "/GameData3.txt");
            SceneManager.LoadScene("1-1");
        }

        //开始新手教程
        public void StartCG()
        {
            PlayerPrefs.SetString("SavePath", UnityEngine.Application.dataPath + "/GameData3.txt");
            SceneManager.LoadScene("01_CG");
        }

        //读取游戏存储数据
        public void ReadData()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = File.Open(UnityEngine.Application.dataPath + "/GameData3.txt", FileMode.Open))
                {
                    save = (SaveData)bf.Deserialize(fs);
                    SetGameTime(save);
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                save = null;
                isFirstGame = true;
            }
        }

    }

}

