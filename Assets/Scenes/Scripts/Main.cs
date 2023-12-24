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

    public delegate void SendCardEvent(List<Card> handCard);
    public event SendCardEvent OnSendCard;

    private List<Card> player1Hand = new List<Card>();
    private List<Card> player2Hand = new List<Card>();

    
    // Start is called before the first frame update
    void Start()
    {

        mPlayer.Init();

        // 初始化撲克牌
        InitializeDeck();

        // 洗牌
        ShuffleDeck();

        // 發牌
        DealCards();
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
        // 使用 Sort 方法和 Lambda 表達式進行排序
        //        player1Hand = player1Hand.OrderBy(card => card.Suit).ThenBy(card => card.Rank).ToList();

        player1Hand = player1Hand.OrderBy(card => card.Rank).ThenBy(card => card.Suit).ToList();
        //player1Hand.Sort((card1, card2) => card1.Rank.CompareTo(card2.Rank));

        OnSendCard?.Invoke(player1Hand);

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

            //foreach (string rank in ranks)
            //{
            //    deck.Add(new Card(  deck.Count+1, rank, currentSuit));
            //}
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

    public void NotifyPlayerPlayedCard(Player player, Card card)
    {
        // 處理玩家出牌的通知，例如檢查是否符合規則，然後通知下一位玩家
        // ...
    }

    public void NotifyBotPlayedCard(BotPlayer botPlayer, Card card)
    {
        // 處理 Bot 出牌的通知，例如檢查是否符合規則，然後通知下一位玩家
        // ...
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
