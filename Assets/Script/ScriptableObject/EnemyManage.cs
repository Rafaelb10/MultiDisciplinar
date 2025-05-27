using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManage : MonoBehaviour
{
    [SerializeField] private List<CardData> possibleCards;
    [SerializeField] private List<Transform> _cardsSlot = new List<Transform>();
    [SerializeField] private Transform _CardToBuy;
    [SerializeField] private float generationInterval = 30f;

    private List<GameObject> spawnedCards = new List<GameObject>();

    [SerializeField] private Image _hp;

    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float maxEnergy = 100f;

    private float currentHp;
    private float currentEnergy;

    private void Start()
    {
        currentHp = maxHp;

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
        foreach (var slot in _cardsSlot)
        {
            if (Random.value < 0.3f)
                continue;

            CardData selectedData = possibleCards[Random.Range(0, possibleCards.Count)];

            Transform newCard = Instantiate(_CardToBuy, slot.position, slot.rotation, slot);

            Card cardDisplay = newCard.GetComponent<Card>();
            if (cardDisplay != null)
            {
                cardDisplay.SetData(selectedData);
                cardDisplay.SetEnemy();
            }

            spawnedCards.Add(newCard.gameObject);
        }
    }
}
