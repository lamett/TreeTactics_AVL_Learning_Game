using UnityEngine;

public class EndeButtonScript : MonoBehaviour
{
    public GameManager gameManager;
    AudioManager audioManager;
    void Awake()
    {
        GetComponent<Outline>().enabled = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameManager.gameState == GameState.AddPhase || Settings.isTutorial || Settings.isSandbox)
            {
                audioManager.PlaySFX(audioManager.PlatineButton);
                gameManager.HandleAddPhaseEnd();
            }
        }
    }
}
