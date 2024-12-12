using UnityEngine;

public class EndeButtonScript : MonoBehaviour
{
    public GameManager gameManager;
    void Awake()
    {
        GetComponent<Outline>().enabled = false;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameManager.HandleAddPhaseEnd();
        }
    }
}
