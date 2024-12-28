using UnityEngine;

public class RoboShuffle : MonoBehaviour
{
    public GameController gameController;
    AudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Settings.isSandbox)
            {
                audioManager.PlaySFX(audioManager.EnemyTakesDamage);
                gameController.randomRot();
            }
        }
    }
}
