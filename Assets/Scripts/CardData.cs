using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using static GameData;

/// <summary>
/// 撲克牌設定
/// </summary>
public class CardData : MonoBehaviour
{
    private Card mCardData;

    public Card cardData { get { return mCardData; } }
    private Image CardSprite;
    private Button cardButton;
    private bool mIsSelect = false;

    public delegate void CardClickEvent(int cardID);
    public event CardClickEvent OnCardClick;

    private float mClickMoveSpace = 20.0f;

    private CanvasGroup canvasGroup;

    private bool isListenerAdded = false;

    public void FadeOut(float duration, float targetAlpha)
    {
        // 如果需要，這裡可以加入淡出動畫
        // 這個例子中使用了 DOFade 方法，需要 DOTween
        canvasGroup.DOFade(targetAlpha, duration);
    }

    private void SetSelect()
    {
        mIsSelect = !mIsSelect;

        if(mIsSelect)
        {
            Vector3 newPosition = this.transform.localPosition;
            transform.DOLocalMoveY(newPosition.y+mClickMoveSpace, 0.5f);
        }
        else
        {
            Vector3 newPosition = this.transform.localPosition;
            transform.DOLocalMoveY(newPosition.y - mClickMoveSpace, 0.2f);
        }
    }


    public void SetCardData(Card card,bool isShowBack = false)
    {
        mCardData = card;

        string loadString = mCardData.Number.ToString("D2");

        CardSprite.sprite = Resources.Load<Sprite>(loadString);

        if(isShowBack)
            CardSprite.sprite = Resources.Load<Sprite>("back");

        if (!isListenerAdded)
        {
            cardButton = gameObject.AddComponent<Button>();
            cardButton.onClick.AddListener(CardClick);
            isListenerAdded = true;
        }
        mIsSelect = false;

        SetCardActive(true);
    }


    private void CardClick()
    {
        // 調用 UnityEvent 以通知外部註冊的方法
        OnCardClick?.Invoke(mCardData.Number);

        SetSelect();

    }

    public void Reset()
    {
        if(CardSprite == null)
        {
            CardSprite = GetComponent<Image>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        mCardData = null;
        SetCardActive(false);
    }

    public void SetCardActive(bool IsShowCard)
    {
        this.gameObject.SetActive(IsShowCard);
    }


}
