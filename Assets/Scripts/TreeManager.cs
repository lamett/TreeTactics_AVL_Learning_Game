using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

class TreeManager
{
    GameObject nodePrefab;
    List<int> possibleNumbers;

    AVLNode newestNode = null;

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

    public GameObject generate(int ID)
    {   
        var prefab = Object.Instantiate(nodePrefab);
        prefab.transform.position = new Vector3(-11, 9, 4);
        var node = prefab.GetComponent<AVLNode>();
        node.ID = ID;
        prefab.transform.GetChild(0).GetComponent<TextMeshPro>().text = node.ID.ToString();
        setVisibilityID(prefab, false);
        setVisibilityBF(prefab,false);
        return prefab;
    }

    private void setVisibilityID(GameObject prefab, bool boolean){
        if (prefab.transform.childCount > 0)
        {
            prefab.transform.GetChild(0).gameObject.SetActive(boolean);
        }
        else {Debug.Log("No Child found.");}
    }

    private void setVisibilityBF(GameObject prefab, bool boolean){
        if (prefab.transform.childCount > 1)
        {
            prefab.transform.GetChild(1).gameObject.SetActive(boolean);
        }
        else {Debug.Log("No Child found.");}
    }

    public bool addObject(GameObject prefab){
        if (baum.treeBalance(baum.root) <= 1)
        {
            prefab.GetComponent<Rigidbody>().isKinematic = true;
            setVisibilityID(prefab,true);
            setVisibilityBF(prefab,true);
            var node = prefab.GetComponent<AVLNode>();
            baum.insert(node);
            baum.calculatePosition();
            newestNode = node;
            testTreeBalance();
            return true;
        }
        return false;
    }

    private void add(int ID)
    {
        if (baum.treeBalance(baum.root) <= 1)
        {
            var prefab = Object.Instantiate(nodePrefab);
            prefab.transform.position = new Vector3(-10, 0, 0);
            var node = prefab.GetComponent<AVLNode>();
            node.ID = ID;
            prefab.transform.GetChild(0).GetComponent<TextMeshPro>().text = node.ID.ToString();
            baum.insert(node);
            baum.calculatePosition();
            newestNode = node;
            testTreeBalance();
        }
    }

    public void leftRotation(int ID)
    {
        baum.leftRot(ID);
        baum.calculatePosition();
        testTreeBalance();
    }

    public void rightRotation(int ID)
    {
        baum.rightRot(ID);
        baum.calculatePosition();
        testTreeBalance();
    }

    private void testTreeBalance()
    {
        if (newestNode != null && baum.treeBalance(baum.root) <= 1)
        {
            newestNode.setGreenMaterial();
            newestNode = null;
        }
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

    public void rotateRandom(int countRotation){
        var allNodes = baum.traverse();
        var rnd = new System.Random();
        for(int i = 0; i < countRotation; i++){
            var ID = allNodes[rnd.Next(0, allNodes.Length -1)].ID;
            if(rnd.Next() % 2 ==0){
                leftRotation(ID);
            }else
            {
                rightRotation(ID);
            }
        }
        baum.calculatePosition();
    }
}
