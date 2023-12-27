using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public enum CardSuit
    {
        None,
        Clubs = 1,//����
        Diamond = 2,//�p��
        Heart = 3,//�R��
        Spade = 4,//�®�
        _MAX
    }


    public class Card
    {
        public int CompareRank; //��P
        public int Number;      //���J�P1~52
        public int Rank;        //�Ʀr
        public CardSuit Suit;   //���
        

        public Card(int compareRank, int number, int rank, CardSuit suit)
        {
            CompareRank = compareRank;
            Number = number;
            Rank = rank;
            Suit = suit;
        }
    }
    public enum CardType
    {
        None,
        Single,
        Pair,
        Straight,
        FullHouse,
        IronCard,
    }
    public class CheckCardLogin
    {
        public bool SendCardLogic = false;
        public CardType SendCardType = CardType.None;
    }
    //��W�d�P�����c
    public class TableCard
    {
        public GameObject cardObj;
        public CardData cardData;
    }

    public class SelectSendCard
    {
        public int Number = 99;
        public CardData cardData;
    }
}
