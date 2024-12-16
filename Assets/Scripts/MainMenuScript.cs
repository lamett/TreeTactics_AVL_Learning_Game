using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public GameObject OptionsWindow;

    void Awake()
    {
        //gets called once in the beginning
        if (!Settings.DidItRun)
        {
            Settings.ShowBalanceFactor = true;
            Settings.ShowArrowHint = true;
            Settings.Volume = 0.5f;
            Settings.DidItRun = true;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Cutscene_Start");
    }

    public void StartTutorial()
    {
        Settings.isTutorial = true;
        SceneManager.LoadScene("Level0Scene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        OptionsWindow.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsWindow.SetActive(false);
    }
}
