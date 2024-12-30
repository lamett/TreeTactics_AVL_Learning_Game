using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Cutscene_Start" && Input.anyKeyDown)
        {
            LoadLevel0();
        }
        if (SceneManager.GetActiveScene().name == "TitelScene" && Input.anyKeyDown)
        {
            LoadMenu();
        }

    }
    public void LoadLevel0()
    {
        Debug.Log("Szenenwechesel");
        SceneManager.LoadScene("Level0Scene");
    }

    public void LoadMenu()
    {
        Debug.Log("Szenenwechesel");
        SceneManager.LoadScene("StartMenu");
    }

}
