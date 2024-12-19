using UnityEngine;
using UnityEngine.UI;

public class showTreeBalance : MonoBehaviour
{
    RectTransform rt;
    Image img;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
    }

    public void updateTreeBalance(int balanceFactor)
    {
        var balance = Mathf.Clamp(balanceFactor, 0, 20);
        
        var pos = new Vector2(rt.offsetMax.x, balance * 0.05f);
        rt.offsetMax = pos;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, balance *0.1f);


        var color = Color.HSVToRGB(Mathf.Lerp(0.21f,0, balance/10f),1,1);
        //Color.Lerp(new Color(190,255,0), Color.red, (balance-1)/10f);
        if(balance == 0)
        {
            audioManager.PlayBing(audioManager.TreeBalanced);
            color = Color.green;
        }
        img.color = color;
    }
}
