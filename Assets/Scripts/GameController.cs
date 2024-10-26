
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject nodePrefab;
    List<int> possibleNumbers;

    AVLTree baum;
    // Start is called before the first frame update
    void Start()
    {
        baum = new AVLTree();
        possibleNumbers = Enumerable.Range(0, 50).ToList();
        shuffle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Randomiziert die Liste von möglichen Nodes
    void shuffle()
    {
        var rnd = new System.Random();
        possibleNumbers = possibleNumbers.OrderBy(item => rnd.Next()).ToList();
    }

    public void add()
    {
        if (possibleNumbers.Count > 0)
        {
            var prefab = Instantiate(nodePrefab);
            var node = prefab.GetComponent<AVLNode>();
            node.ID = possibleNumbers.First();
            possibleNumbers.RemoveAt(0);
            prefab.transform.GetChild(0).GetComponent<TextMeshPro>().text = node.ID.ToString();
            baum.insert(node);
            baum.calculatePosition();
        }
    }

    public void leftRotation(int ID)
    {
        baum.lRot(ID);
        baum.calculatePosition();
    }

    public void rightRotation(int ID)
    {
        baum.rRot(ID);
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
