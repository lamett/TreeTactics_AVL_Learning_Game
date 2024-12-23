
//https://www.programiz.com/dsa/avl-tree
using System;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

class AVLTree
{
    public AVLNode root { get; private set; }
    public int size { get; private set; }

    public float posFactorWidth = 0.8f;
    public float posFactorHeight = 1.2f;

    float zOffset = 1f; //2.3
    float xOffset = -1.95f;
    float yOffset = -0.8f;

    public AVLNode markedDeletion { get; private set; }


    Stack<Tuple<TreeManager.Commands, int>> commandHistory;

    public AVLTree(Stack<Tuple<TreeManager.Commands, int>> commandHistory)
    {
        this.commandHistory = commandHistory;
        root = null;
        size = 0;
    }

    //gibt die Höhe vom Blatt aus an
    private int height(AVLNode node)
    {
        return node?.height ?? 0;
    }

    private int max(int a, int b)
    {
        return (a > b) ? a : b;
    }

    public void leftRot(int ID)
    {
        root = lRot(root, ID);
    }

    private AVLNode lRot(AVLNode node, int ID)
    {
        if (node != null)
        {
            AVLNode tmpNode;
            if (node.ID < ID)
            {
                tmpNode = node.right;
                node.right = lRot(node.right, ID) ?? tmpNode;

            }
            else if (node.ID > ID)
            {
                tmpNode = node.left;
                node.left = lRot(node.left, ID) ?? tmpNode;
            }
            else
            {
                if (node.right != null)
                {
                    node = leftRotation(node);
                }
            }

            node.height = max(height(node.left), height(node.right)) + 1;
            node.setBalanceFactor(height(node.left) - height(node.right));
        }

        return node;
    }

    private AVLNode leftRotation(AVLNode node)
    {
        if (node == null)
        {
            return null;
        }
        var tempNode = node.right;
        if (tempNode == null) return null;
        var T2 = tempNode.left;
        tempNode.left = node;
        node.right = T2;

        commandHistory.Push(new Tuple<TreeManager.Commands, int>(TreeManager.Commands.RotateLeft, tempNode.ID));

        node.height = max(height(node.left), height(node.right)) + 1;
        tempNode.height = max(height(tempNode.left), height(tempNode.right)) + 1;
        node.setBalanceFactor(height(node.left) - height(node.right));
        tempNode.setBalanceFactor(height(tempNode.left) - height(tempNode.right));
        return tempNode;
    }

    public void rightRot(int ID)
    {
        root = rRot(root, ID);
    }

    private AVLNode rRot(AVLNode node, int ID)
    {
        if (node != null)
        {
            AVLNode tmpNode;
            if (node.ID < ID)
            {
                tmpNode = node.right;
                node.right = rRot(node.right, ID) ?? tmpNode;

            }
            else if (node.ID > ID)
            {
                tmpNode = node.left;
                node.left = rRot(node.left, ID) ?? tmpNode;
            }
            else
            {
                if (node.left != null)
                {
                    node = rightRotation(node);
                }
            }

            node.height = max(height(node.left), height(node.right)) + 1;
            node.setBalanceFactor(height(node.left) - height(node.right));
        }

        return node;
    }

    private AVLNode rightRotation(AVLNode node)
    {
        if (node == null)
        {
            return null;
        }
        var tempNode = node.left;
        if (tempNode == null) return null;
        var T2 = tempNode.right;
        tempNode.right = node;
        node.left = T2;

        commandHistory.Push(new Tuple<TreeManager.Commands, int>(TreeManager.Commands.RotateRight, tempNode.ID));

        node.height = max(height(node.left), height(node.right)) + 1;
        tempNode.height = max(height(tempNode.left), height(tempNode.right)) + 1;
        node.setBalanceFactor(height(node.left) - height(node.right));
        tempNode.setBalanceFactor(height(tempNode.left) - height(tempNode.right));

        return tempNode;
    }


    public AVLNode find(int ID)
    {
        return find(root, ID);
    }

    private AVLNode find(AVLNode node, int ID)
    {
        if (node == null)
        {
            return null;
        }
        if (ID == node.ID)
        {
            return node;
        }
        if (ID < node.ID)
        {
            return find(node.left, ID);
        }
        return find(node.right, ID);
    }

