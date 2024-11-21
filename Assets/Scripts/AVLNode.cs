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
    public GameObject arrow;
    GameObject hintArrow;
    Edge leftEdge = null;
    Edge rightEdge = null;
    TextMeshPro balanceFactorObject;
    new MeshRenderer renderer;

    bool markLeftEdge = false;
    bool markRightEdge = false;
    public float heightFactor;

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
            if (markLeftEdge)
            {
                markInsert(true);
            }
        }
        if (rightEdge == null && right != null)
        {
            var rightEdgeObject = Instantiate(edge);
            rightEdge = rightEdgeObject.GetComponent<Edge>();
            if (markRightEdge)
            {
                markInsert(false);
            }
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

    public void setMaterial(TreeManager.NodeMaterial material)
    {
        Material newMaterial = gray;
        switch (material)
        {
            case TreeManager.NodeMaterial.Green:
                newMaterial = green;
                break;
            case TreeManager.NodeMaterial.Gray:
                newMaterial = gray;
                break;
        }
        renderer.material = newMaterial;
    }

    //Visability of ID and Balancefaktor
    public void showID()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void hideID()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void showBF()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void hideBF()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }


    public void markInsert(bool isLeft)
    {
        var timeForEdgeMarking = -transform.position.z / heightFactor / 5;
        if (isLeft)
        {
            if (leftEdge == null)
            {
                markLeftEdge = true;
                return;
            }
            markLeftEdge = false;
            leftEdge.markInsert(timeForEdgeMarking);
        }
        else
        {
            if (rightEdge == null)
            {
                markRightEdge = true;
                return;
            }
            markRightEdge = false;
            rightEdge.markInsert(timeForEdgeMarking);
        }
    }

    public void showHint(bool isLeft, Vector3? reference)
    {
        //if (reference == null && left == null && right == null) return;
        if ((reference == null && isLeft && right == null) || (reference == null && !isLeft && left == null)) return;
        hintArrow = Instantiate(arrow);
        hintArrow.transform.SetParent(gameObject.transform);
        hintArrow.transform.position = transform.position + new Vector3(0, -0.4f, 0);
        Vector3 rot;
        if (reference == null)
        {
            if (isLeft)
            {
                if (left == null)
                {
                    rot = new Vector3(0, 163, 0);
                }
                else
                {
                    var dir = left.transform.position + new Vector3(0, 0, 0.6f) - transform.position;
                    float angle = Mathf.Atan2(dir.z, dir.x) * -Mathf.Rad2Deg;
                    rot = new Vector3(0, angle, 0);
                }
                hintArrow.transform.eulerAngles = rot;
                right?.showHint(true, transform.position);
            }
            else
            {
                if (right == null)
                {
                    rot = new Vector3(180, 17, 0);
                }
                else
                {
                    var dir = right.transform.position + new Vector3(0, 0, 0.6f) - transform.position;
                    float angle = Mathf.Atan2(dir.z, dir.x) * -Mathf.Rad2Deg;
                    rot = new Vector3(180, angle, 0);
                }
                hintArrow.transform.eulerAngles = rot;
                left?.showHint(false, transform.position);
            }
        }
        else
        {
            if (isLeft)
            {
                var dir = (Vector3)reference + new Vector3(0.6f, 0, 0) - transform.position;
                float angle = Mathf.Atan2(dir.z, dir.x) * -Mathf.Rad2Deg;
                rot = new Vector3(0, angle, 0);
            }
            else
            {
                var dir = (Vector3)reference - new Vector3(0.6f, 0, 0) - transform.position;
                float angle = Mathf.Atan2(dir.z, dir.x) * -Mathf.Rad2Deg;
                rot = new Vector3(180, angle, 0);
            }
            hintArrow.transform.eulerAngles = rot;
        }
    }
    public void hideHint()
    {
        Destroy(hintArrow);
        left?.hideHint();
        right?.hideHint();
    }
}