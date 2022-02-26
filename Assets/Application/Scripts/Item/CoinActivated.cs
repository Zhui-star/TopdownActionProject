using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using System;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{

    public class CoinActivated :MonoBehaviour
    {
        public int maxCount;  //可以給的最大金币数量
        public int minCount;
        private int actualMoney; //实际给的数量 每次都不一样
        private GameMenuePanel gameMenuePanel;
        public AudioClip clip;

        private void OnEnable()
        {
            actualMoney = UnityEngine.Random.Range(minCount, maxCount);
        }
        private void Start()
        {
            gameMenuePanel = GameObject.Find("GameMenuePanel(Clone)").GetComponent<GameMenuePanel>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Tags.Player)
            {
                HTDBManager.Instance.SaveCoinGet(actualMoney);
                CoinGotten();
                if (gameMenuePanel != null)
                {
                    gameMenuePanel.UpdatePlayerCoin();
                }
            }
        }
 
        private void CoinGotten()
        {
            this.gameObject.SetActive(false);
            SoundManager.Instance.PlaySound(clip, this.transform.position);
        }
    }

}
