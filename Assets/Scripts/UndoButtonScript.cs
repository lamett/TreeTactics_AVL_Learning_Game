using UnityEngine;

public class UndoButtonScript : MonoBehaviour
{
    public GameController gameController;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameController.undo();
        }
    }
}
