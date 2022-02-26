using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 牌类角色的AI （编码型AI）
    /// </summary>
    public class CardChracterAI : MonoBehaviour
    {
        /// <summary>
        /// 要出的牌
        /// </summary>
        public List<Card> selecCards = new List<Card>();

        public CardType currType = CardType.None;


        public void SmartSelectCards(List<Card> cards, CardType cardType, int weight, int length, bool isBiggest)
        {
            cardType = isBiggest ? CardType.None : cardType;
            currType = cardType;
            selecCards.Clear();

            switch (cardType)
            {
                case CardType.None:
                    //随机出牌
                    selecCards = FidnSmallestCards(cards);
                    break;
                case CardType.Single:
                    selecCards = FindSingle(cards, weight);
                    break;
                case CardType.Double:
                    selecCards = FindDouble(cards, weight);
                    break;
                case CardType.Straight:
                    selecCards = FindStraight(cards, weight, length);
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindBoom(cards, -1);
                        currType = CardType.Boom;
                        if (selecCards.Count == 0)
                        {
                            selecCards = FindJokerBoom(cards);
                            currType = CardType.JokerBoom;
                        }
                    }
                    break;
                case CardType.DoubleStraight:
                    selecCards = FindDoubleStraight(cards, weight, length);
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindBoom(cards, -1);
                        currType = CardType.Boom;
                        if (selecCards.Count == 0)
                        {
                            selecCards = FindJokerBoom(cards);
                            currType = CardType.JokerBoom;
                        }
                    }
                    break;
                case CardType.TripleStraight:
                    selecCards = FindBoom(cards, -1);
                    currType = CardType.Boom;
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindJokerBoom(cards);
                        currType = CardType.JokerBoom;
                    }
                    break;
                case CardType.Three:
                    selecCards = FindThree(cards, weight);
                    break;
                case CardType.ThreeAndOne:
                    selecCards = FindThreeAndOne(cards, weight);
                    break;
                case CardType.ThreeAndTwo:
                    selecCards = FindThreeAndDouble(cards, weight);
                    break;
                case CardType.Boom:
                    selecCards = FindBoom(cards, weight);
                    if (selecCards.Count == 0)
                    {
                        selecCards = FindJokerBoom(cards);
                        currType = CardType.JokerBoom;
                    }
                    break;
                case CardType.JokerBoom:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 自己随便出牌
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private List<Card> FidnSmallestCards(List<Card> cards)
        {
            List<Card> select = new List<Card>();
            //先出顺

            for (int i = 12; i >= 5; i--)
            {
                select = FindStraight(cards, -1, i);
                if (select.Count != 0)
                {
                    currType = CardType.Straight;
                    break;
                }
            }

            //三代二
            if (select.Count == 0)
            {
                //3*12
                for (int i = 0; i < 36; i += 3)
                {
                    select = FindThreeAndDouble(cards, i - 1);
                    if (select.Count != 0)
                    {
                        currType = CardType.ThreeAndTwo;
                        break;
                    }
                }
            }
            //三代一
            if (select.Count == 0)
            {
                //3*12
                for (int i = 0; i < 36; i += 3)
                {
                    select = FindThreeAndOne(cards, i - 1);
                    if (select.Count != 0)
                    {
                        currType = CardType.ThreeAndOne;
                        break;
                    }
                }
            }
            //三不带
            if (select.Count == 0)
            {
                //3*12
                for (int i = 0; i < 36; i += 3)
                {
                    select = FindThree(cards, i - 1);
                    if (select.Count != 0)
                    {
                        currType = CardType.Three;
                        break;
                    }
                }
            }

            //找对er
            if (select.Count == 0)
            {
                for (int i = 0; i < 24; i += 2)
                {
                    select = FindDouble(cards, i - 1);
                    if (select.Count != 0)
                    {
                        currType = CardType.Double;
                        break;
                    }
                }
            }

            //单排
            if (select.Count == 0)
            {
                select = FindSingle(cards, -1);
                currType = CardType.Single;
            }

            return select;
        }




        /// <summary>
        /// 单牌
        /// </summary>
        /// <param name="cards">排序好的手牌</param>
        /// <param name="weight">上家出牌大小</param>
        /// <returns></returns>
        public List<Card> FindSingle(List<Card> cards, int weight)
        {
            List<Card> select = new List<Card>();

            for (int i = 0; i < cards.Count; i++)
            {
                if ((int)cards[i].Cardweight > weight)
                {
                    select.Add(cards[i]);
                    break;
                }
            }
            return select;
        }


        /// <summary>
        /// 对儿
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public List<Card> FindDouble(List<Card> cards, int weight)
        {
            List<Card> select = new List<Card>();
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if ((int)cards[i].Cardweight == (int)cards[i + 1].Cardweight)
                {
                    int totalweight = (int)cards[i].Cardweight + (int)cards[i + 1].Cardweight;
                    if (totalweight > weight)
                    {
                        select.Add(cards[i]);
                        select.Add(cards[i + 1]);
                        break;
                    }
                }

            }
            return select;

        }

        /// <summary>
        /// 找顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="minWeight"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public List<Card> FindStraight(List<Card> cards, int minWeight, int length)
        {

            List<Card> select = new List<Card>();
            int counter = 1;
            //CARD的索引
            List<int> indexList = new List<int>();

            for (int i = 0; i < cards.Count - 4; i++)
            {
                int weight = (int)cards[i].Cardweight;

                if (weight > minWeight)
                {
                    counter = 1;
                    indexList.Clear();
                    for (int j = i + 1; j < cards.Count; j++)
                    {
                        if (cards[j].Cardweight > Weight.One)
                            break;

                        if ((int)cards[j].Cardweight - weight == counter)
                        {
                            counter++;
                            indexList.Add(j);
                        }

                        if (counter == length)
                            break;

                    }
                }

                if (counter == length)
                {
                    indexList.Insert(0, i);
                    break;

                }
            }
            //加元素
            if (counter == length)
            {
                for (int i = 0; i < indexList.Count; i++)
                {
                    select.Add(cards[indexList[i]]);
                }
            }

            return select;

        }

        /// <summary>
        /// 找双顺 556677
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="minWeight"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public List<Card> FindDoubleStraight(List<Card> cards, int minWeight, int length)
        {

            List<Card> select = new List<Card>();
            int counter = 0;
            //CARD的索引
            List<int> indexList = new List<int>();

            for (int i = 0; i < cards.Count - 4; i++)
            {
                int weight = (int)cards[i].Cardweight;

                if (weight > minWeight)
                {
                    counter = 0;
                    indexList.Clear();

                    //游标 控制counter ++
                    int temp = 0;
                    for (int j = i + 1; j < cards.Count; j++)
                    {
                        if (cards[j].Cardweight > Weight.One)
                            break;

                        if ((int)cards[j].Cardweight - weight == counter)
                        {
                            temp++;

                            if (temp % 2 == 1)
                            {
                                counter++;
                            }
                            indexList.Add(j);
                        }

                        if (counter == length / 2)
                            break;

                    }
                }

                if (counter == length / 2)
                {
                    indexList.Insert(0, i);
                    break;

                }
            }
            //加元素
            if (counter == length / 2)
            {
                for (int i = 0; i < indexList.Count; i++)
                {
                    select.Add(cards[indexList[i]]);
                }
            }

            return select;

        }




        /// <summary>
        /// 三不带
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public List<Card> FindThree(List<Card> cards, int weight)
        {
            List<Card> select = new List<Card>();
            for (int i = 0; i < cards.Count - 3; i++)
            {
                if ((int)cards[i].Cardweight == (int)cards[i + 1].Cardweight &&
                    (int)cards[i].Cardweight == (int)cards[i + 2].Cardweight)
                {
                    int totalweight = (int)cards[i].Cardweight + (int)cards[i + 1].Cardweight +
                        (int)cards[i + 2].Cardweight;
                    if (totalweight > weight)
                    {
                        select.Add(cards[i]);
                        select.Add(cards[i + 1]);
                        select.Add(cards[i + 2]);
                        break;
                    }
                }

            }
            return select;

        }

        /// <summary>
        /// 三代一
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public List<Card> FindThreeAndDouble(List<Card> cards, int weight)
        {
            List<Card> select = new List<Card>();
            List<Card> Three = FindThree(cards, weight);

            if (Three.Count > 0)
            {
                foreach (var card in Three)
                    cards.Remove(card);

                List<Card> two = FindDouble(cards, -1);

                if (two.Count != 0)
                {
                    select.AddRange(Three);
                    select.AddRange(two);
                }

            }

            return select;

        }

        /// <summary>
        /// 三代一
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public List<Card> FindThreeAndOne(List<Card> cards, int weight)
        {
            List<Card> select = new List<Card>();
            List<Card> Three = FindThree(cards, weight);

            if (Three.Count > 0)
            {
                foreach (var card in Three)
                    cards.Remove(card);

                List<Card> one = FindSingle(cards, -1);

                if (one.Count != 0)
                {
                    select.AddRange(Three);
                    select.AddRange(one);
                }

            }

            return select;
        }

        /// <summary>
        /// 炸弹
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public List<Card> FindBoom(List<Card> cards, int weight)
        {
            List<Card> select = new List<Card>();
            for (int i = 0; i < cards.Count - 4; i++)
            {
                if ((int)cards[i].Cardweight == (int)cards[i + 1].Cardweight &&
                    (int)cards[i].Cardweight == (int)cards[i + 2].Cardweight &&
                    (int)cards[i].Cardweight == (int)cards[i + 3].Cardweight)
                {
                    int totalweight = (int)cards[i].Cardweight +
                        (int)cards[i + 1].Cardweight +
                        (int)cards[i + 2].Cardweight +
                        (int)cards[i + 3].Cardweight;
                    if (totalweight > weight)
                    {
                        select.Add(cards[i]);
                        select.Add(cards[i + 1]);
                        select.Add(cards[i + 2]);
                        select.Add(cards[i + 3]);
                        break;
                    }
                }

            }
            return select;

        }

        /// <summary>
        /// 王炸
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public List<Card> FindJokerBoom(List<Card> cards)
        {
            List<Card> select = new List<Card>();
            for (int i = 0; i < cards.Count - 1; i++)
            {
                if (cards[i].Cardweight == Weight.SJoker
                    && cards[i + 1].Cardweight == Weight.LJoker)
                {
                    select.Add(cards[i]);
                    select.Add(cards[i + 1]);
                    break;
                }
            }
            return select;

        }
    }

}
