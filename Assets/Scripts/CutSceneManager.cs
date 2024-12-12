using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    public void LoadLevel0()
    {
        Debug.Log("Szenenwechesel");
        SceneManager.LoadScene("Level0Scene");
    }
    
}
