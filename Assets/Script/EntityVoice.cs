using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityVoice : MonoSingleton<EntityVoice>
{
    
    [Header("References")]
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Animator _entityAnimator;
    
    private string[] _startingVoiceLines;
    private string[] _combatVoiceLines;

    private float _timeBetweenLines = 10f;
    private float _timeElapsed;

    private bool _hasEnteredCombat = false;

    protected override void Awake()
    {
        base.Awake();
        PopulateStartingLines();
        PopulateCombatLines();
    }
    void Start()
    {
        
    }

    public void ChangeIntoBattle()
    {
        _entityAnimator.SetTrigger("Down");
        _hasEnteredCombat = true;
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed >= _timeBetweenLines && !_hasEnteredCombat)
        {
            _timeElapsed = 0f;
            int randomLine = Random.Range(0, _startingVoiceLines.Length);
            StartCoroutine(Speak(_startingVoiceLines[randomLine]));
        }

        if (_timeElapsed >= _timeBetweenLines && _hasEnteredCombat)
        {
            _timeElapsed = 0f;
            int randomLine = Random.Range(0, _combatVoiceLines.Length);
            StartCoroutine(Speak(_combatVoiceLines[randomLine]));
        }
    }

    
    //Aqui situam-se as voicelines possíveis para mandar ao text. Mudem o número do array para refletir a quantidade de lines possíveis!!!!
    private void PopulateStartingLines()
    {
        _startingVoiceLines = new string[3];
        _startingVoiceLines[0] = "I hope you are ready to lose your soul...";
        _startingVoiceLines[1] = "Preparation is key! It could save your life...";
        _startingVoiceLines[2] = "I hope you know those cards... then again, I hope not!";

    }
    private void PopulateCombatLines()
    {
        _combatVoiceLines = new string[6];
        _combatVoiceLines[0] = "What astute thinking!... that was ironic.";
        _combatVoiceLines[1] = "When did you learn that? Or was it panic?";
        _combatVoiceLines[2] = "Hmm... you do fight back, huh?";
        _combatVoiceLines[3] = "Humans are so fascinating with the way they think...";
        _combatVoiceLines[4] = "Didn't expect that, now did you?";
        _combatVoiceLines[5] = "I'm growing tired of these simpletons...";
    }

    private IEnumerator Speak(string line)
    {
        for(int i = 0; i < line.Length; i++)
        {
            _text.text += line[i];
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        _text.text = "";
        yield return null;
    }
}
