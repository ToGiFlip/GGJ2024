using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSequenceManager : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite _firstFrame;
    [SerializeField] private float _firstFrameTime;

    [SerializeField] private Sprite _secondFrame;
    [SerializeField] private float _secondFrameTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator Start()
    {
        FartGameManager.instance.StartSequenceTotalTime = _firstFrameTime + _secondFrameTime;

        _spriteRenderer.sprite = _firstFrame;
        yield return new WaitForSeconds(_firstFrameTime);

        _spriteRenderer.sprite = _secondFrame;
        yield return new WaitForSeconds(_secondFrameTime);
    }
}
