using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameData;

public class Main : MonoBehaviour
{
    public delegate void NotifyPlayerReadySendCardEvent(int id);
    public event NotifyPlayerReadySendCardEvent OnNotifyPlayerReadySendCardEvent;

    public delegate void NotifyWinnerEvent(int id);
    public event NotifyWinnerEvent OnNotifiWinner;

    public delegate void PlayerSendCardEvent();
    public event PlayerSendCardEvent OnPlayerSendCard;


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


    [SerializeField]
    private int SendCardID = 0;

    // Start is called before the first frame update
    void Start()
    {

        mPlayer.Init();
        mBotPlayer.Init();
        TableData.Init();

        mPlayer.OnSendCardToMain += GetPlayerSendCardEvent;
        mBotPlayer.OnBotSendCardToMain += GetBotPlayerSendCardEvent;
        mBotPlayer.OnBotSendPass += GetBotSendPass;

        NewGame();

    }

    private void NewGame()
    {
        mTableData.Reset();
        
        // ��l�Ƽ��J�P
        InitializeDeck();

        // �~�P
        ShuffleDeck();

        // �o�P
        DealCards();

        // ���o��a�̤p���d�P
        Card player1MinCard = player1Hand.OrderBy(card => card.CompareRank).First();
        Card player2MinCard = player2Hand.OrderBy(card => card.CompareRank).First();

        Debug.Log($"player1MinCard - {player1MinCard.CompareRank} - {player1MinCard.Rank} - {player1MinCard.Suit}");
        Debug.Log($"player2MinCard - {player2MinCard.CompareRank} - {player2MinCard.Rank} - {player2MinCard.Suit}");


        if (player1MinCard.CompareRank == player2MinCard.CompareRank)
        {
            if (player1MinCard.Suit < player2MinCard.Suit)
            {
                StartCoroutine(delaySend(0));
            }
            else
                StartCoroutine(delaySend(1));
        }
        else if (player1MinCard.CompareRank < player2MinCard.CompareRank)
        {
            StartCoroutine(delaySend(0));
        }
        else if (player1MinCard.CompareRank > player2MinCard.CompareRank)
            StartCoroutine(delaySend(1));


    }

    private void GetBotSendPass()
    {
        NotifyPlayerPlayedCard(0);
        mTableData.GetSendData(null);
    }

    private IEnumerator delaySend(int id)
    {

        yield return new WaitForSeconds(1.0f);
        NotifyPlayerPlayedCard(id);
    }

    private IEnumerator WaitForSecond2()
    {

        yield return new WaitForSeconds(1.0f);
        NewGame();
    }


    private void GetBotPlayerSendCardEvent(List<CardData> sendData)
    {
        //�ˬd���W�O�_���o�ǥd�P
        foreach (var data in sendData)
        {
            if (!player2Hand.Contains(data.cardData))
            {
                Debug.Log("���~�A���䤣�쪺�d�P���");
                return;
            }
        }

        mTableData.GetSendData(sendData);

        foreach (var data in sendData)
            player2Hand.Remove(data.cardData);


        //mPlayer.ClearSelectCardIndex();

        if (player2Hand.Count > 0)
        {
            NotifyPlayerPlayedCard(0);
        }else
        {
            NotiftWinner(1);
        }

    }

    /// <summary>
    /// ������ǹL�Ӫ��d�P
    /// </summary>
    /// <param name="sendData"></param>
    private void GetPlayerSendCardEvent(List<CardData> sendData)
    {
        //�ˬd���W�O�_���o�ǥd�P
        foreach(var data in sendData)
        {
            if(!player1Hand.Contains(data.cardData))
            {
                Debug.Log("���~�A���䤣�쪺�d�P���");
                return;
            }
        }

        mTableData.GetSendData(sendData);

        foreach(var data in sendData)
            player1Hand.Remove(data.cardData);


        mPlayer.ClearSelectCardIndex();

        if(player1Hand.Count > 0)
        {
            NotifyPlayerPlayedCard(1);
        }else
        {
            NotiftWinner(0);
        }

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

    public void NotifyPlayerPlayedCard(int sendCardID)
    {
        // �B�z���a�X�P���q���A�Ҧp�ˬd�O�_�ŦX�W�h�A�M��q���U�@�쪱�a
        // ...
        mTableData.SetArrowImage(sendCardID);

        OnNotifyPlayerReadySendCardEvent?.Invoke(sendCardID);
    }

    public void NotiftWinner(int id)
    {
        OnNotifiWinner?.Invoke(id);
        StartCoroutine(WaitForSecond2());
    }

}
