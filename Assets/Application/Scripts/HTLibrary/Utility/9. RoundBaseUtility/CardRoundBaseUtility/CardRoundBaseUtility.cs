using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HTLibrary.Framework;
namespace HTLibrary.Utility
{
    /// <summary>
    /// AI 执行数据（斗地主模板）
    /// </summary>
    public class ComputerSmartArgs
    {
        public CardType CardType;
        public int Weight;
        public int Length;
        public CharacterType IsBiggest;
        public CharacterType CharacterType;

    }
    /// <summary>
    /// 回合执行工具(斗地主模板）
    /// 下回合
    /// 初始回合
    /// </summary>
    public class CardRoundBaseUtility:Singleton<CardRoundBaseUtility>
    {

        public bool isLandlord = false;
        public bool isWin = false;

        public static event Action<bool> PlayerHandler;
        public static event Action<ComputerSmartArgs> ComputerHandler;

        int currentWeight;
        int currentLength;
        CardType currentType;
        CharacterType biggestCharacter;
        CharacterType currentCharacter;

        /// <summary>
        ///最大出牌人的出牌大小
        /// </summary>
        public int CurrentWeight
        {
            get
            {
                return currentWeight;
            }

            set
            {
                currentWeight = value;
            }
        }

        /// <summary>
        /// 出牌长度
        /// </summary>
        public int CurrentLength
        {
            get
            {
                return currentLength;
            }

            set
            {
                currentLength = value;
            }
        }

        /// <summary>
        /// 最大出牌者
        /// </summary>
        public CharacterType BiggestCharacter
        {
            get
            {
                return biggestCharacter;
            }

            set
            {
                biggestCharacter = value;
            }
        }
        /// <summary>
        /// 现在该谁出牌
        /// </summary>
        public CharacterType CurrentCharacter
        {
            get
            {
                return currentCharacter;
            }

            set
            {
                currentCharacter = value;
            }
        }
        /// <summary>
        /// 现在出牌类型
        /// </summary>
        public CardType CurrentType
        {
            get
            {
                return currentType;
            }

            set
            {
                currentType = value;
            }
        }

        public void InitRound()
        {
            this.currentType = CardType.None;
            this.currentWeight = -1;
            this.currentLength = -1;
            this.biggestCharacter = CharacterType.Desk;
            this.currentCharacter = CharacterType.Desk;
        }

        /// <summary>
        /// 抢地主的人触发
        /// </summary>
        /// <param name="cType"></param>
        public void Start(CharacterType cType)
        {
            this.biggestCharacter = cType;
            this.currentCharacter = cType;
            BeginWith(cType);
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="cType"></param>
        public void BeginWith(CharacterType cType)
        {
            if (cType == CharacterType.Player)
            {
                //玩家出牌
                if (PlayerHandler != null)
                    PlayerHandler(BiggestCharacter != CharacterType.Player);
            }
            else
            {
                //电脑出牌
                if (ComputerHandler != null)
                {
                    ComputerSmartArgs e = new ComputerSmartArgs()
                    {
                        CardType = this.CurrentType,
                        Length = this.CurrentLength,
                        Weight = this.CurrentWeight,
                        IsBiggest = this.BiggestCharacter,
                        CharacterType = this.CurrentCharacter
                    };
                    ComputerHandler(e);
                }
            }
        }
        /// <summary>
        /// 轮换出牌
        /// </summary>
        public void Turn()
        {
            currentCharacter++;
            if (currentCharacter == CharacterType.Desk || currentCharacter == CharacterType.Library)
                currentCharacter = CharacterType.Player;
            BeginWith(currentCharacter);
        }
    }

}
