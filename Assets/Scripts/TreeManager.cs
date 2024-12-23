
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class TreeManager
{
    public UnityEvent<int> updateTreeBalance;
    GameObject nodePrefab;
    List<int> possibleNumbers;

    AVLNode newestNode = null;
    AVLTree baum;
    int[] oldBaum;

    public enum Commands
    {
        RotateRight,
        RotateLeft,
        Insert,
        Delete,
    }

    public enum NodeMaterial
    {
        Green,
        Gray,
        Pink
    }

    public TreeManager(GameObject nodePrefab, UnityEvent<int> updateTreeBalance, Stack<Tuple<Commands, int>> commandHistory)
    {
        this.nodePrefab = nodePrefab;
        this.updateTreeBalance = updateTreeBalance;
        baum = new AVLTree(commandHistory);
        possibleNumbers = Enumerable.Range(0, 50).ToList();
        shuffle();
    }

    //Randomiziert die Liste von möglichen Nodes
    void shuffle()
    {
        var rnd = new System.Random();
        possibleNumbers = possibleNumbers.OrderBy(item => rnd.Next()).ToList();
    }
    public GameObject[] getTreeAsGOArray()
    {
        AVLNode[] nodes = baum.traverse();
        GameObject[] balls = new GameObject[nodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            balls[i] = nodes[i].gameObject;
        }
        return balls;
    }

    public void backUpTree()
    {
        oldBaum = baum.saveTree();
    }

    public void destroyTree()
    {
        var nodes = baum.saveTree().Reverse();
        foreach (int ID in nodes)
        {
            baum.markDeletion(ID);
        }
    }
    public void rebuildTree()
    {
        rebuildTree(oldBaum);
    }

    public void rebuildTree(int[] nodes)
    {
        foreach (int ID in nodes)
        {
            addObject(instantiateBallForBowl(), ID);
        }
        showAllBF();
    }

    public GameObject instantiateBallForBowl()
    {
        var prefab = UnityEngine.Object.Instantiate(nodePrefab);
        prefab.transform.position = new Vector3(-17.5f, 20, 3.23f);
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
            colorTree();

            return true;
        }
        return false;
    }

    public int calculateID()
    {
        int ID = calculateIDHard();
        int i = 0;
        while (ID == -1 && i < 5)
        {
            ID = calculateIDHard();
            i++;
        }

        return ID == -1 ? calculateIDRandom() : ID;
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

    public void leftRotation(int ID)
    {
        baum.leftRot(ID);
        baum.calculatePosition();
        colorTree();
        baum.root.hideHint();
    }

    public void rightRotation(int ID)
    {
        baum.rightRot(ID);
        baum.calculatePosition();
        colorTree();
        baum.root.hideHint();
    }

    public bool isBalanced()
    {
        return baum.treeBalance(baum.root) <= 1;
    }

    private void colorTree()
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

    public bool chooseDeletion(int ID)
    {
        if (baum.markedDeletion != null)
        {
            int markedDeletion = baum.markedDeletion.ID;
            var correct = baum.chooseDeletion(ID);
            if (correct)
            {
                possibleNumbers.Add(markedDeletion);
                shuffle();
                colorTree();
            }
            baum.calculatePosition();
            return correct;
        }
        return false;
    }

    public void resetGapFillers()
    {
        AVLNode[] nodes = baum.traverse();
        if (nodes != null)
        {
            foreach (AVLNode node in nodes)
            {
                if (node != null)
                {
                    node.isGapFiller = false;
                }
            }
        }
    }

    public void markGapFillers()
    {
        AVLNode[] nodes = baum.findChoiceforChooseDeletion();
        if (nodes != null)
        {
            foreach (AVLNode node in nodes)
            {
                if (node != null)
                {
                    node.isGapFiller = true;
                }
            }
        }
        else
        {
            Debug.Log("No GapFillers found");
        }
    }

    public void colorGapFillers()
    {
        AVLNode[] nodes = baum.traverse();
        if (nodes != null)
        {
            foreach (AVLNode node in nodes)
            {
                if (node.GetComponent<AVLNode>().isGapFiller)
                {
                    node.setMaterial(NodeMaterial.Green);
                }
                else
                {
                    node.setMaterial(NodeMaterial.Pink);
                }

            }
        }
    }

    public int findNodeToDelete()
    {
        var rnd = new System.Random();
        var nodes = baum.traverse().ToList().OrderBy(item => rnd.Next()).ToList();
        var result = baum.root;
        foreach (var node in nodes)
        {
            if (node.left != null && node.right != null)
            {
                result = node;
                break;
            }
        }
        return result.ID;
    }

    public async void rotateRandom(int countRotation)
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
            await Task.Delay(10);
        }
        baum.calculatePosition();
    }

    public async void balanceTreeCompletly()
    {

        if (baum.treeBalance(baum.root) < 2)
        {
            return;
        }
        var index = 2;
        while (index < baum.size)
        {
            var notDone = await balanceTree(baum.root, index);
            while (notDone)
            {
                index = 2;
                notDone = await balanceTree(baum.root, index);
            }
            if (baum.treeBalance(baum.root) < 2)
            {
                break;
            }
            index++;
        }
    }

    async Task<bool> balanceTree(AVLNode node, int balanceFactor)
    {
        bool result = false;
        if (node != null)
        {
            if (Mathf.Abs(node.getBalanceFactor()) == balanceFactor)
            {
                if (node.getBalanceFactor() < 0)
                {
                    if (node.right.getBalanceFactor() >= 0)
                    {
                        rightRotation(node.right.ID);
                        await Task.Delay(300);
                        leftRotation(node.ID);
                    }
                    else
                    {
                        leftRotation(node.ID);
                    }
                }
                else
                {
                    if (node.left.getBalanceFactor() >= 0)
                    {
                        leftRotation(node.left.ID);
                        await Task.Delay(300);
                        rightRotation(node.ID);
                    }
                    else
                    {
                        rightRotation(node.ID);
                    }
                }
                await Task.Delay(300);
                return true;
            }
            result |= await balanceTree(node.left, balanceFactor);
            result |= await balanceTree(node.right, balanceFactor);
        }
        return result;
    }

    public void showAllBF()
    {
        var nodes = baum.traverse();

        if (Settings.ShowBalanceFactor)
        {
            foreach (var node in nodes)
            {
                node.showBF();
            }
        }
        else
        {
            foreach (var node in nodes)
            {
                node.hideBF();
            }
        }
    }

    public AVLNode findNode(int ID)
    {
        return baum.find(ID);
    }

    public int Count()
    {
        return baum.size;
    }

    public AVLNode getRoot()
    {
        return baum.root;
    }
}
