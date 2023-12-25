using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameData;

public class Main : MonoBehaviour
{
    [SerializeField]
    private Player mPlayer;

    [SerializeField]
    private BotPlayer mBotPlayer;

    private static Main instance;
    public static Main Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Main>();
            }
            return instance;
        }
    }


    private List<Card> deck = new List<Card>();

    private List<int> ranks = new List<int>() { 1,2,3,4,5,6,7,8,9,10,11,12,13 };
    private List<int> compareRank = new List<int>() { 14,15,3,4,5,6,7,8,9,10,11,12,13 };

    [SerializeField]
    private int mShuffleCount = 30;

    public delegate void SendCardEvent(List<Card> handCard,int ID);
    public event SendCardEvent OnSendCard;

    private List<Card> player1Hand = new List<Card>();
    private List<Card> player2Hand = new List<Card>();

    [SerializeField]
    private TableData mTableData;

    public TableData TableData { get { return mTableData; } }


    // Start is called before the first frame update
    void Start()
    {

        mPlayer.Init();
        mBotPlayer.Init();
        TableData.Init();
        // 初始化撲克牌
        InitializeDeck();

        // 洗牌
        ShuffleDeck();

        // 發牌
        DealCards();

        // 取得兩家最小的卡牌
        Card player1MinCard = player1Hand.OrderBy(card => card.CompareRank).First();
        Card player2MinCard = player2Hand.OrderBy(card => card.CompareRank).First(); 

        Debug.Log($"player1MinCard - {player1MinCard.CompareRank} - {player1MinCard.Rank} - {player1MinCard.Suit}");
        Debug.Log($"player2MinCard - {player2MinCard.CompareRank} - {player2MinCard.Rank} - {player2MinCard.Suit}");


        if (player1MinCard.CompareRank == player2MinCard.CompareRank)
        {
            if (player1MinCard.Suit < player2MinCard.Suit)
            {
                NotifyPlayerPlayedCard();
            }
            else
                NotifyBotPlayedCard();
        }else if (player1MinCard.CompareRank < player2MinCard.CompareRank)
        {
            NotifyPlayerPlayedCard();
        }else if (player1MinCard.CompareRank > player2MinCard.CompareRank)
            NotifyBotPlayedCard();
    }

    private void DealCards()
    {
        player1Hand.Clear();
        player2Hand.Clear();

        for (int i = 0; i < deck.Count/2; i++)
        {
            if (i % 2 == 0)
            {
                player1Hand.Add(deck[i]);
            }
            else
            {
                player2Hand.Add(deck[i]);
            }
        }
        player1Hand = player1Hand.OrderBy(card => card.Rank).ThenBy(card => card.Suit).ToList();
        player2Hand = player2Hand.OrderBy(card => card.Rank).ThenBy(card => card.Suit).ToList();
        OnSendCard?.Invoke(player1Hand,0);
        OnSendCard?.Invoke(player2Hand, 1);
    }

    private Card GetMinCard(List<Card> hand)
    {
        Card minCard = hand[0];
        foreach (Card card in hand)
        {
            if (ranks.IndexOf(card.CompareRank) > ranks.IndexOf(minCard.CompareRank))
            {
                minCard = card;
            }
        }
        return minCard;
    }


    private void InitializeDeck()
    {
        deck.Clear();

        for(int i = 1; i < (int)CardSuit._MAX; i++)
        {
            CardSuit currentSuit = (CardSuit)i;

            for(int j = 0; j < ranks.Count;j++)
            {
                deck.Add(new Card(compareRank[j] ,deck.Count + 1, ranks[j], currentSuit));
            }
        }
    }

    private void ShuffleDeck()
    {
        int n = mShuffleCount;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            Card value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }

    public void NotifyPlayerPlayedCard()
    {
        // 處理玩家出牌的通知，例如檢查是否符合規則，然後通知下一位玩家
        // ...
        mTableData.SetArrowImage(0);
    }

    public void NotifyBotPlayedCard()
    {
        // 處理 Bot 出牌的通知，例如檢查是否符合規則，然後通知下一位玩家
        // ...
        mTableData.SetArrowImage(1);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
