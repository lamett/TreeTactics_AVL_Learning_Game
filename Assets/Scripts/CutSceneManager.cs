using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Level0Scene");
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
