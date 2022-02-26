using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using HTLibrary.Framework;
using HTLibrary.Utility;
using UnityEngine.UI;
using PixelCrushers.Wrappers;
namespace HTLibrary.Application
{
    public class SavePanel : MonoBehaviour
    {
        public event Action UpdateUIEvent;
        public Transform cancelTrs;
        public Transform decideNameTrs;
        public InputField inputFileText;
        public Text decideNameBtnText;

        public void TransformState(bool open)
        {
            if(open)
            {
                transform.DOScale(1, 0.25f);
            }
            else
            {
                transform.DOScale(0, 0.25f);
            }
        }

        public void CancelConfirmPanelClick()
        {
            cancelTrs.DOScale(0, 0.25f);
        }

        public void ConfirmDeleteClick()
        {
            int id = SaveManager.Instance.LoadGameID;
            PlayerPrefs.DeleteKey(Consts.Name + id);
            PlayerPrefs.DeleteKey(Consts.GameTime + id);
            PlayerPrefs.DeleteKey(Consts.GameLevel + id);
            PlayerPrefs.DeleteKey(Consts.GameExp + id);
            PlayerPrefs.DeleteKey(Consts.GameCurrentHP + id);
            PlayerPrefs.DeleteKey(Consts.Knapsack + id);
            PlayerPrefs.DeleteKey(Consts.EquipPanel + id);
            PlayerPrefs.DeleteKey(Consts.shieldWeapon + id);
            PlayerPrefs.DeleteKey(Consts.sowrdWeapon + id);
            PlayerPrefs.DeleteKey(Consts.archerWeapon + id);
            PlayerPrefs.DeleteKey(Consts.magicianWeapon + id);
            PlayerPrefs.DeleteKey(Consts.LearnedTalent + id);
            PlayerPrefs.DeleteKey(Consts.CameraDistance + id);
            PlayerPrefs.DeleteKey(Consts.Coin + id);
            SaveManager.Instance._dialogGameSveStore.DeleteSavedGameData(id);
            PlayerPrefs.Save();


            CancelConfirmPanelClick();
            UpdateUIEvent?.Invoke();
        }

        public void CancelNameDecideClick()
        {
            decideNameTrs.DOScale(0, 0.25f);
        }

        public void DeicdeNameClick()
        {
           if(String.IsNullOrEmpty(inputFileText.text))
            {
                decideNameTrs.DOShakePosition(0.25f, 2);
                return;
            }

         

            if (!PlayerPrefs.HasKey(Consts.Name+SaveManager.Instance.LoadGameID))
            {
                PlayerPrefs.SetString(Consts.Name + SaveManager.Instance.LoadGameID, inputFileText.text);
                PlayerPrefs.Save();
                SaveManager.Instance.InitialData();
                LoadScenesUtility.Instance.LoadScenes(1);
            }
            else
            {
                PlayerPrefs.SetString(Consts.Name + SaveManager.Instance.LoadGameID, inputFileText.text);
                PlayerPrefs.Save();
                CancelNameDecideClick();
                UpdateUIEvent?.Invoke();
            }


        }

        public  void DecideNameBtnText()
        {
            if(!PlayerPrefs.HasKey(Consts.Name + SaveManager.Instance.LoadGameID))
            {
                decideNameBtnText.text = "确定";
            }
            else
            {
                decideNameBtnText.text = "修改";
            }
        }


    }

}
