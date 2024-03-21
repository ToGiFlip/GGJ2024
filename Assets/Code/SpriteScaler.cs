using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private Camera _cam;

    void Start(){
        spriteRend = GetComponent<SpriteRenderer>();
        _cam = Camera.main;
    }
    void Update(){
        // Get sprite size in world units
        float wSpriteWidth = spriteRend.sprite.bounds.size.x;
        float wSpriteHeight = spriteRend.sprite.bounds.size.y;

        // Get screen height & width in world space units
        float wScreenHeight = _cam.orthographicSize * 2.0f;
        float wScreenWidth = (wScreenHeight / Screen.height) * Screen.width;

        // Define new scale
        Vector3 _newScale = new Vector3(wScreenWidth /wSpriteWidth, wScreenHeight / wSpriteHeight);

        // Apply scale
        transform.localScale = _newScale;
    }
}
