using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using System;
using HTLibrary.Application;
namespace HTLibrary.Utility
{
    
    public class Wallet :MonoSingleton<Wallet>
    {
        private float coinAmount;

        public event Action<float> GetCoinAmountEvent;
        public PlayerDBConfiguer playerDB;

        private void OnEnable()
        {
            if (playerDB == null) return;
            this.coinAmount = playerDB.GetCoinAmount();
        }

        public float GetCoinAmount
        {
            get { return coinAmount; }
        }

        /// <summary>
        /// 消耗金钱
        /// </summary>
        /// <param name="coinAmount"></param>
        /// <returns></returns>
        public bool ConsumerCoin(float coinAmount)
        {
            if (this.coinAmount >= coinAmount)
            {
                this.coinAmount -= coinAmount;
                GetCoinAmountEvent?.Invoke(GetCoinAmount);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获得金钱
        /// </summary>
        /// <param name="coinAmount"></param>
        public void EarnCoin(float coinAmount)
        {
            this.coinAmount += coinAmount;
            GetCoinAmountEvent?.Invoke(GetCoinAmount);
        }

        private void OnDisable()
        {
            playerDB.SetCoinAmount(this.coinAmount);
        }
    }

}
