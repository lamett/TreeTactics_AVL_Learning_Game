
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TreeManager
{
    public UnityEvent<int> updateTreeBalance;
    GameObject nodePrefab;
    List<int> possibleNumbers;

    AVLNode newestNode = null;
    AVLTree baum;

    public enum NodeMaterial
    {
        Green,
        Gray,
    }

    public GameObject[] getTreeAsGOArray()
    {
        AVLNode[] nodes = baum.traverse();
        GameObject[] balls = new GameObject[nodes.Length];
        for(int i = 0; i < nodes.Length; i++)
        {
            balls[i] = nodes[i].gameObject;
        }
        return balls;
    }

    // Start is called before the first frame update
    public TreeManager(GameObject nodePrefab, UnityEvent<int> updateTreeBalance)
    {
        this.nodePrefab = nodePrefab;
        this.updateTreeBalance = updateTreeBalance;
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
    
    public GameObject instantiateBallForBowl()
    {   
        var prefab = UnityEngine.Object.Instantiate(nodePrefab);
        prefab.transform.position = new Vector3(-11, 9, 4);
        return prefab;
    }

    public bool addObject(GameObject prefab, int ID)
    {
        if (baum.treeBalance(baum.root) <= 1)
        {
            var node = prefab.GetComponent<AVLNode>();
            node.ID = ID;
            prefab.transform.GetChild(0).GetComponent<TextMeshPro>().text = node.ID.ToString();

            prefab.GetComponent<Rigidbody>().isKinematic = true;

            baum.insert(node);
            baum.calculatePosition();
            newestNode = node;
            testTreeBalance();

            return true;
        }
        return false;
    }

    public int calculateID()
    {
        int ID = calculateIDHard();
        int i = 0;
        while(ID == -1 && i<5)
        {
            ID = calculateIDHard();
            i++;
        }
        
        return ID == -1?calculateIDRandom():ID;
    }

    public int calculateIDRandom()
    {
        if (possibleNumbers.Count > 0)
        {
            var ID = possibleNumbers.First();
            possibleNumbers.RemoveAt(0);
            return ID;
        }

        return -1;
    }

    public int calculateIDHard()
    {
        if (possibleNumbers.Count > 0)
        {
            if (baum.isTreeBalanceZero(baum.root))
            {
                return calculateIDRandom();
            }

            var rangeWithExcept = findHardNode(baum.root, new Tuple<int, int, int>(int.MinValue, int.MaxValue, -1));

            //Debug.Log($"low: {rangeWithExcept.Item1}, high: {rangeWithExcept.Item2}, except: {rangeWithExcept.Item3}");

            int lowEnd = rangeWithExcept.Item1;
            int heighEnd = rangeWithExcept.Item2;
            int except = rangeWithExcept.Item3;

            for (int i = 0; i < possibleNumbers.Count; i++)
            {
                var possibleID = possibleNumbers[i];
                if (possibleID < heighEnd && possibleID > lowEnd && possibleID != except)
                {
                    possibleNumbers.RemoveAt(i);
                    return possibleID;
                }
            }
        }
        return -1;
    }

    Tuple<int, int, int> findHardNode(AVLNode node, Tuple<int, int, int> rangeWithExcept)
    {
        if (node != null)
        {
            Tuple<int, int, int> result;
            if (node.getBalanceFactor() < 0)
            {
                result = findHardNode(node.right, new Tuple<int, int, int>(node.ID, rangeWithExcept.Item2, -1));
            }
            else if (node.getBalanceFactor() > 0)
            {
                result = findHardNode(node.left, new Tuple<int, int, int>(rangeWithExcept.Item1, node.ID, -1));
            }
            else
            {
                if (node.left != null && node.right != null)
                {
                    if (new System.Random().Next() % 2 == 0)
                    {
                        result = findHardNode(node.left, new Tuple<int, int, int>(rangeWithExcept.Item1, node.ID, -1));
                    }
                    else
                    {
                        result = findHardNode(node.right, new Tuple<int, int, int>(node.ID, rangeWithExcept.Item2, -1));
                    }
                }
                else if (node.left != null)
                {
                    result = findHardNode(node.left, new Tuple<int, int, int>(rangeWithExcept.Item1, node.ID, -1));
                }
                else if (node.right != null)
                {
                    result = findHardNode(node.right, new Tuple<int, int, int>(node.ID, rangeWithExcept.Item2, -1));
                }
                else
                {
                    return new Tuple<int, int, int>(rangeWithExcept.Item1, rangeWithExcept.Item2, node.ID);
                }
            }
            return result;
        }
        Debug.Log("Das sollte nicht passieren");
        return null;
    }

    //public GameObject add(int ID)
    //{
    //    if (baum.treeBalance(baum.root) <= 1)
    //    {
    //        var prefab = UnityEngine.Object.Instantiate(nodePrefab);
    //        prefab.transform.position = new Vector3(-10, 0, 0);
    //        var node = prefab.GetComponent<AVLNode>();
    //        node.ID = ID;
    //        prefab.transform.GetChild(0).GetComponent<TextMeshPro>().text = node.ID.ToString();
    //        baum.insert(node);
    //        baum.calculatePosition();
    //        testTreeBalance();
    //        return prefab;
    //    }
    //    return null;
    //}

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
        var balance = baum.treeBalance(baum.root);

        NodeMaterial material = NodeMaterial.Gray;
        if (balance <= 1)
        {
            material = NodeMaterial.Green;
        }
        baum.setMaterial(baum.root, material);

        updateTreeBalance.Invoke(balance);
    }

    public void markDeletion(int ID)
    {
        var correct = baum.markDeletion(ID);
        if (baum.markedDeletion != null && correct)
        {
            possibleNumbers.Add(ID);
            shuffle();
        }
        baum.calculatePosition();
    }

    public void chooseDeletion(int ID)
    {
        int markedDeletion = baum.markedDeletion.ID;
        var correct = baum.chooseDeletion(ID);
        if (correct)
        {
            possibleNumbers.Add(markedDeletion);
            shuffle();
            testTreeBalance();
        }
        baum.calculatePosition();
    }

    public void rotateRandom(int countRotation)
    {
        var allNodes = baum.traverse();
        var rnd = new System.Random();
        for (int i = 0; i < countRotation; i++)
        {
            var ID = allNodes[rnd.Next(0, allNodes.Length - 1)].ID;
            if (rnd.Next() % 2 == 0)
            {
                leftRotation(ID);
            }
            else
            {
                rightRotation(ID);
            }
        }
        baum.calculatePosition();
    }
}
