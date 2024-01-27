using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSequenceManager : MonoBehaviour
{
    [SerializeField] private Sprite _firstFrame;
    [SerializeField] private float _firstFrameTime;

    [SerializeField] private Sprite _secondFrame;
    [SerializeField] private float _secondFrameTime;

    private IEnumerator Start()
    {
        FartGameManager.Instance.StartSequenceTotalTime = _firstFrameTime + _secondFrameTime;

        BackgroundManager.Instance.ChangeBackground(_firstFrame);
        yield return new WaitForSeconds(_firstFrameTime);

        BackgroundManager.Instance.ChangeBackground(_secondFrame);
        yield return new WaitForSeconds(_secondFrameTime);
    }
}
