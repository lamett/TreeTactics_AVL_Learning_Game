using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    Image img;
    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();
        GetComponent<Timer>().startTimer(5, 0.2f);
    }

    // Update is called once per frame
    public void UpdateTimer(float percent)
    {
        img.color = Color.Lerp(Color.green, Color.red, (percent - 0.4f) * 2.5f);
        img.fillAmount = 1 - percent;
    }
}