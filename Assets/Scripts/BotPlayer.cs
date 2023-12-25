using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class BotPlayer : MonoBehaviour
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

    public int sortingOrder = 0; // �]�w��ܶ���

    const float CardSpacing = 25f;

    private List<int> acceptCardNum = new List<int> { 1, 2, 5 };//��i�B��l�B���l�K�� ��Ī

    private CardLogic CardLogic = new CardLogic();

    public void Init()
    {
        Main.Instance.OnSendCard += ReceiveHand;

        Main.Instance.TableData.OnReportReceiveCard += GetReportReceiveCard;

        cardDatas = new CardData[cardObjects.Length];

        for (int i = 0; i < cardObjects.Length; i++)
        {
            cardDatas[i] = cardObjects[i].AddComponent<CardData>();
            cardDatas[i].Reset();
        }
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
        // ���ݭn�b�o����ܤ�P�A�]�� Bot ���ݭn�e���W��ܤ�P

        for (int i = 0; i < hand.Count; i++)
        {
            cardDatas[i].SetCardData(hand[i],false);
        }

    }

    public void PlayCard()
    {

    }
}
