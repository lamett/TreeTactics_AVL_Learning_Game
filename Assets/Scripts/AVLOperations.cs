using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AVLOperations : MonoBehaviour
{
    GameController gameController;
    AVLNode avlNode;

    public bool isOperationsEnabled;
    public bool isAddable;
    public bool isChoosableForDel;
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

    public void setIsChoosableForDel(bool isOperatable)
    {
        this.isChoosableForDel= isOperatable;
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
            if (Input.GetKeyDown(KeyCode.B))
            {
                gameController.balance();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameController.randomRot();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                gameController.killTree();
            }
        }

        if (isChoosableForDel)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                gameController.markDeletion(avlNode.ID);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (avlNode.isGapFiller)
                {
                    gameController.chooseDeletion(avlNode.ID); // deletes node, fill gap, calculate new positions, color
                    gameController.EndSpezialAttakDel(); //stops timer and go to balance phase
                }
                else
                {
                    gameController.RerunSpezialAttakDel();
                }
            }
        }

        if (isAddable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gameController.addFromBowl(gameObject))
                {
                    gameObject.transform.rotation = Quaternion.identity;
                    gameController.enableBallsClickOperation(true);
                    avlNode.showID();
                    avlNode.showBF();
                }

            }
        }
    }
}
