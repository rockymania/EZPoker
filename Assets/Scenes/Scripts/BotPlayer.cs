using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class BotPlayer : MonoBehaviour
{
    private List<Card> hand = new List<Card>();

    public void ReceiveHand(List<Card> cards)
    {
        hand = cards;
        // 不需要在這裡顯示手牌，因為 Bot 不需要畫面上顯示手牌
    }

    public void PlayCard()
    {

    }
}
