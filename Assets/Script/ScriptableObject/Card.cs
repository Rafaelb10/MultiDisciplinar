using UnityEngine;

public class Card : IInterectable
{
    public CardData Data { get; private set; }

    public Card(CardData data)
    {
        Data = data;
    }

    public void Interect()
    {

    }

    public void PossibleToInterect()
    {

    }

    public void ResetColor()
    {

    }
}
