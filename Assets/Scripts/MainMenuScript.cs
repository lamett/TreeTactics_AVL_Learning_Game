using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public GameObject OptionsWindow;
    public GameObject GameModesWindow;

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

    public void OpenGameModes()
    {
        GameModesWindow.SetActive(true);
    }
    public void CloseGameModes()
    {
        GameModesWindow.SetActive(false);
    }

    public void StartGame()
    {
        Settings.HardModeActivated = false;
        Settings.isTutorial = false;
        Settings.isSandbox = false;
        SceneManager.LoadScene("Cutscene_Start");
    }

    public void StartHardMode()
    {
        Settings.HardModeActivated = true;
        Settings.isTutorial = false;
        Settings.isSandbox = false;
        Settings.ShowArrowHint = false;
        Settings.ShowBalanceFactor = false;
        SceneManager.LoadScene("Level0Scene");
    }

    public void StartTutorial()
    {
        Settings.HardModeActivated = false;
        Settings.isTutorial = true;
        Settings.isSandbox = false;
        SceneManager.LoadScene("Level0Scene");
    }

    public void StartSandbox()
    {
        Settings.HardModeActivated = false;
        Settings.isTutorial = false;
        Settings.isSandbox = true;
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
