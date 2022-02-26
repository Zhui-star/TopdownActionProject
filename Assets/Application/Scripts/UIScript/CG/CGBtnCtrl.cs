using UnityEngine;
using UnityEngine.SceneManagement;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    public class CGBtnCtrl : MonoBehaviour
    {
        public GameObject LoadingPage;

       
        public void OnStartBtnClick()
        {
            LoadingPage.SetActive(true);
           // SceneManager.LoadSceneAsync("0-1");

            //TODO 测试打包
            SceneManager.LoadSceneAsync("99-1-Lobby");

        }
    }
}

