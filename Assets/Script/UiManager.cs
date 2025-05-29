using System;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI interacThintText;
    
    
    [SerializeField] private GameObject _wall;
    
    [SerializeField] private bool _start;
    [SerializeField] private bool _gainEnergy;

    [SerializeField] private GameObject _win;
    [SerializeField] private GameObject _lose;

    [SerializeField] private Image _energy;
    [SerializeField] private Image _hp;

    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float maxEnergy = 100f;

    [SerializeField] private GameObject _cardView;
    [SerializeField] private TextMeshProUGUI _lifeCard;
    [SerializeField] private TextMeshProUGUI _damageCard;
    [SerializeField] private TextMeshProUGUI _descriptionCard;
    [SerializeField] private Image _spriteCard;
    [SerializeField] private float lerpSpeed = 2f;
    private float currentHp;
    private float currentEnergy;
    private float displayedEnergy;
    
    public bool StartBattle { get => _start;}

    void Start()
    {
        currentHp = maxHp;
        currentEnergy = maxEnergy;
        displayedEnergy = 1f;
        UpdateUI();

        _timer.text = "00:30";
        StartCoroutine(PreGameCountdown());
        StartCoroutine(RecoverEnergy());
    }
    
    private void UpdateTimerDisplay(int seconds)
    {
        int minutes = seconds / 60;
        int secs = seconds % 60;
        _timer.text = $"{minutes:00}:{secs:00}";
    }

    public void StartGame()
    {
        _start = true;
        _gainEnergy = true;
        StartCoroutine(WallFall());
    }

    public void EndGame()
    {
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
        float targetFill =  currentHp / maxHp;
        //_hp.fillAmount = currentHp / maxHp;
        //_energy.fillAmount = currentEnergy / maxEnergy;
        displayedEnergy = Mathf.Lerp(currentEnergy, targetFill, Time.deltaTime * lerpSpeed);
        _energy.fillAmount = displayedEnergy;
    }

    private IEnumerator PreGameCountdown()
    {
        int totalSeconds = 30;

        while (totalSeconds > 0)
        {
            UpdateTimerDisplay(totalSeconds);
            yield return new WaitForSeconds(1f);
            totalSeconds--;
        }

        UpdateTimerDisplay(0);
        StartGame();

        StartCoroutine(GameCountdown());
    }

    private IEnumerator WallFall()
    {
        EntityVoice.Instance.StartCombatMode();
        _wall.GetComponent<Animator>().SetTrigger("Fall");
        yield return new WaitForSeconds(1.1f);

        _wall.SetActive(false);
        FindFirstObjectByType<EnemyManage>().EnemyUI.SetActive(true);
    }

    public void UpdateInteract(bool state, string interactName="")
    {
        interacThintText.enabled = state;
        interacThintText.text = interactName;
    }

    public void ViewCard(bool state, int life, int attack, string description,Sprite _card)
    {
        _cardView.SetActive(state);
        _lifeCard.text = Convert.ToString(life);
        _damageCard.text = Convert.ToString(attack); ;
        _descriptionCard.text = description;
        _spriteCard.sprite = _card;
    }

    private IEnumerator RecoverEnergy()
    {
        _gainEnergy = false;
        yield return new WaitForSeconds(30f);
        GainEnergy(25f);
        _gainEnergy = true;
    }

    private IEnumerator GameCountdown()
    {
        int totalSeconds = 5 * 60;

        while (totalSeconds > 0)
        {
            UpdateTimerDisplay(totalSeconds);
            yield return new WaitForSeconds(1f);
            totalSeconds--;
        }

        UpdateTimerDisplay(0);
        EndGame();
    }
}