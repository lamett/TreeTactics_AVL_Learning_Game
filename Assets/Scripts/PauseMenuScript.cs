using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject gameController;
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
    }

    void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Settings.GameIsPaused = true;
    }

    public void loadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void loadOptions()
    {
        optionsMenuUI.SetActive(true);
        optionsMenuUI.transform.parent.GetComponent<OptionsMenuScript>().updateUI();
    }
}
