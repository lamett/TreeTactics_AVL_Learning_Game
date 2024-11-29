using UnityEditor.Experimental.GraphView;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class AVLOperations : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameController gameController;
    AVLNode avlNode;

    public bool isOperationsEnabled;
    public bool isAddable;
    public bool isChoosableForDel;

    private Vector3 mouseStartPosition;
    private Vector3 mouseEndPosition;
    private bool isDragging = false;

    private Rotation prevRotation = Rotation.None;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        avlNode = GetComponent<AVLNode>();
        isOperationsEnabled = false;
        isAddable = false;
        gameObject.GetComponent<Outline>().enabled = false;
    }


    void Update()
    {
        if (isDragging)
        {
            Rotation rotation = DetectDragDirection();

            if (rotation != prevRotation)
            {
                Debug.Log(rotation);
                switch (rotation)
                {
                    case Rotation.Right:
                        avlNode.showHint(false); //show rightrotation hint
                        break;
                    case Rotation.Left:
                        avlNode.showHint(true); //show leftrotation hint
                        break;
                    case Rotation.None:
                        avlNode.hideHint();
                        break;
                }
                prevRotation = rotation;
            }
        }
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

    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.GetComponent<Outline>().enabled = true;
        if (isOperationsEnabled)
        {
            mouseStartPosition = Input.mousePosition;
            isDragging = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gameObject.GetComponent<Outline>().enabled = false;
        if (isOperationsEnabled)
        {
            if (isDragging)
            {
                Rotation rotation = DetectDragDirection();
                switch (rotation)
                {
                    case Rotation.Right:
                        gameController.rightRotation(avlNode.ID);
                        break;
                    case Rotation.Left:
                        gameController.leftRotation(avlNode.ID);
                        break;
                    case Rotation.None:
                        Debug.Log("No Rotation detected");
                        break;
                }
                isDragging = false;
            }
        }
    }

    private Rotation DetectDragDirection()
    {
        float dragDistanceX = Input.mousePosition.x - mouseStartPosition.x;

        float dragThreshold = 10f;
        if (Mathf.Abs(dragDistanceX) > dragThreshold)
        {
            if (dragDistanceX > 0)
            {
                return Rotation.Right;
                //gameController.rightRotation(avlNode.ID);
            }
            else
            {
                return Rotation.Left;
                //gameController.leftRotation(avlNode.ID);
            }
        }
        else
        {
            return Rotation.None;
        }
    }

    void OnMouseOver()
    {
        //if (isOperationsEnabled)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
                //gameController.OpenRotationPanel(this);

        //    }
        //    if (Input.GetKeyDown(KeyCode.B))
        //    {
        //        gameController.balance();
        //    }
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        gameController.randomRot();
        //    }
        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        gameController.killTree();
        //    }
        //}

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

    public void RotateLeft()
    {
        gameController.leftRotation(avlNode.ID);
    }
    
    public void RotateRight()
    {
        gameController.rightRotation(avlNode.ID);
    }
}
public enum Rotation
{
    None,
    Right,
    Left
}
