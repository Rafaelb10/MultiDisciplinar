using UnityEngine;

public class TableLocation : MonoBehaviour, IInterectable
{
    private Material _materialInstance;
    private Color _originalColor;

    private Transform _transformSpaw;
    private Card _cardInThisPlace;
    private bool haveACardInthisPlace = false;

    [SerializeField] private CardManager _cardManager;

    void Start()
    {

    }

    void Update()
    {

    }
    public void ReceiveCard(Card card)
    {
        _cardInThisPlace = card;
        haveACardInthisPlace = true;
    }

    public void Interect()
    {
        if (haveACardInthisPlace == false)
        {
            if (_cardManager.HaveCard == true) 
            {
                _cardManager.PlaceCardToTable(this);
            }
        }
    }

    public void PossibleToInterect()
    {
        if (_cardManager.HaveCard == true)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                Color baseColor = renderer.material.GetColor("_BaseColor");
                Color bluish = new Color(baseColor.r * 0.8f, baseColor.g * 0.8f, 1f, baseColor.a);
                renderer.material.SetColor("_BaseColor", bluish);
            }
        }
    }

    public void ResetColor()
    {
        if (_materialInstance != null)
        {
            _materialInstance.SetColor("_BaseColor", _originalColor);
        }
    }

    public void ClearCardFromThisPlace()
    {
        _cardInThisPlace = null;
        haveACardInthisPlace = false;
    }
}
