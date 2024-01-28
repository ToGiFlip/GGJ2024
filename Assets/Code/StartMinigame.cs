using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMinigame : MonoBehaviour
{
    [SerializeField] private VFXPlayer _classmatesVFX;

    [SerializeField] private float _waitTime;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_waitTime);
        SceneManager.LoadScene("FartAndCough");
    }

    // Update is called once per frame
    void Update()
    {
        _classmatesVFX.PlayAnimation();
    }
}
