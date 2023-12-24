using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static GameData;
public class Player : MonoBehaviour
{
    private List<Card> hand = new List<Card>();

    [SerializeField]
    private GameObject[] cardObjects;

    private CardData[] cardDatas;

    [SerializeField]
    private List<int> _SelectCardIndex = new List<int>();

    [SerializeField]
    private List<CardData> _SelectCardData = new List<CardData>();

    [SerializeField]
    private GameObject mCardPlate;

    public int sortingOrder = 0; // 設定顯示順序

    [SerializeField]
    private TableData mTableData;

    const float CardSpacing = 25f;

    private List<int> acceptCardNum = new List<int> { 1, 2, 5 };//單張、對子、順子鐵支 葫蘆

    private CardLogic CardLogic = new CardLogic();

    private void Start()
    {

    }
    public void Init()
    {
        Main.Instance.OnSendCard += ReceiveHand;

        mTableData.OnReportReceiveCard += GetReportReceiveCard;

        cardDatas = new CardData[cardObjects.Length];

        for (int i = 0; i < cardObjects.Length; i++)
        {
            cardDatas[i] = cardObjects[i].AddComponent<CardData>();
            cardDatas[i].Reset();
            cardDatas[i].OnCardClick += OnCardClicked;// 註冊卡牌點擊事件
        }
    }

    private void GetReportReceiveCard(bool isOk)
    {
        if(isOk)
        {
            foreach (var data in _SelectCardData)
            {
                data.gameObject.SetActive(false);
            }
        }
    }

    void OnCardClicked(int cardID)
    {
        if(!_SelectCardIndex.Contains(cardID))
        {
            _SelectCardIndex.Add(cardID);
        }else
        {
            _SelectCardIndex.Remove(cardID);
        }

        // 卡牌被點擊時的邏輯
        Debug.Log("Card Clicked! cardID: " + cardID);
    }

    public void ReceiveHand(List<Card> cards)
    {
        hand = cards;

        for (int i = 0; i < hand.Count; i++)
        {
            cardDatas[i].SetCardData(hand[i]);
        }

    }

    public void onPlayerSendCard()
    {

        _SelectCardData.Clear();

        //取得要出牌的物件
        for (int i = 0; i < _SelectCardIndex.Count; i++)
        {
            CardData cardData = GetCardDataByNumber(_SelectCardIndex[i]);
            if (cardData == null)
            {
                Debug.Log("Error");
                return;
            }

            _SelectCardData.Add(cardData);
        }

        if (!CardLogic.SendCardLogic(_SelectCardData))
            return;

        _SelectCardData = _SelectCardData.OrderBy(cardData => cardData.cardData.CompareRank)
                                 .ThenBy(cardData => cardData.cardData.Rank)
                                 .ThenBy(cardData => cardData.cardData.Suit)
                                 .ToList();

        Sequence sequence = DOTween.Sequence();

        Vector3 targetPosition = mCardPlate.transform.localPosition;

        for (int i = 0; i < _SelectCardData.Count; i++)
        {
            // 計算每張牌的位置
            float targetX = (i - (_SelectCardData.Count - 1) / 2.0f) * CardSpacing;

            // 取得Transform
            Transform cardTransform = _SelectCardData[i].transform;

            sequence.Join(cardTransform.DOLocalMove( new Vector3(targetPosition.x+targetX,targetPosition.y, targetPosition.z), 1.0f));

        }

        sequence.OnComplete(() => MoveCardToTable(_SelectCardData));

    }

    private void MoveCardToTable(List<CardData> sendCardData)
    {
        mTableData.GetSendData(sendCardData);

        _SelectCardIndex.Clear();
    }

    private CardData GetCardDataByNumber(int number)
    {
        foreach(var data in cardDatas)
        {
            if (data.cardData.Number == number)
                return data;
        }

        return null;
    }

}
