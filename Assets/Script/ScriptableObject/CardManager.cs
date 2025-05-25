using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<CardData> possibleCards;

    private List<Card> _cardsInHand = new List<Card>();

    private const int _cardsInHandLimit = 5;

    private Card _insuredCard;
    private bool _haveCard = false;

    [SerializeField] private List<Transform> _cardsSlot = new List<Transform>();
    public void BuyCard()
    {
    }

    public void GenerateCard()
    {
    }

    public void PlaceCard()
    {
    }

    private void PlaceCardsInSlots()
    {
    }

    public void SelectCard()
    {
    }

    public void RemoveCard()
    {
    }
}
