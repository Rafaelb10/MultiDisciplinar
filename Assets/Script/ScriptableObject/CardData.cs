using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public enum CardTime
    {
        Present,
        Past,
        Future,
        Monstrous
    }

    public enum CardMaster
    {
        Player,
        Bot
    }

    [Header("Card Type")]
    [SerializeField] private CardTime _cardType;
    private CardMaster _cardMaster;

    [Header("Normal")]
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _gameObjectCard;
    [SerializeField] private GameObject _gameObjectCharacter;

    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [SerializeField] private int _cost;

    [Header("Character attributes")]
    [SerializeField] private int _life;
    [SerializeField] private int _attack;
    [SerializeField] private int _velocity;

    public GameObject GameObjectCard { get => _gameObjectCard; set => _gameObjectCard = value; }
}
