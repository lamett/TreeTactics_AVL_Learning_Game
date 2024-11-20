using UnityEngine;

public class AVLOperations : MonoBehaviour
{
    GameController gameController;
    AVLNode avlNode;

    public bool isOperationsEnabled;
    public bool isAddable;
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

    public void setIsOperatable(bool isOperatable)
    {
        this.isOperationsEnabled = isOperatable;
    }

    void OnMouseOver()
    {
        if (isOperationsEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameController.OpenRotationPanel(this);
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
                    gameObject.transform.rotation = Quaternion.identity;
                    gameController.enableBallsCLick();
                    GetComponent<AVLNode>().showID();
                    GetComponent<AVLNode>().showBF();
                }
                
            }
        }
    }

    public void RotateLeft()
    {
        gameController.leftRotation(avlNode.ID);
    }
    public void RotateRight()
    {
        gameController.rightRotation(avlNode.ID);
    }
}
