using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameData;

/// <summary>
/// �P�_�X�P�ϥΪ�
/// </summary>

public class CardLogic
{



    //�i�H�������P��
    private List<int> acceptCardNum = new List<int> { 1, 2, 5 };//��i�B��l�B���l

    //2�N���Ʀr�O15
    private const int BigTwoNumber = 2;
    private const int BigTwoCompareRank = 15;




    /// <summary>
    /// �P�_�n�X���d�P�O�_�ŦX�W�w
    /// </summary>
    /// <param name="checkCardData"></param>
    /// <returns></returns>
    public CheckCardLogin SendCardLogic(List<CardData> checkCardData)
    {
        //��i�Ӫ��d�P�P�_�O�_�i�H�X�P

        CheckCardLogin checkCardLogin = new CheckCardLogin();

        checkCardData = checkCardData.OrderBy(cardData => cardData.cardData.CompareRank).ToList();

        if (!acceptCardNum.Contains(checkCardData.Count))
        {
            Debug.Log("�d�P�ƶq�����D");
            return checkCardLogin;
        }

        else if (checkCardData.Count == 1)
        {
            checkCardLogin.SendCardLogic = true;
            checkCardLogin.SendCardType = CardType.Single;
            return checkCardLogin;
        }

        else if (checkCardData.Count == 2)
        {
            if (checkCardData[0].cardData.CompareRank == checkCardData[1].cardData.CompareRank)
            {
                checkCardLogin.SendCardLogic = true;
                checkCardLogin.SendCardType = CardType.Pair;

                return checkCardLogin;
            }
        }

        else if (checkCardData.Count == 5)
        {
            // �P�_�O�_����Ī
            if (IsFullHouse(checkCardData))
            {
                checkCardLogin.SendCardLogic = true;
                checkCardLogin.SendCardType = CardType.FullHouse;
                return checkCardLogin;
            }



            bool listHaveTwo = false;

            if (checkCardData.Any(cardData => cardData.cardData.Rank == BigTwoNumber))
            {
                listHaveTwo = true;
            }

            //���o�Ĥ@�ӼƦr
            int tempCount = checkCardData[0].cardData.CompareRank;

            //�p�G�Ʀr�j1�N�}�l�ˬd���l
            if (tempCount + 1 == checkCardData[1].cardData.CompareRank)
            {
                tempCount = checkCardData[1].cardData.CompareRank;

                //�P�_���l
                for (int i = 2; i < checkCardData.Count; i++)
                {
                    if (listHaveTwo)
                    {
                        //�p�G��2���ܡA�N�P�_�e���|�i�O���O3.4.5.6.2(15)
                        if(checkCardData[0].cardData.CompareRank != 3 || checkCardData[1].cardData.CompareRank != 4 || checkCardData[2].cardData.CompareRank != 5 || checkCardData[3].cardData.CompareRank != 6)
                        {
                                Debug.Log("�d�P�����D");
                                return checkCardLogin;
                        }
                    }
                    else
                    {
                        if (tempCount + 1 == checkCardData[i].cardData.CompareRank)
                        {
                            // �p�G�s��A�~��P�_�U�@�i�P
                            tempCount = checkCardData[i].cardData.CompareRank;
                        }
                        else
                        {
                            // �p�G���s��A�o���O���l
                            Debug.Log("�d�P�����D");
                            return checkCardLogin;
                        }
                    }
                }

                checkCardLogin.SendCardLogic = true;
                checkCardLogin.SendCardType = CardType.Straight;

                return checkCardLogin;
            }
            else
            {
                for (int i = 0; i <= checkCardData.Count - 4; i++)
                {
                    if (checkCardData[i].cardData.CompareRank == checkCardData[i + 1].cardData.CompareRank &&
                        checkCardData[i].cardData.CompareRank == checkCardData[i + 2].cardData.CompareRank &&
                        checkCardData[i].cardData.CompareRank == checkCardData[i + 3].cardData.CompareRank)
                    {
                        checkCardLogin.SendCardLogic = true;
                        checkCardLogin.SendCardType = CardType.IronCard;

                        return checkCardLogin;
                    }else
                    {
                        // �p�G���s��A�o���O���l
                        Debug.Log("�d�P�����D");
                        return checkCardLogin;
                    }
                }
            }


        }

        Debug.Log("�L�k�X�P");
        return checkCardLogin;
    }

    /// <summary>
    /// �P�_��Ī
    /// </summary>
    /// <param name="checkCardData"></param>
    /// <returns></returns>
    private bool IsFullHouse(List<CardData> checkCardData)
    {
        // ���P�_�O�_���T��
        for (int i = 0; i <= checkCardData.Count - 3; i++)
        {
            if (checkCardData[i].cardData.CompareRank == checkCardData[i + 1].cardData.CompareRank &&
                checkCardData[i].cardData.CompareRank == checkCardData[i + 2].cardData.CompareRank)
            {
                // ���T���A�A��@��
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
    /// �z�L��i�Ӫ��d�P�P�_�j�p
    /// </summary>
    /// <returns></returns>
    public bool CompareCard(Card aCard,Card bCard)
    {
        if (aCard.Number > bCard.Number)
            return true;

        if(aCard.Number == bCard.Number)
        {
            if (aCard.Suit > bCard.Suit)
                return true;
        }

        return false;
    }

}
