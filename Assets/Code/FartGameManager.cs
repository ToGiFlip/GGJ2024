using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FartGameManager : MonoBehaviour
{
    public static FartGameManager Instance;

    [Header("Settings")]
    [SerializeField] private float _successRange;
    [SerializeField] private float _susRange;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _minAcceleration;
    [SerializeField] private float _maxAcceleration;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;

    [Header("References")]
    [SerializeField] private Transform _coughIcon;
    [SerializeField] private VFXPlayer _speedLinesVFX;
    [SerializeField] private VFXPlayer _coughVFX;
    [SerializeField] private VFXPlayer[] _fartVFXs;
    [SerializeField] private AudioClip[] _fartSounds;
    [SerializeField] private AudioClip[] _coughSounds;
    [SerializeField] private AudioClip[] _focusSounds;

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

        _acceleration = Random.Range(_minAcceleration, _maxAcceleration);

        // Delay
        yield return new WaitForSeconds(_startDelay);

        PlayRandomClipFromList(_focusSounds);

        GameState = FartGameState.GameActive;
    }

    private void Update()
    {
        //Speed Lines init
        _speedLinesVFX.PlayAnimation();


        if (GameState != FartGameState.GameActive)
            return;

        if(MeterValue >= FartValue && _hasFarted == false)
        {
            Fart();
        }

        if (MeterValue >= 100)
        {
            if (_hasCoughed == false)
            {
                if (Random.Range(0, 10) == 0)
                {
                    // Load explosion scene
                    GameState = FartGameState.Results;
                    SceneManager.LoadScene("Explosion");
                    return;
                }
                Cough();
            }

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
        
        //PlayRandomClipFromList(_fartSounds);

        // TO DO: fart vfx
    }

    private void Cough()
    {
        _hasCoughed = true;
        CoughValue = MeterValue;

        //PlayRandomClipFromList(_coughSounds);

        // TO DO: cough vfx

        _coughVFX.PlayAnimation();

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
                SceneManager.LoadScene("Success");
            }
            else if (FartValue - CoughValue <= _susRange)
            {
                // success-ish
                Debug.Log("Close enough! But a bit sus maybe ???");
                yield return new WaitForSeconds(_endDelay);
                // Load the "a bit too early, but successful"-scene
                SceneManager.LoadScene("SusEarly");
            }
            else
            {
                // failure
                Debug.Log("Coughed too early!");
                yield return new WaitForSeconds(_endDelay);
                // Load the too early-scene
                SceneManager.LoadScene("TooEarly");
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
                SceneManager.LoadScene("Success");
            }
            else if (CoughValue - FartValue <= _susRange)
            {
                // success-ish
                Debug.Log("Close enough! But a bit sus maybe ???");
                yield return new WaitForSeconds(_endDelay);
                // Load the "a bit too late, but successful"-scene
                SceneManager.LoadScene("SusLate");
            }
            else
            {
                // failure
                Debug.Log("Coughed too late!");
                yield return new WaitForSeconds(_endDelay);
                // Load the too late-scene
                SceneManager.LoadScene("TooLate");
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
