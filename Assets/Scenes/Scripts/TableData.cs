using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableData : MonoBehaviour
{
    public Transform cardSpawnPoint; // �P�ͦ�����m
    [SerializeField]
    private GameObject cardPrefab; // �d�P���w�m����
    public float moveDuration = 1.0f; // ���ʪ��ɶ�
    public float fadeDuration = 0.5f; // �z�����ܤƪ��ɶ�
    public float fadedAlpha = 0.5f; // �z�����ܤƪ��ؼЭ�

    //�ǰe�i�Ӫ��d�P�s���a��
    private List<TableCard> cardList = new List<TableCard>();
    //���e�ǰe��Ӧs���a��
    private List<TableCard> lastCardList = new List<TableCard>();

    
    //�P�_�P�O�_�i�H�X�P�᪺Event
    public delegate void ReportReceiveCardEvent(bool isOk);
    public event ReportReceiveCardEvent OnReportReceiveCard;

    //�X�P�޿�P�_
    private CardLogic cardLogic;

    //�X�L���d�P�n���ʪ���m
    private const int mLastCardSpceing = -50;

    //��W�d�P�����c
    public class TableCard
    {
        public GameObject cardObj;
        public CardData cardData;
    }

    /// <summary>
    /// �����쪱�a��P
    /// </summary>
    /// <param name="sendCardData"></param>
    public void GetSendData(List<CardData> sendCardData)
    {
        //�@���ˬd
        if (!cardLogic.SendCardLogic(sendCardData))
        {
            //�����~
            OnReportReceiveCard?.Invoke(false);
            return;
        }

        //�i�H�X�P
        OnReportReceiveCard?.Invoke(true);
        CreateCardInTable(sendCardData);
    }


    /// <summary>
    /// �b�ୱ�W���͵P
    /// </summary>
    /// <param name="sendCardData"></param>
    public void CreateCardInTable(List<CardData> sendCardData)
    {

        lastCardList.AddRange(cardList);

        cardList = new List<TableCard>();

        for (int i = 0; i < sendCardData.Count;i++)
        {
            TableCard tempObj = new TableCard();
            tempObj.cardObj = Instantiate(cardPrefab, cardSpawnPoint.position, Quaternion.identity);

            tempObj.cardObj.transform.SetParent(cardSpawnPoint);
            tempObj.cardData = tempObj.cardObj.AddComponent<CardData>();
            tempObj.cardData.Reset();
            tempObj.cardData.SetCardData(sendCardData[i].cardData);

            cardList.Add(tempObj);
        }

        UpdateCardPositionsAndAlpha();

    }

    /// <summary>
    /// ���ʦ�m
    /// </summary>
    private void UpdateCardPositionsAndAlpha()
    {
        //�P��P�������Z��
        float spacing = 20f;

        for (int i = 0; i < lastCardList.Count; i++)
        {
            float targetX = (i - cardList.Count - lastCardList.Count) * spacing;

            lastCardList[i].cardObj.transform.DOLocalMoveX(mLastCardSpceing + targetX, moveDuration);

            //�z��
            lastCardList[i].cardData.FadeOut(fadeDuration, fadedAlpha);
        }

        for (int i = 0; i < cardList.Count; i++)
        {
            float targetX = i * spacing;

            cardList[i].cardObj.transform.localPosition = new Vector3(targetX, 0, 0);
            cardList[i].cardObj.transform.DOLocalMoveX(targetX, moveDuration);
        }
    }
}
