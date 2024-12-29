using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject gameController;
    public AudioManager audioManager;
    bool resumeTimerSound = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Settings.GameIsPaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }

    public void resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Settings.GameIsPaused = false;
        gameController.GetComponent<GameController>().showAllBF();
        if (resumeTimerSound)
        {
            audioManager.StartTimer();
            resumeTimerSound = false;
        }
    }

    void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Settings.GameIsPaused = true;
        if (audioManager.isTimerPlaying())
        {
            audioManager.StopTimer();
            resumeTimerSound = true;
        }
    }

    public void loadMenu()
    {
        Time.timeScale = 1f;
        resumeTimerSound = false;
        SceneManager.LoadScene("StartMenu");
    }

    public void loadOptions()
    {
        optionsMenuUI.SetActive(true);
        optionsMenuUI.transform.parent.GetComponent<OptionsMenuScript>().updateUI();
    }
}
