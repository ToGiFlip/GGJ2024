using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVFX : MonoBehaviour
{
    [SerializeField] private VFXPlayer _animToPlay;
    [SerializeField] private float _delay;
    [SerializeField] private AudioClip _soundToPlay;




    private  AudioSource _sfxPlayer;

    void Awake()
    {
        _sfxPlayer = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delay);
        _animToPlay.PlayAnimation();
        _sfxPlayer.PlayOneShot(_soundToPlay);
    }

}
