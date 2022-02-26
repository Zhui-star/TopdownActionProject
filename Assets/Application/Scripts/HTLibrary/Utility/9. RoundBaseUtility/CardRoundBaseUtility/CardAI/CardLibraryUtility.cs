using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;

namespace HTLibrary.Utility
{
    /// <summary>
    /// 一个牌库包含了（斗地主模板）
    /// 洗牌
    /// 发牌
    /// </summary>
    public class CardLibraryUtility:Singleton<CardLibraryUtility>
    {

        CharacterType cType = CharacterType.Library;
        Queue<Card> CardLibrary = new Queue<Card>();

        /// <summary>
        /// 剩余牌数
        /// </summary>
        public int CardCount
        {
            get
            {
                return CardLibrary.Count;
            }
        }

        /// <summary>
        /// 54张牌
        /// </summary>
        public void InitCardLibrary()
        {
            //52张
            for (int color = 1; color < 5; color++)
            {
                for (int weight = 0; weight < 13; weight++)
                {
                    Colors c = (Colors)color;
                    Weight w = (Weight)weight;
                    string name = c.ToString() + w.ToString();
                    Card card = new Card(name, c, w, cType);
                    CardLibrary.Enqueue(card);
                }
            }

            Card sJoker = new Card("SJoker", Colors.None, Weight.SJoker, cType);
            Card lJoker = new Card("LJoker", Colors.None, Weight.LJoker, cType);
            CardLibrary.Enqueue(sJoker);
            CardLibrary.Enqueue(lJoker);


        }

        /// <summary>
        /// 返回牌库
        /// </summary>
        /// <returns></returns>
        public Queue<Card> GetCards()
        {
            return CardLibrary;
        }

        /// <summary>
        /// 设置牌库
        /// </summary>
        /// <param name="cards"></param>
        public void SetCards(Queue<Card> cards)
        {
            CardLibrary = cards;
        }


        /// <summary>
        /// 洗牌
        /// </summary>
        public void Shuffle()
        {
            List<Card> newList = new List<Card>();
            foreach (var card in CardLibrary)
            {
                int index = Random.Range(0, newList.Count + 1);
                newList.Insert(index, card);
            }

            CardLibrary.Clear();
            foreach (var card in newList)
            {
                CardLibrary.Enqueue(card);
            }

            newList.Clear();
        }

        /// <summary>
        /// 最开始发牌
        /// </summary>
        /// <param name="sendTo">发给谁</param>
        public Card DealCard(CharacterType sendTo)
        {
            Card card = CardLibrary.Dequeue();
            card.BelongTo = sendTo;
            return card;
        }
    }

}
