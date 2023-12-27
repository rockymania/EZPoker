using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class BotPlayer : MonoBehaviour
{
    public int ID = 1;

    public delegate void BotSendCardToMainEvent(List<CardData> sendCardData);
    public BotSendCardToMainEvent OnBotSendCardToMain;

    public delegate void BotSendPassEvent();
    public event BotSendPassEvent OnBotSendPass;

    private List<Card> hand = new List<Card>();

    [SerializeField]
    private GameObject[] cardObjects;

    private CardData[] cardDatas;

    [SerializeField]
    private List<int> _SelectCardIndex = new List<int>();

    [SerializeField]
    private List<CardData> _SelectCardData = new List<CardData>();

    private List<CardData> UseCardData = new List<CardData>();


    [SerializeField]
    private GameObject mCardPlate;

    public int sortingOrder = 0; // 設定顯示順序

    const float CardSpacing = 25f;

    private List<int> acceptCardNum = new List<int> { 1, 2, 5 };//單張、對子、順子鐵支 葫蘆

    private CardLogic CardLogic = new CardLogic();

    private int mNowSendCardID = -1;

    private bool AnalyzSendCard = false;
    private void Update()
    {
        if (mNowSendCardID == 1)
        {
            if (!AnalyzSendCard)
            {
                AnalyzSendCard = true;
                //出牌
                SendCard();
            }
        }
    }

    private void SendCardPresent()
    {
        Sequence sequence = DOTween.Sequence();

        Vector3 targetPosition = mCardPlate.transform.localPosition;

        for (int i = 0; i < _SelectCardData.Count; i++)
        {
            // 計算每張牌的位置
            float targetX = (i - (_SelectCardData.Count - 1) / 2.0f) * CardSpacing;

            // 取得Transform
            Transform cardTransform = _SelectCardData[i].transform;

            sequence.Join(cardTransform.DOLocalMove(new Vector3(targetPosition.x + targetX, targetPosition.y, targetPosition.z), 1.0f));

        }

        sequence.OnComplete(() => MoveCardToTable(_SelectCardData));
    }

    private void MoveCardToTable(List<CardData> sendCardData)
    {
        OnBotSendCardToMain?.Invoke(sendCardData);
        AnalyzSendCard = false;
    }


    private void SendCard()
    {
        //取得Type
        CardType cardType = Main.Instance.TableData.nowTableCardType;
        _SelectCardData.Clear();
        if (cardType == CardType.None)
        {

            //出牌，目前就最小最先抓到的卡牌
            //手上卡牌順序 1～13 但是出牌應該是出 最小的
          
            SelectSendCard selectSendCard = new SelectSendCard();

            foreach (var data in cardDatas)
            {
                if (UseCardData.Contains(data)) continue;

                if (data.cardData.CompareRank < selectSendCard.Number)
                {
                    selectSendCard.Number = data.cardData.CompareRank;
                    selectSendCard.cardData = data;
                }
            }

            if(selectSendCard.Number != 99)
                _SelectCardData.Add(selectSendCard.cardData);
        }

        //可以出的Type
        if (cardType == CardType.Single)
        {
            //找出最小的比桌面牌大的就可以
            var tableCard = Main.Instance.TableData.nowTableCardList;

            SelectSendCard selectSendCard = new SelectSendCard();

            foreach (var data in cardDatas)
            {
                if (UseCardData.Contains(data)) continue;

                if (data.cardData.CompareRank < selectSendCard.Number  )
                {
                    if (data.cardData.CompareRank < tableCard[0].cardData.cardData.CompareRank)
                        continue;
                    if (data.cardData.CompareRank == tableCard[0].cardData.cardData.CompareRank && data.cardData.Suit < tableCard[0].cardData.cardData.Suit)
                        continue;


                    selectSendCard.Number = data.cardData.CompareRank;
                    selectSendCard.cardData = data;
                }
            }
            if (selectSendCard.Number != 99)

                _SelectCardData.Add(selectSendCard.cardData);
        }

        if (_SelectCardData.Count == 0)
        {
            OnBotSendPass?.Invoke();
            AnalyzSendCard = false;
            return;
        }

        UseCardData.AddRange(_SelectCardData);
        SendCardPresent();
    }

    public void Init()
    {
        Main.Instance.OnSendCard += ReceiveHand;

        Main.Instance.TableData.OnReportReceiveCard += GetReportReceiveCard;

        Main.Instance.OnNotifyPlayerReadySendCardEvent += GetNotifiSendCard;

        cardDatas = new CardData[cardObjects.Length];

        for (int i = 0; i < cardObjects.Length; i++)
        {
            cardDatas[i] = cardObjects[i].AddComponent<CardData>();
            cardDatas[i].Reset();
        }
        if(UseCardData == null)
            UseCardData = new List<CardData>();

        UseCardData.Clear();

        Main.Instance.OnNotifiWinner += GetNotifiWinner;
    }

    private void GetNotifiSendCard(int id)
    {
        mNowSendCardID = id;
    }
    private void GetReportReceiveCard(bool isOk)
    {
        if (isOk)
        {
            foreach (var data in _SelectCardData)
            {
                data.gameObject.SetActive(false);
            }
        }
    }

    public void ReceiveHand(List<Card> cards, int ID)
    {
        if (ID != 1) return;

        hand = cards;
        // 不需要在這裡顯示手牌，因為 Bot 不需要畫面上顯示手牌

        for (int i = 0; i < hand.Count; i++)
        {
            cardDatas[i].SetCardData(hand[i],false);
        }
    }

    private void GetNotifiWinner(int id)
    {
        Debug.Log("勝利者產生");
    }
}
