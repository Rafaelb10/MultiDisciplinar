using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour ,IInterectable
{
    [SerializeField] private Image _spriteImage;
    public CardData Data { get; private set; }

    private Material _materialInstance;
    private Color _originalColor;
    private int _status = 0;

    private Transform _spawCharacters;
    private int _master = 0;
    private bool _invoked;
    private bool _inTheTable;

    public Transform SpawCharacters { get => _spawCharacters; set => _spawCharacters = value; }
    public bool InTheTable { get => _inTheTable; set => _inTheTable = value; }

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

    private void Update()
    {
        if (_status == 2 && FindObjectOfType<UiManager>().StartBattle == true && _invoked == false && InTheTable == true)
        {
            GameObject character = Data.GameObjectCharacter;
            character.transform.localScale = new Vector3(1, 1, 1);
            Instantiate(character, _spawCharacters);
            FindObjectOfType<UiManager>().LoseEnergy(Data.Cost);
            StartCoroutine(CooldownToInvoke());
        }

        if (_master == 1 && FindObjectOfType<UiManager>().StartBattle == true && _invoked == false) 
        {
            GameObject character = Data.GameObjectCharacter;
            character.transform.localScale = new Vector3(0.05f,0.05f,0.05f);
            Instantiate(character, Data.GameObjectCharacter.transform);
            FindObjectOfType<EnemyManage>().LoseEnergy(Data.Cost);
            StartCoroutine(CooldownToInvoke());
        }
    }
    public void Interect() 
    {
        if (_master == 0)
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

    private IEnumerator CooldownToInvoke()
    {  
        _invoked = true;
       yield return new WaitForSeconds(Random.Range(10f, 20f));
        _invoked = false;
    }

    public void SetEnemy()
    {
        _master = 1;
    }
}