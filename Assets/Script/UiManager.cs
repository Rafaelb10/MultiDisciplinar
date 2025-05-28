using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private GameObject _wall;
    [SerializeField ]private Material _material;

    [SerializeField] private bool _start;
    [SerializeField] private bool _gainEnergy;

    [SerializeField] private GameObject _win;
    [SerializeField] private GameObject _lose;

    [SerializeField] private Image _energy;
    [SerializeField] private Image _hp;

    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float maxEnergy = 100f;

    private float currentHp;
    private float currentEnergy;

    public bool StartBattle { get => _start;}

    void Start()
    {
        currentHp = maxHp;
        currentEnergy = maxEnergy;

        UpdateUI();

        _timer.text = "00:30";
        StartCoroutine(PreGameCountdown());
        StartCoroutine(RecoverEnergy()); //verificar
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
        _hp.fillAmount = currentHp / maxHp;
        _energy.fillAmount = currentEnergy / maxEnergy;
    }

    private IEnumerator PreGameCountdown()
    {
        int totalSeconds = 10;

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
        float number = 2f;
        float numberToFinish = -1f;
        _material.SetFloat("CutoffHeight", Mathf.Lerp(number, numberToFinish, 10f));
        yield return new WaitForSeconds(10f);
        _wall.SetActive(false);
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