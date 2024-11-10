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
        this.isChoosableForDel = isOperatable;
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
        }

        if (isChoosableForDel)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                gameController.markDeletion(avlNode.ID);
            }
            if (Input.GetMouseButtonDown(0))
            {
                gameController.chooseDeletion(avlNode.ID);
                //gameController.endSpecialAttackDeletion(); hier muss methode so geschrieben werden dass gotDeltionRight und isBalanced mit übergeben werden können
            }
        }

        if(isAddable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gameController.addFromBowl(gameObject))
                {
                    gameObject.transform.rotation = Quaternion.identity;
                    gameController.enableBallsClickAddPhase();
                    GetComponent<AVLNode>().showID();
                    GetComponent<AVLNode>().showBF();
                }
                
            }
        }
    }
}
