
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

class TreeManager
{
    GameObject nodePrefab;
    List<int> possibleNumbers;

    AVLTree baum;
    // Start is called before the first frame update
    public TreeManager(GameObject nodePrefab)
    {
        this.nodePrefab = nodePrefab;
        baum = new AVLTree();
        possibleNumbers = Enumerable.Range(0, 50).ToList();
        shuffle();
    }

    //Randomiziert die Liste von möglichen Nodes
    void shuffle()
    {
        var rnd = new System.Random();
        possibleNumbers = possibleNumbers.OrderBy(item => rnd.Next()).ToList();
    }

    public void addRandom()
    {
        if (possibleNumbers.Count > 0)
        {
            var ID = possibleNumbers.First();
            possibleNumbers.RemoveAt(0);
            add(ID);
        }
    }

    public void addMultiple(int[] IDs)
    {
        foreach (int ID in IDs)
        {
            add(ID);
        }
    }

    private void add(int ID)
    {
        var prefab = Object.Instantiate(nodePrefab);
        prefab.transform.position = new Vector3(-10, 0, 0);
        var node = prefab.GetComponent<AVLNode>();
        node.ID = ID;
        prefab.transform.GetChild(0).GetComponent<TextMeshPro>().text = node.ID.ToString();
        baum.insert(node);
        baum.calculatePosition();
    }

    public void leftRotation(int ID)
    {
        baum.leftRot(ID);
        baum.calculatePosition();
    }

    public void rightRotation(int ID)
    {
        baum.rightRot(ID);
        baum.calculatePosition();
    }

    public void markDeletion(int ID)
    {
        baum.markDeletion(ID);
        baum.calculatePosition();
    }

    public void chooseDeletion(int ID)
    {
        baum.chooseDeletion(ID);
        baum.calculatePosition();
    }
}
