using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartGameManager : MonoBehaviour
{
    public static FartGameManager Instance;

    [Header("Settings")]
    [SerializeField] private float SuccessRange;
    [SerializeField] private float SpottedRange;
    [SerializeField] private float FailedRange;
    [SerializeField] private float StartSpeed;
    [SerializeField] private float Acceleration;

    [Header("References")]
    [SerializeField] private Transform _coughIcon;
    [SerializeField] private VFXPlayer[] _fartVFXs;
    [SerializeField] private AudioClip[] _fartSounds;
    [SerializeField] private AudioClip[] _coughSounds;

    [Header("Debug")]
    public FartGameState GameState;
    public float StartSequenceTotalTime;

    public float FartValue;
    public float MeterValue;
    public float CoughValue;

    private bool _hasCoughed;
    private bool _hasFarted;
    private float _time;

    private AudioSource _sfxPlayer;
    WaitForSeconds _oneSecondDelay = new WaitForSeconds(1);

    private void Awake()
    {
        // Initiliaze singleton
        if (Instance == null)
            Instance = this;
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
        Debug.Log("Fart");
        foreach(VFXPlayer fartVFX in _fartVFXs)
        {
            fartVFX.PlayAnimation();
        }
        PlayRandomClipFromList(_fartSounds);
        // TO DO: fart vfx
    }

    private void Cough()
    {
        _hasCoughed = true;
        CoughValue = MeterValue;

        PlayRandomClipFromList(_coughSounds);
        // TO DO: cough vfx

        _coughIcon.SetParent(_coughIcon.parent.parent.parent.parent);
    }

    private IEnumerator CheckResults()
    {
        GameState = FartGameState.Results;

        // check results
        if(CoughValue < FartValue)
        {
            // coughed before the fart

            if(FartValue - CoughValue <= SuccessRange)
            {
                // success
                Debug.Log("Coughed on time");
            }
            else if (FartValue - CoughValue <= SpottedRange)
            {
                // success!
                Debug.Log("Close enough! Somebody might have noticed");
            }
            else
            {
                // failure
                Debug.Log("Coughed too early!");
            }

        }
        else
        {
            if (CoughValue - FartValue <= SuccessRange)
            {
                // success
                Debug.Log("Coughed on time");
            }
            else if (CoughValue - FartValue <= SpottedRange)
            {
                // success!
                Debug.Log("Close enough! Somebody might have noticed");
            }
            else
            {
                // failure
                Debug.Log("Coughed too late!");
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