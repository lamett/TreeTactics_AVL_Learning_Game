using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject creditsPanel;

    public void whenButtonClicked()
    {
        if (creditsPanel.activeInHierarchy == true)
            creditsPanel.SetActive(false);
        else
            creditsPanel.SetActive(true);
    }

    public void closeUI()
    {
        creditsPanel.SetActive(false);
    }


}
