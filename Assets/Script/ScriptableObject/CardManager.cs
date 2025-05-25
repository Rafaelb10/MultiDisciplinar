using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<CardData> possibleCards;
    [SerializeField] private List<Transform> _cardsSlot = new List<Transform>();

    private List<Card> _cardsInHand = new List<Card>();

    private const int _cardsInHandLimit = 5;

    private Card _insuredCard;
    private bool _haveCard = false;


    private void Start()
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
