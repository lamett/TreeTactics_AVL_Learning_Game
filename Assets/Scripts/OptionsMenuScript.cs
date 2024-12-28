using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    public GameObject optionsMenuUI;
    public Toggle showBF;
    public Toggle ShowArrow;
    public Slider volumeSlider;

    void Start()
    {
        showBF.isOn = Settings.ShowBalanceFactor;
        ShowArrow.isOn = Settings.ShowArrowHint;
        volumeSlider.value = Settings.Volume;
    }

    public void closeUI()
    {
        optionsMenuUI.SetActive(false);
    }

    public void changeBalanceFactor(bool balanceFactor)
    {
        Settings.ShowBalanceFactor = balanceFactor;
    }

    public void changeArrowHint(bool arrowHint)
    {
        Settings.ShowArrowHint = arrowHint;
    }

    public void ChangeVolume(float volume)
    {
        Settings.Volume = volume;
        AudioListener.volume = volume;
    }
}