    private AVLNode findParent(AVLNode node, int ID)
    {
        if (node == null)
        {
            return null;
        }
        if (ID == node.left?.ID || ID == node.right?.ID)
        {
            return node;
        }
        if (ID < node.ID)
        {
            return findParent(node.left, ID);
        }
        return findParent(node.right, ID);
    }


    public void insert(AVLNode newNode)
    {
        root = insert(root, newNode);
    }

    private AVLNode insert(AVLNode node, AVLNode newNode)
    {
        if (node == null)
        {
            size++;
            commandHistory.Push(new Tuple<TreeManager.Commands, int>(TreeManager.Commands.Insert, newNode.ID));
            newNode.heightFactor = posFactorHeight;
            return newNode;
        }
        if (newNode.ID < node.ID)
        {
            node.markInsert(true);
            node.left = insert(node.left, newNode);
        }
        else if (newNode.ID > node.ID)
        {
            node.markInsert(false);
            node.right = insert(node.right, newNode);
        }

        node.height = max(height(node.left), height(node.right)) + 1;
        node.setBalanceFactor(height(node.left) - height(node.right));
        return node;
    }

    //Markiert nur die Node als zu löschend. Zum wirklichen löschen chooseDeletion(nachbarID)
    public bool markDeletion(int ID)
    {
        markedDeletion = find(ID);
        if (markedDeletion == null) return false;
        //Falls es ein Blatt ist, muss man nicht mehr den Nachbarn wählen
        if (markedDeletion.left == null && markedDeletion.right == null)
        {
            root = delete(root, ID, true);
        }

        markedDeletion.setDeletion();
        return true;
    }

    //Methode um die nachrückende Node nach dem löschen zu bestimmen
    //bei korrekter Wahl wird Node vollständig gelöscht
    public bool chooseDeletion(int neighborID)
    {
        if (markedDeletion == null) return false;
        var leftNeighbor = findSmallerNeighbor(markedDeletion);
        var rightNeighbor = findHigherNeighbor(markedDeletion);
        if (leftNeighbor?.ID == neighborID)
        {
            root = delete(root, markedDeletion.ID, true);
            markedDeletion = null;
            return true;
        }
        if (rightNeighbor?.ID == neighborID)
        {
            root = delete(root, markedDeletion.ID, false);
            markedDeletion = null;
            return true;
        }
        return false;
    }

    public AVLNode[] findChoiceforChooseDeletion()
    {
        var leftNeighbor = findSmallerNeighbor(markedDeletion);
        var rightNeighbor = findHigherNeighbor(markedDeletion);
        AVLNode[] list = {leftNeighbor, rightNeighbor};
        return list;
    }
    //standard löschen, in abhängigkeit linker oder rechter nachbar
    //mit update der Node daten
    private AVLNode delete(AVLNode node, int ID, bool left)
    {
        if (node == null)
            return node;
        if (ID < node.ID)
            node.left = delete(node.left, ID, left);
        else if (ID > node.ID)
            node.right = delete(node.right, ID, left);
        else
        {
            //falls der Knoten maximal ein Kind hat
            if ((node.left == null) || (node.right == null))
            {
                AVLNode temp = null;
                if (temp == node.left)
                    temp = node.right;
                else
                    temp = node.left;
                //Es ist ein Blatt
                if (temp == null)
                {
                    if (ID == markedDeletion.ID)
                    {
                        node.delete();
                        commandHistory.Push(new Tuple<TreeManager.Commands, int>(TreeManager.Commands.Delete, ID));
                        size--;
                    }
                    node = null;
                }
                //Es ist kein Blatt
                else
                {
                    if (ID == markedDeletion.ID)
                    {
                        node.delete();
                        commandHistory.Push(new Tuple<TreeManager.Commands, int>(TreeManager.Commands.Delete, ID));
                        size--;
                    }
                    node = temp;
                }
            }
            //Knoten hat zwei Kinder hier wird der Rechte oder Linke Nachbar ausgewählt
            else
            {
                AVLNode temp;
                if (left)
                {
                    temp = findSmallerNeighbor(node);
                }
                else
                {
                    temp = findHigherNeighbor(node);
                }
                if (temp != null)
                {
                    node = delete(node, temp.ID, left);
                    temp.left = node?.left;
                    temp.right = node?.right;
                    node.delete();
                    commandHistory.Push(new Tuple<TreeManager.Commands, int>(TreeManager.Commands.Delete, ID));
                    node = temp;
                    size--;
                }
            }
        }
        if (node == null)
            return node;

        // Update the balance factor of each node and balance the tree
        node.height = max(height(node.left), height(node.right)) + 1;
        node.setBalanceFactor(height(node.left) - height(node.right));
        return node;
    }

