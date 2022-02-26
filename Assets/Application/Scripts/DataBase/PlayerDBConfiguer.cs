using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class PlayerDBConfiguer :ScriptableObject
    {
        public float CoinAmount;

        /// <summary>
        /// 返回现有的钱
        /// </summary>
        /// <returns></returns>
        public float GetCoinAmount()
        {
            return CoinAmount;
        }

        /// <summary>
        /// 更新现有的钱
        /// </summary>
        /// <param name="coinAmount"></param>
        public void SetCoinAmount(float coinAmount)
        {
            this.CoinAmount = coinAmount;
        }

        /// <summary>
        /// 叠加现有的钱
        /// </summary>
        /// <param name="coinAmount"></param>
        public void AddCoinAmount(float coinAmount)
        {
            this.CoinAmount += coinAmount;
        }
    }

}
