using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, emptyHeart;
    Image heartimage;

    private void Awake()
    {
        heartimage = GetComponent<Image>();


    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.empty:
                heartimage.sprite = emptyHeart;
                break;
            case HeartStatus.full:
                heartimage.sprite = fullHeart;
                break;

        }
    }

}

public enum HeartStatus
{
    empty = 0,
    full = 1
}
