using UnityEngine;

public class AVLOperations : MonoBehaviour
{
    GameController gameController;
    AVLNode avlNode;

    private bool isOperationsEnabled;
    private bool isAddable;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        avlNode = GetComponent<AVLNode>();
        isOperationsEnabled = false;
        isAddable = false;
    }

    public void setIsAddable(bool isAddable)
    {
        this.isAddable = isAddable;
    }

    void OnMouseOver()
    {
        if (isOperationsEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameController.leftRotation(avlNode.ID);
            }
            if (Input.GetMouseButtonDown(1))
            {
                gameController.rightRotation(avlNode.ID);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                gameController.markDeletion(avlNode.ID);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                gameController.chooseDeletion(avlNode.ID);
            }
        }

        if(isAddable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gameController.addFromBowl(gameObject))
                {
                    switchFromAddToOp();
                }
                
            }
        }
    }

    public void switchFromAddToOp()
    {
        isAddable = false;
        isOperationsEnabled = true;
        GetComponent<AVLNode>().showID();
        GetComponent<AVLNode>().showBF();
    }
}
