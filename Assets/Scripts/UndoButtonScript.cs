using UnityEngine;

public class UndoButtonScript : MonoBehaviour
{
    public GameController gameController;
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
            audioManager.PlaySFX(audioManager.PlatineButton);
            gameController.undo();
        }
    }
}
