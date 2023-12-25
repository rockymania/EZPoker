using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableData : MonoBehaviour
{
    public Transform cardSpawnPoint; // 牌生成的位置
    [SerializeField]
    private GameObject cardPrefab; // 卡牌的預置物體
    public float moveDuration = 1.0f; // 移動的時間
    public float fadeDuration = 0.5f; // 透明度變化的時間
    public float fadedAlpha = 0.5f; // 透明度變化的目標值

    //傳送進來的卡牌存的地方
    private List<TableCard> cardList = new List<TableCard>();
    //之前傳送近來存的地方
    private List<TableCard> lastCardList = new List<TableCard>();

    
    //判斷牌是否可以出牌後的Event
    public delegate void ReportReceiveCardEvent(bool isOk);
    public event ReportReceiveCardEvent OnReportReceiveCard;

    //出牌邏輯判斷
    private CardLogic cardLogic = new CardLogic();

    //出過的卡牌要移動的位置
    private const int mLastCardSpceing = -50;

    [SerializeField]
    private GameObject _UpImageObj;
    [SerializeField]
    private GameObject _DownImageObj;


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

    //桌上卡牌的結構
    public class TableCard
    {
        public GameObject cardObj;
        public CardData cardData;
    }

    /// <summary>
    /// 接受到玩家丟牌
    /// </summary>
    /// <param name="sendCardData"></param>
    public void GetSendData(List<CardData> sendCardData)
    {
        //一樣檢查
        if (!cardLogic.SendCardLogic(sendCardData))
        {
            //有錯誤
            OnReportReceiveCard?.Invoke(false);
            return;
        }

        //可以出牌
        OnReportReceiveCard?.Invoke(true);
        CreateCardInTable(sendCardData);
    }


    /// <summary>
    /// 在桌面上產生牌
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
    /// 移動位置
    /// </summary>
    private void UpdateCardPositionsAndAlpha()
    {
        //牌跟牌之間的距離
        float spacing = 20f;

        for (int i = 0; i < lastCardList.Count; i++)
        {
            float targetX = (i - cardList.Count - lastCardList.Count) * spacing;

            lastCardList[i].cardObj.transform.DOLocalMoveX(mLastCardSpceing + targetX, moveDuration);

            //透明
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
