using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        // Initiliaze singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeBackground(Sprite newBackground)
    {
        _spriteRenderer.sprite = newBackground;
    }
}
