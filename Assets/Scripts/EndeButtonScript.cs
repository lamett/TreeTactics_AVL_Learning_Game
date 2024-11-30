using UnityEngine;

public class EndeButtonScript : MonoBehaviour
{
    public GameManager gameManager;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameManager.HandleAddPhaseEnd();
        }
    }
}
