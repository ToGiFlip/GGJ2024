using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSequenceManager : MonoBehaviour
{
    [SerializeField] private Sprite _firstFrame;
    [SerializeField] private float _firstFrameTime;

    [SerializeField] private Sprite _secondFrame;
    [SerializeField] private float _secondFrameTime;

    [SerializeField] private float _startDelay;

    private IEnumerator Start()
    {
        // animations for the start sequence etc. 

        BackgroundManager.Instance.ChangeBackground(_firstFrame);
        yield return new WaitForSeconds(_firstFrameTime);

        BackgroundManager.Instance.ChangeBackground(_secondFrame);
        yield return new WaitForSeconds(_secondFrameTime);

        yield return new WaitForSeconds(_startDelay);
        SceneManager.LoadScene("FartAndCough");
    }
}
