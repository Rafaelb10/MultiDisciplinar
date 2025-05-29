using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour, IInterectable
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
    public string InteractName { get => GetStatus(); }

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

        _spriteImage.sprite = Data.Sprite;

    }

    private void Update()
    {
        if (_status == 2 && Object.FindFirstObjectByType<UiManager>().StartBattle == true && _invoked == false && InTheTable == true)
        {
            GameObject character = Data.GameObjectCharacter;
            character.transform.localScale = new Vector3(1, 1, 1);
            Instantiate(character, _spawCharacters);
            Object.FindFirstObjectByType<UiManager>().LoseEnergy(Data.Cost);
            StartCoroutine(CooldownToInvoke());
        }

        if (_master == 1 && Object.FindFirstObjectByType<UiManager>().StartBattle == true && _invoked == false)
        {
            GameObject character = Data.GameObjectCharacter;
            character.transform.localScale = new Vector3(1f, 1f, 1f);
            Instantiate(character, _spawCharacters);
            Object.FindFirstObjectByType<EnemyManage>().LoseEnergy(Data.Cost);
            StartCoroutine(CooldownToInvoke());
        }
    }
    public string GetStatus()
    {
        switch(_status)
        {
            case 0:
            return "Buy";
            case 1:
            return "Pick";
            case 2:
            return "Place";
            default:
            return "";
        }
    }
    public void Interact()
    {
        if (_master == 0)
        {
            if (_status == 0 && Object.FindFirstObjectByType<CardManager>().CardsInHand.Count < 5)
            {
                _status = 1;
                Object.FindFirstObjectByType<CardManager>().SelectCard(this);
                return;
            }

            if (_status == 1)
            {
                _status = 2;
                Object.FindFirstObjectByType<CardManager>().HoldCard(this);
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

    public void ReturnStatus()
    {
        _status = 1;
    }
}