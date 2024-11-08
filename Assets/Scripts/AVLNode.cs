using System.Collections;
using TMPro;
using UnityEngine;

public class AVLNode : MonoBehaviour
{
    public int ID;
    public AVLNode left = null;
    public AVLNode right = null;
    int balanceFactor = 0;
    public int depth = 0;
    //height gibt an, wie weit es unter diesem Knoten bis zum weitersten Blatt geht
    public int height = 1;
    bool isDeleted = false;

    public Material green;
    public Material gray;
    public GameObject edge;
    Edge leftEdge = null;
    Edge rightEdge = null;
    TextMeshPro balanceFactorObject;
    new MeshRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.material = gray;
        balanceFactorObject = transform.GetChild(1).GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        updateEdge();
    }

    //Soll eine Animation triggern, um von der aktuellen Position zur neuen Position zu bewegen
    public void updatePosition(Vector3 newPosition)
    {
        if (Vector3.Distance(transform.position, newPosition) > 0.1)
        {
            StartCoroutine(LerpPostion(newPosition, 0.25f));
        }

        setEdges();
    }

    IEnumerator LerpPostion(Vector3 newPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = newPosition;
    }

    public void setBalanceFactor(int balanceFactor)
    {
        this.balanceFactor = balanceFactor;
        balanceFactorObject.text = balanceFactor.ToString();
    }

    public int getBalanceFactor()
    {
        return balanceFactor;
    }

    //Setzt die Edges für die Kindern
    void setEdges()
    {
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

    //Soll Edge Prefeab von this.positon zu left/right.position ziehen
    void updateEdge()
    {
        //gibt die aktuellen Positionen an die Edges weiter
        if (leftEdge != null && left != null)
        {
            leftEdge.headPos = transform.position;
            leftEdge.tailPos = left.transform.position;
        }
        if (rightEdge != null && right != null)
        {
            rightEdge.headPos = transform.position;
            rightEdge.tailPos = right.transform.position;
        }

        //remove wenn Node kein Kind mehr hat
        if (left == null && leftEdge != null)
        {
            Destroy(leftEdge.gameObject);
            leftEdge = null;
        }
        if (right == null && rightEdge != null)
        {
            Destroy(rightEdge.gameObject);
            rightEdge = null;
        }
    }

    public void setDeletion()
    {
        isDeleted = true;
        Debug.Log(ID.ToString() + " als gelöscht markiert: " + isDeleted.ToString());
        //TODO hier muss die bessere veränderung für eine Gelöschte node rein
        transform.localScale *= 0.5f;
        renderer.enabled = false;
    }

    public void delete()
    {
        Destroy(leftEdge?.gameObject);
        Destroy(rightEdge?.gameObject);
        Destroy(gameObject);
    }

    public void setGreenMaterial()
    {
        renderer.material = green;
    }

    //Visability of ID and Balancefaktor
    public void showID()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void hideID()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void showBF()
    {
        this.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void hideBF()
    {
        this.transform.GetChild(1).gameObject.SetActive(false);
    }

}