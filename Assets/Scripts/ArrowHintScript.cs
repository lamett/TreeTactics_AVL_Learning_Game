using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHintScript : MonoBehaviour
{
    public Sprite straightSprite;
    public Sprite curvecSprite;
    new SpriteRenderer renderer;

    void Awake()
    {
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void setCurvedSprite()
    {
        renderer.sprite = curvecSprite;
    }

    public void setStraightSprite()
    {
        renderer.sprite = straightSprite;
    }
}
