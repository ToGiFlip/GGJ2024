using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioDelayed : MonoBehaviour
{
    [SerializeField] float _delay;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(PlayAudioWithDelay());
    }

    private IEnumerator PlayAudioWithDelay()
    {
        yield return new WaitForSeconds(_delay);
        _audioSource.Play();
    }
}
