using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
	/// <summary>
	/// 存档数据记录
	/// </summary>
	[System.Serializable]
	public class SaveData
	{
		private bool isFirstGame;
		private float spendTime;
		private int hour;
		private int minute;
		private int second;

		public void SetIsFirstGame(bool isFirstGame)
		{
			this.isFirstGame = isFirstGame;
		}
        public void SetSpendTime(float spendTime)
		{
			this.spendTime = spendTime;
		}
        public void SetHour(int hour)
		{
			this.hour = hour;
		}
        public void SetMinute(int minute)
		{
			this.minute = minute;
		}
        public void SetSecond(int second)
		{
			this.second = second;
		}

        public bool GetIsFirstGame()
		{
			return isFirstGame;
		}
        public float GetSpendTime()
		{
			return spendTime;
		}
        public int GetHour()
		{
			return hour;
		}
        public int GetMinute()
		{
			return minute;
		}
        public int GetSecond()
		{
			return second;
		}
	}
}
