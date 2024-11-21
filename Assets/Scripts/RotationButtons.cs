using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationButtons : MonoBehaviour
{
    private AVLOperations currentNode;

    
    public GameObject panel; 
    public Canvas canvas; 
    public Vector3 panelOffset;

   
    public void SetNode(AVLOperations newNode)
    {
        currentNode = newNode;
    }
    
    public void Left()
    {
        currentNode.RotateLeft();
        panel.SetActive(false); 
    }
   
    public void Right()
    {
        currentNode.RotateRight();
        panel.SetActive(false); 
    }
    private void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {            
            if (EventSystem.current.IsPointerOverGameObject())
            {
              
                return;
            }
           
            Ray ballRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ballRay, out RaycastHit ballHit, 100))
            {               
                if (ballHit.collider.gameObject.TryGetComponent<AVLNode>(out AVLNode ball))
                {                                        
                    PositionPanel(ballHit.collider.gameObject);
                    
                    SetNode(ball.GetComponent<AVLOperations>());
                    
                    panel.SetActive(true);
                }
                else
                {
                    Debug.Log($"Kein AVLNode: {ballHit.collider.gameObject.name}");
                    panel.SetActive(false); 
                }
            }
            else
            {                
                panel.SetActive(false); 
            }
        }
    }

    private void PositionPanel(GameObject ball)
    {      
        Vector3 worldPosition = ball.transform.position;
        
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);        
        
        Vector3 newPosition = screenPosition + panelOffset;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            newPosition,
            canvas.worldCamera,
            out Vector2 canvasPosition
        );
        panel.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
    }

    public void leftHint(){
        currentNode.GetComponent<AVLNode>().showHint(true);
    }
    
    public void rightHint(){
        currentNode.GetComponent<AVLNode>().showHint(false);
    }

    public void hideHint(){
        currentNode.GetComponent<AVLNode>().hideHint();
    }
}

