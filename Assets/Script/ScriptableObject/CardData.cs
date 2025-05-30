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

    [Header("Card Type")]
    [SerializeField] private CardTime _cardType;

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
    public Sprite Sprite { get => _sprite;}
    public GameObject GameObjectCharacter { get => _gameObjectCharacter; }
    public int Cost { get => _cost;}
    public int Life { get => _life;}
    public int Attack { get => _attack;}
    public string Description { get => _description;}
}
