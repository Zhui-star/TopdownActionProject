using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 持有牌的角色
    /// </summary>
    public enum CharacterType
    {
        Library,
        Player,
        ComputerRight,
        //ComputerLeft,
        Desk
    }

    /// <summary>
    /// 花色
    /// </summary>
    public enum Colors
    {
        None,//小王 大王
        Club,//梅花
        Heart,//红桃
        Spade,//黑桃
        Square//方片
    }


    /// <summary>
    /// 牌的大小
    /// </summary>
    public enum Weight
    {
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        One,
        Two,
        SJoker,
        LJoker
    }

    /// <summary>
    /// 出牌类型
    /// </summary>
    public enum CardType
    {
        None,
        Single,//单 1
        Double,//对儿 2
        Straight,//顺子 5 - 12
        DoubleStraight,//双顺 >= 6
        TripleStraight,//飞机 >= 6 
        Three,//三不带 3
        ThreeAndOne,//三带一 4
        ThreeAndTwo,//三代二 5
        Boom,//炸弹 4
        JokerBoom//王炸 2
    }

    /// <summary>
    /// 牌类属性（斗地主模板）
    /// </summary>
    public class Card
    {
        string cardName;
        Weight cardweight;
        Colors cardColor;
        CharacterType belongTo;

        public string CardName
        {
            get
            {
                return cardName;
            }

        }

        public Weight Cardweight
        {
            get
            {
                return cardweight;
            }


        }

        public CharacterType BelongTo
        {
            get
            {
                return belongTo;
            }

            set
            {
                belongTo = value;
            }
        }

        public Colors CardColor
        {
            get
            {
                return cardColor;
            }

        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="name">卡牌名字</param>
        /// <param name="color">卡牌花色</param>
        /// <param name="weight">卡牌大小</param>
        /// <param name="type">归属于谁</param>
        public Card(string name, Colors color, Weight weight, CharacterType type)
        {
            this.cardName = name;
            this.cardColor = color;
            this.cardweight = weight;
            this.belongTo = type;
        }
    }

}
