using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour ,IInterectable
{
    [SerializeField] private Image _spriteImage;
    public CardData Data { get; private set; }
    private Material _materialInstance;
    private Color _originalColor;
    private int _status = 0;


    public void SetData(CardData data)
    {
        Data = data;
    }


    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            _materialInstance = renderer.material;
            _originalColor = _materialInstance.GetColor("_BaseColor");
        }

        _spriteImage.sprite =  Data.Sprite;

    }

    public void Interect() 
    {
        if (_status == 0 && FindObjectOfType<CardManager>().CardsInHand.Count < 5)
        {
            _status = 1;
            FindObjectOfType<CardManager>().SelectCard(this);
            return;
        }

        if (_status == 1 && FindObjectOfType<CardManager>().HaveCard == false)
        {
            _status = 2;
            FindObjectOfType<CardManager>().HoldCard(this);
            return;
        }

        if (_status == 2)
        {
            TableLocation table = GetComponentInParent<TableLocation>();
            if (table != null)
            {
                Destroy(this.gameObject);
                table.ClearCardFromThisPlace();
            }
            return;
        }

    }

    public void PossibleToInterect()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color baseColor = renderer.material.GetColor("_BaseColor");
            Color bluish = new Color(baseColor.r * 0.8f, baseColor.g * 0.8f, 1f, baseColor.a);
            renderer.material.SetColor("_BaseColor", bluish);
        }
    }

    public void ResetColor()
    {
        if (_materialInstance != null)
        {
            _materialInstance.SetColor("_BaseColor", _originalColor);
        }
    }
}