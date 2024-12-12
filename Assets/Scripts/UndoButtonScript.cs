using UnityEngine;

public class UndoButtonScript : MonoBehaviour
{
    public GameController gameController;
    void Awake()
    {
        GetComponent<Outline>().enabled = false;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameController.undo();
        }
    }
}
