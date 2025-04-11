using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = PlayerReferences.instance.GetSpriteRenderer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// You need to send the player input direction
    /// </summary>
    /// <param name="playerDirection"></param>
    public void Flip(Vector2 playerDirection)
    {
        if (playerDirection.x > 0)
        {
            spriteRenderer.transform.rotation = new Quaternion(0, 0, 0, 0);
            PlayerReferences.instance.SetPlayerLookingDirection(1);
            //throwingPos.transform.localPosition = new Vector2(Mathf.Abs(throwingPos.transform.localPosition.x), throwingPos.transform.localPosition.y);

        }
        else if (playerDirection.x < 0)
        {
            spriteRenderer.transform.rotation = new Quaternion(0, 180, 0, 0);
            PlayerReferences.instance.SetPlayerLookingDirection(-1);
            //throwingPos.transform.localPosition = new Vector2(throwingPos.transform.localPosition.x < 0 ? throwingPos.transform.localPosition.x : -throwingPos.transform.localPosition.x, throwingPos.transform.localPosition.y);

        }
    }
}
