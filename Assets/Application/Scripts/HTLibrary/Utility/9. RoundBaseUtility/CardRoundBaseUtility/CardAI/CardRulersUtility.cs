using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 牌类规则管理器（斗地主模板）
    /// </summary>
    public class CardRulersUtility:Singleton<CardRulersUtility>
    {
        /// <summary>
        /// 可否出牌
        /// </summary>
        /// <param name="cards">传入的牌</param>
        /// <param name="type">出牌类型</param>
        /// <returns></returns>
        public static bool CanPop(List<Card> cards, out CardType type)
        {
            type = CardType.None;
            bool can = false;
            switch (cards.Count)
            {
                case 1:
                    if (IsSingle(cards))
                    {
                        can = true;
                        type = CardType.Single;
                    }

                    break;
                case 2:
                    if (IsDouble(cards))
                    {
                        can = true;
                        type = CardType.Double;
                    }
                    else if (IsJokerBoom(cards))
                    {
                        can = true;
                        type = CardType.JokerBoom;
                    }
                    break;
                case 3:
                    if (IsThree(cards))
                    {
                        can = true;
                        type = CardType.Three;
                    }
                    break;
                case 4:
                    if (IsBoom(cards))
                    {
                        can = true;
                        type = CardType.Boom;
                    }
                    else if (IsThreeAndOne(cards))
                    {
                        can = true;
                        type = CardType.ThreeAndOne;
                    }
                    break;
                case 5:
                    if (IsStraight(cards))
                    {
                        can = true;
                        type = CardType.Straight;
                    }
                    else if (IsThreeAndTwo(cards))
                    {
                        can = true;
                        type = CardType.ThreeAndTwo;
                    }
                    break;
                case 6:
                    if (IsStraight(cards))
                    {
                        can = true;
                        type = CardType.Straight;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        can = true;
                        type = CardType.DoubleStraight;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        can = true;
                        type = CardType.TripleStraight;
                    }
                    break;
                case 7:
                    if (IsStraight(cards))
                    {
                        can = true;
                        type = CardType.Straight;
                    }
                    break;
                case 8:
                    if (IsStraight(cards))
                    {
                        can = true;
                        type = CardType.Straight;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        can = true;
                        type = CardType.DoubleStraight;
                    }
                    break;
                case 9:
                    if (IsStraight(cards))
                    {
                        can = true;
                        type = CardType.Straight;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        can = true;
                        type = CardType.TripleStraight;
                    }
                    break;
                case 10:
                    if (IsStraight(cards))
                    {
                        can = true;
                        type = CardType.Straight;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        can = true;
                        type = CardType.DoubleStraight;
                    }
                    break;
                case 11:
                    if (IsStraight(cards))
                    {
                        can = true;
                        type = CardType.Straight;
                    }
                    break;
                case 12:
                    if (IsStraight(cards))
                    {
                        can = true;
                        type = CardType.Straight;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        can = true;
                        type = CardType.DoubleStraight;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        can = true;
                        type = CardType.TripleStraight;
                    }
                    break;
                case 13:
                    break;
                case 14:
                    if (IsDoubleStraight(cards))
                    {
                        can = true;
                        type = CardType.DoubleStraight;
                    }
                    break;
                case 15:
                    if (IsTripleStraight(cards))
                    {
                        can = true;
                        type = CardType.TripleStraight;
                    }
                    break;
                case 16:
                    if (IsDoubleStraight(cards))
                    {
                        can = true;
                        type = CardType.DoubleStraight;
                    }
                    break;
                case 17:
                    break;
                case 18:
                    if (IsDoubleStraight(cards))
                    {
                        can = true;
                        type = CardType.DoubleStraight;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        can = true;
                        type = CardType.TripleStraight;
                    }
                    break;
                case 19:
                    break;
                case 20:
                    if (IsDoubleStraight(cards))
                    {
                        can = true;
                        type = CardType.DoubleStraight;
                    }
                    break;
                default:
                    break;
            }
            return can;
        }

        /// <summary>
        /// 是否单牌
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsSingle(List<Card> cards)
        {
            return cards.Count == 1;
        }


        /// <summary>
        /// 判断对儿
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDouble(List<Card> cards)
        {
            if (cards.Count == 2)
            {
                if (cards[0].Cardweight == cards[1].Cardweight)
                    return true;

            }

            return false;
        }

        /// <summary>
        /// 是否是顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsStraight(List<Card> cards)
        {
            if (cards.Count < 5 || cards.Count > 12)
                return false;
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i + 1].Cardweight - cards[i].Cardweight != 1)
                    return false;
                //不能超过A
                if (cards[i].Cardweight > Weight.One || cards[i + 1].Cardweight > Weight.One)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// 是否双顺
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDoubleStraight(List<Card> cards)
        {
            if (cards.Count < 6 || cards.Count % 2 != 0)
                return false;

            for (int i = 0; i < cards.Count - 2; i += 2)
            {
                if (cards[i + 1].Cardweight != cards[i].Cardweight)
                    return false;
                if (cards[i + 2].Cardweight - cards[i].Cardweight != 1)
                    return false;

                //不能超过A
                if (cards[i].Cardweight > Weight.One || cards[i + 2].Cardweight > Weight.One)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// 是否飞机
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraight(List<Card> cards)
        {
            if (cards.Count < 6 || cards.Count % 3 != 0)
                return false;

            for (int i = 0; i < cards.Count - 3; i += 3)
            {
                if (cards[i + 1].Cardweight != cards[i].Cardweight)
                    return false;
                if (cards[i + 2].Cardweight != cards[i].Cardweight)
                    return false;
                if (cards[i + 1].Cardweight != cards[i + 2].Cardweight)
                    return false;

                if (cards[i + 3].Cardweight - cards[i].Cardweight != 1)
                    return false;

                //不能超过A
                if (cards[i].Cardweight > Weight.One || cards[i + 3].Cardweight > Weight.One)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// 三不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThree(List<Card> cards)
        {
            if (cards.Count != 3)
                return false;

            if (cards[1].Cardweight != cards[0].Cardweight)
                return false;
            //if (cards[2].Cardweight != cards[0].Cardweight)
            //    return false;
            if (cards[1].Cardweight != cards[2].Cardweight)
                return false;
            return true;
        }

        /// <summary>
        /// 三带一
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndOne(List<Card> cards)
        {
            if (cards.Count != 4)
                return false;
            if (cards[1].Cardweight == cards[0].Cardweight && cards[1].Cardweight == cards[2].Cardweight)
                return true;
            else if (cards[1].Cardweight == cards[2].Cardweight && cards[3].Cardweight == cards[2].Cardweight)
                return true;
            return false;

        }

        /// <summary>
        /// 三代二
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndTwo(List<Card> cards)
        {
            if (cards.Count != 5)
                return false;

            if (cards[1].Cardweight == cards[0].Cardweight && cards[1].Cardweight == cards[2].Cardweight)
            {
                if (cards[3].Cardweight == cards[4].Cardweight)
                    return true;

            }
            else if (cards[3].Cardweight == cards[2].Cardweight && cards[3].Cardweight == cards[4].Cardweight)
            {
                if (cards[1].Cardweight == cards[0].Cardweight)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 炸弹
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoom(List<Card> cards)
        {
            if (cards.Count != 4)
                return false;

            if (cards[1].Cardweight != cards[0].Cardweight)
                return false;
            if (cards[2].Cardweight != cards[1].Cardweight)
                return false;
            if (cards[3].Cardweight != cards[2].Cardweight)
                return false;
            return true;
        }
        /// <summary>
        /// 王炸
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsJokerBoom(List<Card> cards)
        {
            if (cards.Count != 2)
                return false;

            if (cards[0].Cardweight == Weight.SJoker && cards[1].Cardweight == Weight.LJoker)
                return true;

            return false;
        }
    }

}
