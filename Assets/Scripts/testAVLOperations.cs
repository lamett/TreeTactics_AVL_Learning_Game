using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAVLOperations : MonoBehaviour
{
    GameController gameController;
    AVLNode avlNode;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        avlNode = GetComponent<AVLNode>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
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
}
