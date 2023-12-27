using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class TableData : MonoBehaviour
{
    public Transform cardSpawnPoint; // �P�ͦ�����m
    [SerializeField]
    private GameObject cardPrefab; // �d�P���w�m����
    public float moveDuration = 1.0f; // ���ʪ��ɶ�
    public float fadeDuration = 0.5f; // �z�����ܤƪ��ɶ�
    public float fadedAlpha = 0.5f; // �z�����ܤƪ��ؼЭ�

    //�ǰe�i�Ӫ��d�P�s���a��
    private List<TableCard> mCardList = new List<TableCard>();
    //���e�ǰe��Ӧs���a��
    private List<TableCard> mLastCardList = new List<TableCard>();

    public List<TableCard> nowTableCardList { get { return mCardList; } }

    //�P�_�P�O�_�i�H�X�P�᪺Event
    public delegate void ReportReceiveCardEvent(bool isOk);
    public event ReportReceiveCardEvent OnReportReceiveCard;

    //�X�P�޿�P�_
    private CardLogic cardLogic = new CardLogic();

    //�X�L���d�P�n���ʪ���m
    private const int mLastCardSpceing = -50;

    [SerializeField]
    private GameObject _UpImageObj;
    [SerializeField]
    private GameObject _DownImageObj;

    private CardType mCardType = CardType.None;

    public CardType nowTableCardType { get { return mCardType; } }
    public void Init()
    {
        SetArrowImage(-1);
    }

    public void SetArrowImage(int kind)
    {
        _UpImageObj.SetActive(false);
        _DownImageObj.SetActive(false);

        if(kind == 0)
            _DownImageObj.SetActive(true);
        else if(kind == 1)
            _UpImageObj.SetActive(true);


    }

    


    /// <summary>
    /// �����쪱�a��P
    /// </summary>
    /// <param name="sendCardData"></param>
    public void GetSendData(List<CardData> sendCardData)
    {
        if(sendCardData == null)
        {
            mCardType = CardType.None;
            return;
        }

        CheckCardLogin checkCardLogin = new CheckCardLogin();

        checkCardLogin = cardLogic.SendCardLogic(sendCardData);

        if(mCardType != CardType.None)
        if (!checkCardLogin.SendCardLogic || mCardType != checkCardLogin.SendCardType )
        {
            //�����~
            OnReportReceiveCard?.Invoke(false);
            return;
        }

        mCardType = checkCardLogin.SendCardType;

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

        mLastCardList.AddRange(mCardList);

        mCardList = new List<TableCard>();

        for (int i = 0; i < sendCardData.Count;i++)
        {
            TableCard tempObj = new TableCard();
            tempObj.cardObj = Instantiate(cardPrefab, cardSpawnPoint.position, Quaternion.identity);

            tempObj.cardObj.transform.SetParent(cardSpawnPoint);
            tempObj.cardData = tempObj.cardObj.AddComponent<CardData>();
            tempObj.cardData.Reset();
            tempObj.cardData.SetCardData(sendCardData[i].cardData);

            mCardList.Add(tempObj);
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

        for (int i = 0; i < mLastCardList.Count; i++)
        {
            float targetX = (i - mCardList.Count - mLastCardList.Count) * spacing;

            mLastCardList[i].cardObj.transform.DOLocalMoveX(mLastCardSpceing + targetX, moveDuration);

            //�z��
            mLastCardList[i].cardData.FadeOut(fadeDuration, fadedAlpha);
        }

        for (int i = 0; i < mCardList.Count; i++)
        {
            float targetX = i * spacing;

            mCardList[i].cardObj.transform.localPosition = new Vector3(targetX, 0, 0);
            mCardList[i].cardObj.transform.DOLocalMoveX(targetX, moveDuration);
        }
    }

    public void Reset()
    {
        mCardType = CardType.None;
        mCardList = new List<TableCard>();
        mLastCardList = new List<TableCard>();
        if (_DownImageObj != null)
            _DownImageObj.SetActive(false);
        if (_UpImageObj != null)
            _UpImageObj.SetActive(false);

        foreach (Transform child in cardSpawnPoint)
        {
            Destroy(child.gameObject); // �Ψϥ� DestroyImmediate(child.gameObject);
        }
    }

}
