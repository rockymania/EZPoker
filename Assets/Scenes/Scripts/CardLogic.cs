using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 判斷出牌使用的
/// </summary>

public class CardLogic
{

    //可以接受的牌型
    private List<int> acceptCardNum = new List<int> { 1, 2, 5 };//單張、對子、順子

    //2代表的數字是15
    private const int BigTwoNumber = 2;
    private const int BigTwoCompareRank = 15;

    /// <summary>
    /// 判斷要出的卡牌是否符合規定
    /// </summary>
    /// <param name="checkCardData"></param>
    /// <returns></returns>
    public bool SendCardLogic(List<CardData> checkCardData)
    {
        //丟進來的卡牌判斷是否可以出牌

        checkCardData = checkCardData.OrderBy(cardData => cardData.cardData.CompareRank).ToList();

        if (!acceptCardNum.Contains(checkCardData.Count))
        {
            Debug.Log("卡牌數量有問題");
            return false;
        }

        else if (checkCardData.Count == 1)
        {
            return true;
        }

        else if (checkCardData.Count == 2)
        {
            if (checkCardData[0].cardData.CompareRank == checkCardData[1].cardData.CompareRank)
                return true;
        }

        else if (checkCardData.Count == 5)
        {
            // 判斷是否有葫蘆
            if (IsFullHouse(checkCardData))
            {
                return true;
            }



            bool listHaveTwo = false;

            if (checkCardData.Any(cardData => cardData.cardData.Rank == BigTwoNumber))
            {
                listHaveTwo = true;
            }

            //取得第一個數字
            int tempCount = checkCardData[0].cardData.CompareRank;

            //如果數字大1就開始檢查順子
            if (tempCount + 1 == checkCardData[1].cardData.CompareRank)
            {
                //判斷順子
                for (int i = 2; i < checkCardData.Count; i++)
                {
                    if (listHaveTwo)
                    {
                        //如果有2的話，就判斷前面四張是不是3.4.5.6.2(15)
                        if(checkCardData[0].cardData.Number != 3 || checkCardData[1].cardData.Number != 4 || checkCardData[2].cardData.Number != 5 || checkCardData[3].cardData.Number != 6)
                        {
                                Debug.Log("卡牌有問題");
                                return false;
                        }
                    }
                    else
                    {
                        if (tempCount + 1 == checkCardData[i].cardData.CompareRank)
                        {
                            // 如果連續，繼續判斷下一張牌
                            tempCount = checkCardData[i].cardData.CompareRank;
                        }
                        else
                        {
                            // 如果不連續，這不是順子
                            Debug.Log("卡牌有問題");
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                for (int i = 0; i <= checkCardData.Count - 4; i++)
                {
                    if (checkCardData[i].cardData.CompareRank == checkCardData[i + 1].cardData.CompareRank &&
                        checkCardData[i].cardData.CompareRank == checkCardData[i + 2].cardData.CompareRank &&
                        checkCardData[i].cardData.CompareRank == checkCardData[i + 3].cardData.CompareRank)
                    {
                        return true;
                    }else
                    {
                        // 如果不連續，這不是順子
                        Debug.Log("卡牌有問題");
                        return false;
                    }
                }
            }


        }

        Debug.Log("無法出牌");
        return false;
    }

    /// <summary>
    /// 判斷葫蘆
    /// </summary>
    /// <param name="checkCardData"></param>
    /// <returns></returns>
    private bool IsFullHouse(List<CardData> checkCardData)
    {
        // 先判斷是否有三條
        for (int i = 0; i <= checkCardData.Count - 3; i++)
        {
            if (checkCardData[i].cardData.CompareRank == checkCardData[i + 1].cardData.CompareRank &&
                checkCardData[i].cardData.CompareRank == checkCardData[i + 2].cardData.CompareRank)
            {
                // 找到三條，再找一對
                for (int j = 0; j <= checkCardData.Count - 2; j++)
                {
                    if (j != i && j != i + 1 && j != i + 2 &&
                        checkCardData[j].cardData.CompareRank == checkCardData[j + 1].cardData.CompareRank)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


    /// <summary>
    /// 透過丟進來的卡牌判斷大小
    /// </summary>
    /// <returns></returns>
    public bool CompareCard()
    {


        return false;
    }

}
