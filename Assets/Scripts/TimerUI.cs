using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    Image img;
    RectTransform rt;
    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
        UpdateTimer(-1);
        transform.parent.parent.gameObject.GetComponent<Outline>().enabled = false;
    }

    // Update is called once per frame
    public void UpdateTimer(float percent)
    {
        if (percent < 0)
        {
            img.enabled = false;
        }
        else
        {
            img.enabled = true;
        }
        img.color = Color.Lerp(Color.green, Color.red, (percent - 0.4f) * 2.5f);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, Mathf.Lerp(0, 1.72f, percent));
    }
}