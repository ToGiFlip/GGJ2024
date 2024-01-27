using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartGameManager : MonoBehaviour
{
    public static FartGameManager instance;

    [Header("References")]
    [SerializeField] private Transform _coughIcon;
    [SerializeField] private AudioClip[] _fartSounds;
    [SerializeField] private AudioClip[] _coughSounds;

    [Header("Debug")]
    public FartGameState GameState;
    public float StartSequenceTotalTime;

    public float FartValue;
    public float MeterValue;

    private bool _hasCoughed;
    private bool _hasFarted;
    private float _time;
    [SerializeField] private float _coughTime;
    [SerializeField] private float _fartTime;

    private AudioSource _sfxPlayer;
    WaitForSeconds _oneSecondDelay = new WaitForSeconds(1);

    private void Awake()
    {
        // Initiliaze singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        _sfxPlayer = GetComponent<AudioSource>();

        _hasCoughed = false;
        _hasFarted = false;

        FartValue = Random.Range(20, 90);
    }
    
    private IEnumerator Start()
    {
        GameState = FartGameState.StartSequence;

        // Delay
        yield return new WaitForSeconds(StartSequenceTotalTime);

        GameState = FartGameState.GameActive;
    }

    private void Update()
    {
        if (GameState != FartGameState.GameActive)
            return;

        if(MeterValue >= FartValue && _hasFarted == false)
        {
            Fart();
        }

        if (MeterValue >= 100)
        {
            if(_hasCoughed == false)
                Cough();

            StartCoroutine(CheckResults());
            return;
        }

        if (SpaceIsPressed())
        {
            Cough();
            return;
        }

        UpdateMeterValue();
    }

    private void UpdateMeterValue()
    {
        _time += Time.deltaTime;
        MeterValue = Mathf.Pow(_time, _time);
    }

    private bool SpaceIsPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void Fart()
    {
        _hasFarted = true;
        _fartTime = _time;
        Debug.Log("Fart");
        PlayRandomClipFromList(_fartSounds);
        // TO DO: fart vfx
    }

    private void Cough()
    {
        _hasCoughed = true;
        _coughTime = _time;
        Debug.Log($"Coughed at < {MeterValue}");

        PlayRandomClipFromList(_coughSounds);
        // TO DO: cough vfx

        _coughIcon.parent = _coughIcon.parent.parent.parent.parent;
    }

    private IEnumerator CheckResults()
    {
        GameState = FartGameState.Results;

        // check results
        if(_coughTime < _fartTime)
        {
            // coughed before the fart

            if(_fartTime - _coughTime > 0.1f)
            {
                // failure
                Debug.Log("Coughed too early");
            }
            else
            {
                // success!
                Debug.Log("Coughed on time");
            }
        }
        else
        {
            // coughed after the fart

            if (_coughTime - _fartTime > 0.1f)
            {
                // failure
                Debug.Log("Coughed too late");
            }
            else
            {
                // success!
                Debug.Log("Coughed on time");
            }
        }

        // Transition delay
        yield return _oneSecondDelay;

        // TO DO: to celebrate or to not celebrate? that is the question
    }

    private void PlayRandomClipFromList(AudioClip[] listOfClips)
    {
        int randomClipIndex = Random.Range(0, listOfClips.Length);
        _sfxPlayer.pitch = Random.Range(1f, 1.2f);
        _sfxPlayer.PlayOneShot(listOfClips[randomClipIndex]);
    }
}

public enum FartGameState
{
    StartSequence,
    GameActive,
    Results
}