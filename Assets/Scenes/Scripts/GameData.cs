using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public enum CardSuit
    {
        None,
        Clubs = 1,//梅花
        Diamond = 2,//鑽石
        Heart = 3,//愛心
        Spade = 4,//黑桃
        _MAX
    }


    public class Card
    {
        public int CompareRank; //比牌
        public int Number;      //撲克牌1~52
        public int Rank;        //數字
        public CardSuit Suit;   //花色
        

        public Card(int compareRank, int number, int rank, CardSuit suit)
        {
            CompareRank = compareRank;
            Number = number;
            Rank = rank;
            Suit = suit;
        }
    }
}
