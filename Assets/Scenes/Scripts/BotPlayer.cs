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
        // ���ݭn�b�o����ܤ�P�A�]�� Bot ���ݭn�e���W��ܤ�P
    }

    public void PlayCard()
    {

    }
}
