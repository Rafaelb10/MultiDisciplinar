using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<CardData> possibleCards;
    [SerializeField] private List<Transform> _cardsSlot = new List<Transform>();

    [SerializeField] private Transform _CardToBuy;

    private List<Card> _cardsInHand = new List<Card>();
    private const int _cardsInHandLimit = 5;

    private Card _holdCard;
    private bool _haveCard = false;

    public bool HaveCard { get => _haveCard; }

    public List<Transform> CardsSlot { get => _cardsSlot; }
    public List<Card> CardsInHand { get => _cardsInHand;}

    private void Start()
    {
        StartCoroutine(GenerateCardEveryMinute());
    }

    private IEnumerator GenerateCardEveryMinute()
    {
        while (true)
        {
            GenerateCard();
            yield return new WaitForSeconds(1f);
        }
    }

    public void SelectCard(Card card)
    {
        if (CardsInHand.Count >= _cardsInHandLimit)
            return; 

        if (CardsSlot.Count <= CardsInHand.Count)
            return; 

        CardsInHand.Add(card);
        PlaceCardsInSlots();

        card.transform.SetParent(null);
    }

    public void GenerateCard()
    {
        if (_CardToBuy.childCount > 0)
        {
            foreach (Transform child in _CardToBuy)
            {
                Destroy(child.gameObject);
            }
        }

        if (possibleCards.Count == 0) return;

        CardData selectedData = possibleCards[Random.Range(0, possibleCards.Count)];
        GameObject cardGO = Instantiate(selectedData.GameObjectCard, _CardToBuy);
        cardGO.transform.localPosition = Vector3.zero;

        Card cardComponent = cardGO.GetComponent<Card>();
        if (cardComponent != null)
        {
            cardComponent.SetData(selectedData);
        }
    }

    private void PlaceCardsInSlots()
    {
        for (int i = 0; i < CardsInHand.Count; i++)
        {
            Card card = CardsInHand[i];
            if (i >= CardsSlot.Count) break;

            Transform slot = CardsSlot[i];
            card.transform.SetParent(slot);
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = Vector3.one; 
        }
    }
    public void HoldCard(Card card)
    {
        _holdCard = card;
        _haveCard = true;
    }

    public void PlaceCardToTable(TableLocation location)
    {
        if (_holdCard == null || !_haveCard) return;

        if (CardsInHand.Contains(_holdCard))
        {
            CardsInHand.Remove(_holdCard);
        }
        _holdCard.transform.SetParent(location.transform);
        _holdCard.transform.localPosition = Vector3.zero;
        _holdCard.transform.localRotation = Quaternion.identity;
        _holdCard.transform.localScale = Vector3.one;

        _holdCard.SpawCharacters = location.TransformSpaw;
        _holdCard.InTheTable = true;
        location.ReceiveCard(_holdCard);

        _holdCard = null;
        _haveCard = false;

        PlaceCardsInSlots(); 
    }

}
