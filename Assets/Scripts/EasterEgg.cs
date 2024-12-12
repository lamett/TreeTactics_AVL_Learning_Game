using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    public GameController gameController;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameController.addFromButton();
        }
    }
}