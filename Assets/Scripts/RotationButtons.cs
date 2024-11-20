using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationButtons : MonoBehaviour
{

    private AVLOperations currentNode;

    public void SetNode(AVLOperations newNode)
    {
        currentNode = newNode;
    }
            
    public void Left()
    {
        currentNode.RotateLeft();
        gameObject.SetActive(false);
    }


    public void Right()
    {
        currentNode.RotateRight();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("");
                return;
            }
            Ray ballRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ballRay, out RaycastHit ballHit, 100))
            {
                

                if (!ballHit.collider.gameObject.TryGetComponent<AVLNode>(out AVLNode ball))
                {
                    Debug.Log(ballHit.collider.gameObject.name);
                    gameObject.SetActive(false);
                }
            }else
            {
                Debug.Log("");
                gameObject.SetActive(false);
                return;
            }
                
                
            }
    }

}
