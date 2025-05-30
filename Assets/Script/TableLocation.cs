using Unity.VisualScripting;
using UnityEngine;

public class TableLocation : MonoBehaviour, IInterectable
{
    private Material _materialInstance;
    private Color _originalColor;

    [SerializeField] private Transform _transformSpaw;
    private Card _cardInThisPlace;
    private bool haveACardInthisPlace = false;

    [SerializeField] private CardManager _cardManager;

    public Transform TransformSpaw { get => _transformSpaw;}
    public string InteractName { get => "PlaceCard";}

    void Start()
    {

    }

    void Update()
    {
        if (haveACardInthisPlace == true)
        {
            this.GetComponent<Collider>().enabled = false;
        }
        else if (haveACardInthisPlace == false)
        {
            this.GetComponent<Collider>().enabled = true;
        }
    }
    public void ReceiveCard(Card card)
    {
        _cardInThisPlace = card;
        haveACardInthisPlace = true;
    }

    public void Interact()
    {
        if (haveACardInthisPlace == false)
        {
            if (_cardManager.HaveCard == true) 
            {
                _cardManager.PlaceCardToTable(this);
            }
        }
    }

    public void ClearCardFromThisPlace()
    {
        _cardInThisPlace = null;
        haveACardInthisPlace = false;
    }
}
