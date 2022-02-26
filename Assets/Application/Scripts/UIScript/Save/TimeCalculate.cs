using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
namespace HTLibrary.Application
{
    public class TimeCalculate : MonoBehaviour
    {
        private SaveData save;

        private bool isFirstGame;
        private float spendTime;
        private int hour;
        private int minute;
        private int second;
        public string path = null;


        public static TimeCalculate _instance;
        public Text timeText;

        private void Awake()
        {
            path = PlayerPrefs.GetString("SavePath");
        }

        private void Start()
        {
            _instance = this;
            InitGameData();
            //DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            
            spendTime += Time.deltaTime;
            hour = (int)spendTime / 3600;
            minute = ((int)spendTime - hour * 3600) / 60;
            second = (int)spendTime - hour * 3600 - minute * 60;

            timeText.text = "游戏时间: " + string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
            SaveData();
        }

        private void InitGameData()
        {
            ReadData();
            if(save!=null)
            {
                isFirstGame = save.GetIsFirstGame();
            }
            else
            {
                isFirstGame = true;
            }
            if (isFirstGame)
            {
                isFirstGame = false;
                spendTime = 0;
                hour = 0;
                minute = 0;
                second = 0;
                save = new SaveData();
                SaveData();
            }
            else
            {
                spendTime = save.GetSpendTime();
                hour = save.GetHour();
                minute = save.GetMinute();
                second = save.GetSecond();
            }
        }

        public void SaveData()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using(FileStream fs = File.Create(path))
                {
                    save.SetSpendTime(spendTime);
                    save.SetHour(hour);
                    save.SetMinute(minute);
                    save.SetSecond(second);
                    bf.Serialize(fs, save);
                }
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private void ReadData()
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = File.Open(path,FileMode.Open))
                {
                    save = (SaveData)bf.Deserialize(fs);
                }
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void ResetData()
        {
            File.Delete(path);
            isFirstGame = true;
            spendTime = 0;
            hour = 0;
            minute = 0;
            second = 0;
        }
    }
}

