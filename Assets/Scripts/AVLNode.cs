using TMPro;
using UnityEngine;

public class AVLNode: MonoBehaviour
{
    public int ID;
    public AVLNode left = null;
    public AVLNode right = null;
    int balanceFactor = 0;
    public int depth = 0;
    //height gibt an, wie weit es unter diesem Knoten bis zum weitersten Blatt geht
    public int height = 1;
    public bool isDeleted = false;
    public Vector3 position { get; private set;}

    public GameObject edge;
    Edge leftEdge = null;
    Edge rightEdge = null;
    TextMeshPro balanceFactorObject;

    private void Start()
    {
        position = new Vector3(0f, 0f, 0f);
        balanceFactorObject = transform.GetChild(1).GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if(leftEdge != null && left != null)
        {
            leftEdge.headPos = transform.position;
            leftEdge.tailPos = left.transform.position;
        }
        if (rightEdge != null && right != null)
        {
            rightEdge.headPos = transform.position;
            rightEdge.tailPos = right.transform.position;
        }
    }

    //Soll eine Animation triggern, um von der aktuellen Position zur neuen Position zu bewegen
    public void updatePosition(Vector3 newPosition)
    {
        position = newPosition;
        transform.position = position;
        if (leftEdge == null && left != null)
        {
            var leftEdgeObject = Instantiate(edge);
            leftEdge = leftEdgeObject.GetComponent<Edge>();
        }
        if (rightEdge == null && right != null)
        {
            var rightEdgeObject = Instantiate(edge);
            rightEdge = rightEdgeObject.GetComponent<Edge>();
        }
    }

    public void setBalanceFactor(int balanceFactor)
    {
        this.balanceFactor = balanceFactor;
        balanceFactorObject.text = balanceFactor.ToString();
    }

    //Soll Edge Prefeab von this.positon zu left/right.position ziehen
    public void updateEdge(){
        //TODO
    }
}