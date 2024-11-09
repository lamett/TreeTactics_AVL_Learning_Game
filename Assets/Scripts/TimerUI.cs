using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    Image img;
    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();
        UpdateTimer(-1);
    }

    // Update is called once per frame
    public void UpdateTimer(float percent)
    {
        if(percent < 0){
            img.enabled = false;
        } else{
            img.enabled = true;
        }
        img.color = Color.Lerp(Color.green, Color.red, (percent - 0.4f) * 2.5f);
        img.fillAmount = 1 - percent;
    }
}