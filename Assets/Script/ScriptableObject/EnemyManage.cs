using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManage : MonoBehaviour
{
    [SerializeField] private List<CardData> possibleCards;
    [SerializeField] private List<Transform> _cardsSlot = new List<Transform>();
    [SerializeField] private List<Transform> _EnemySlot = new List<Transform>();
    [SerializeField] private Transform _CardToBuy;
    [SerializeField] private float generationInterval = 30f;

    private List<GameObject> spawnedCards = new List<GameObject>();

    [SerializeField] private Image _hp;
    [SerializeField] private GameObject _EnemyUI;

    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float maxEnergy = 100f;

    private float currentHp;
    private float currentEnergy;

    public GameObject EnemyUI { get => _EnemyUI; set => _EnemyUI = value; }

    private void Update()
    {
        if (currentHp <= 0)
        {
            FindFirstObjectByType<UiManager>().Win.SetActive(true);
        }
    }

    private void Start()
    {
        currentHp = maxHp;
        currentEnergy = maxEnergy;

        UpdateUI();

        StartCoroutine(GenerateCardsRoutine());
    }

    public void LoseHp(float amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateUI();
    }

    public void LoseEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
        }
        else
        {
            float leftover = amount - currentEnergy;
            currentEnergy = 0f;
            LoseHp(leftover);
        }

        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateUI();
    }

    public void GainEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _hp.fillAmount = currentHp / maxHp;
    }


    private IEnumerator GenerateCardsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(generationInterval);

            ClearOldCards();
            GenerateNewCards();
        }
    }

    private void ClearOldCards()
    {
        foreach (var card in spawnedCards)
        {
            if (card != null)
                Destroy(card);
        }
        spawnedCards.Clear();
    }

    private void GenerateNewCards()
    {
        for (int i = 0; i < _cardsSlot.Count; i++)
        {
            var slot = _cardsSlot[i];

            if (Random.value < 0.3f)
                continue;

            CardData selectedData = possibleCards[Random.Range(0, possibleCards.Count)];

            GameObject cardGO = Instantiate(selectedData.GameObjectCard, slot);
            cardGO.transform.localPosition = Vector3.zero;

            Card cardDisplay = cardGO.GetComponent<Card>();

            if (cardDisplay != null)
            {
                cardDisplay.SetData(selectedData);
                cardDisplay.SpawCharacters = _EnemySlot[i].transform;
                cardDisplay.SetEnemy();
            }

            spawnedCards.Add(cardGO);
        }
    }
}