    private AVLNode findSmallerNeighbor(AVLNode node)
    {
        if (node.left == null) return null;
        var currentNode = node.left;
        while (currentNode.right != null)
        {
            currentNode = currentNode.right;
        }
        return currentNode;
    }

    private AVLNode findHigherNeighbor(AVLNode node)
    {
        if (node.right == null) return null;
        var currentNode = node.right;
        while (currentNode.left != null)
        {
            currentNode = currentNode.left;
        }
        return currentNode;
    }


    public void printInorder()
    {
        var allNodes = traverse();
        foreach (var node in allNodes)
        {
            Console.WriteLine(node.ID + ", ");
        }
    }


    public AVLNode[] traverse()
    {
        List<AVLNode> allNodes = new List<AVLNode>();
        traverse(root, allNodes);
        return allNodes.ToArray();
    }

    private void traverse(AVLNode node, List<AVLNode> allNodes)
    {
        //left, root, right
        if (node != null)
        {
            traverse(node.left, allNodes);
            allNodes.Add(node);
            traverse(node.right, allNodes);
        }
    }

    private void updateDepth(AVLNode node, int depth)
    {
        if (node != null)
        {
            updateDepth(node.left, depth + 1);
            node.depth = depth;
            updateDepth(node.right, depth + 1);
        }
    }


    //Errechnet die aktuellen Positionen für alle Nodes relativ zur RootNode
    //Muss vorm rendern aufgerufen werden.
    public void calculatePosition()
    {
        if (root == null)
        {
            return;
        }
        updateDepth(root, 0);
        var allNodes = traverse();
        int rootIndex = 0;
        while (allNodes[rootIndex] != root)
        {
            rootIndex++;
        }
        for (int i = 0; i < allNodes.Length; i++)
        {
            allNodes[i].updatePosition(new Vector3((i - rootIndex) * posFactorWidth + xOffset , yOffset, (-allNodes[i].depth * posFactorHeight) + zOffset));
        }
    }

    public int treeBalance(AVLNode node)
    {
        if (node != null)
        {
            var left = treeBalance(node.left);
            var thisNode = Math.Abs(node.getBalanceFactor());
            var right = treeBalance(node.right);
            return (left <= 1 ? 0 : left) + (thisNode <= 1 ? 0 : thisNode) + (right <= 1 ? 0 : right);
        }
        return 0;
    }

    public bool isTreeBalanceZero(AVLNode node)
    {
        if (node != null)
        {
            var left = isTreeBalanceZero(node.left);
            var thisNode = node.getBalanceFactor() == 0;
            var right = isTreeBalanceZero(node.right);
            return left && thisNode && right;
        }
        return true;
    }

    public void setMaterial(AVLNode node, TreeManager.NodeMaterial material)
    {
        if (node != null)
        {
            setMaterial(node.left, material);
            node.setMaterial(material);
            setMaterial(node.right, material);
        }
    }

    public void setSingleNodeMaterial(AVLNode node, TreeManager.NodeMaterial material)
    {
        if (node != null)
        {
            node.setMaterial(material);
        }
    }

    public int[] saveTree()
    {
        updateDepth(root, 0);
        var dict = new Dictionary<int, List<int>>();
        dict = depthTraversal(root, dict);
        var savedTree = new int[size];
        int i = 0;
        for (int j = 0; j < dict.Keys.Count; j++)
        {
            foreach (int k in dict[j])
            {
                savedTree[i] = k;
                i++;
            }
        }
        return savedTree;
    }

    Dictionary<int, List<int>> depthTraversal(AVLNode node, Dictionary<int, List<int>> dict)
    {
        if (node != null)
        {
            if (dict.ContainsKey(node.depth))
            {
                dict[node.depth].Add(node.ID);
            }
            else
            {
                dict.Add(node.depth, new List<int>() { node.ID });
            }
            dict = depthTraversal(node.left, dict);
            dict = depthTraversal(node.right, dict);
        }
        return dict;
    }
}