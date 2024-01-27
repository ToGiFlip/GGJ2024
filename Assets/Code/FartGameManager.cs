using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class FartGameManager : MonoBehaviour
{
    public static FartGameManager Instance;

    [Header("Settings")]
    [SerializeField] private float _successRange;
    [SerializeField] private float _susRange;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;

    [Header("References")]
    [SerializeField] private Transform _coughIcon;
    [SerializeField] private VFXPlayer[] _fartVFXs;
    [SerializeField] private AudioClip[] _fartSounds;
    [SerializeField] private AudioClip[] _coughSounds;

    [Header("Debug")]
    public FartGameState GameState;

    public float FartValue;
    public float MeterValue;
    public float CoughValue;

    private bool _hasCoughed;
    private bool _hasFarted;
    [SerializeField] private float _speed;

    private AudioSource _sfxPlayer;

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
        yield return new WaitForSeconds(_startDelay);

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
        //_time += Time.deltaTime;
        //MeterValue = Mathf.Pow(_time, _time);

        _speed += Time.deltaTime;
        MeterValue = Mathf.Pow(_speed, _acceleration);
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

            if(FartValue - CoughValue <= _successRange)
            {
                // success
                Debug.Log("Coughed on time");
                yield return new WaitForSeconds(_endDelay);
                // Load the successful-scene
            }
            else if (FartValue - CoughValue <= _susRange)
            {
                // success-ish
                Debug.Log("Close enough! But a bit sus maybe ???");
                yield return new WaitForSeconds(_endDelay);
                // Load the "a bit too early, but successful"-scene
            }
            else
            {
                // failure
                Debug.Log("Coughed too early!");
                yield return new WaitForSeconds(_endDelay);
                // Load the too early-scene
            }

        }
        else
        {
            if (CoughValue - FartValue <= _successRange)
            {
                // success
                Debug.Log("Coughed on time");
                yield return new WaitForSeconds(_endDelay);
                // Load the successful-scene
            }
            else if (CoughValue - FartValue <= _susRange)
            {
                // success-ish
                Debug.Log("Close enough! But a bit sus maybe ???");
                yield return new WaitForSeconds(_endDelay);
                // Load the "a bit too late, but successful"-scene
            }
            else
            {
                // failure
                Debug.Log("Coughed too late!");
                yield return new WaitForSeconds(_endDelay);
                // Load the too late-scene
            }
        }
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