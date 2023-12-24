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
}
