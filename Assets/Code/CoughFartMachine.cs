using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoughFartMachine : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _fartDelay;
    [SerializeField] private float _coughDelay;

    [Header("References")]
    [SerializeField] private VFXPlayer _fartVFX;
    [SerializeField] private AudioClip[] _fartsSFX;

    [SerializeField] private VFXPlayer _coughVFX;
    [SerializeField] private AudioClip[] _coughsSFX;

    [SerializeField] private GameObject _endScreen;

    private AudioSource _sfxPlayer;
    private bool _hasCoughed;
    private bool _hasFarted;

    void Awake()
    {
        _sfxPlayer = GetComponent<AudioSource>();
        _hasCoughed = false;
        _hasFarted = false;
        _endScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FinallyFart(_fartDelay));
        StartCoroutine(FinallyCough(_coughDelay));
    }

    // Update is called once per frame
    void Update()
    {
        if(_hasCoughed && _hasFarted)
        {
            _endScreen.SetActive(true);
        }
    }

    private IEnumerator FinallyFart(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);

        if (_fartVFX)
        {
            _fartVFX.PlayAnimation();
        }
        PlayRandomClipFromList(_fartsSFX);

        yield return new WaitForSeconds(2);

        _hasFarted = true;
    }

    private IEnumerator FinallyCough(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);

        if (_coughVFX)
        {
            _coughVFX.PlayAnimation();
        }
        PlayRandomClipFromList(_coughsSFX);

        yield return new WaitForSeconds(2);

        _hasCoughed = true;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("StartScene");
    }

    private void PlayRandomClipFromList(AudioClip[] listOfClips)
    {
        if(listOfClips.Length == 0) return;
        
        int randomClipIndex = Random.Range(0, listOfClips.Length);
        _sfxPlayer.pitch = Random.Range(1f, 1.2f);
        _sfxPlayer.PlayOneShot(listOfClips[randomClipIndex]);
    }
}
