using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSequenceManager : MonoBehaviour
{
    public 

    void Update()
    {
        if (FartGameManager.Instance.GameState == FartGameState.Results)
            StartCoroutine(EndSequence());
    }

    private IEnumerator EndSequence()
    {
        yield return new WaitForSeconds(1);
    }
}

public enum Result
{

}
