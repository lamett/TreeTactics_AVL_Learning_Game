using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuScript : MonoBehaviour
{

    public GameObject OptionsWindow;

    public Slider VolumeSlider;

    private void Start()
    {
        VolumeSlider.onValueChanged.AddListener(delegate {ChangeVolume(); });
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level0Scene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        OptionsWindow.active = true;
    }

    public void CloseOptions()
    {
        OptionsWindow.active = false;
    }
    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        
    }

}
