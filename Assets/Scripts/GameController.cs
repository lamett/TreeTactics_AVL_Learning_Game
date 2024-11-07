using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject nodePrefab;

    TreeManager treeManager;
    // Start is called before the first frame update
    void Start()
    {
        treeManager = new TreeManager(nodePrefab);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addRandom()
    {
        treeManager.addRandom();
    }


    public void leftRotation(int ID)
    {
        treeManager.leftRotation(ID);
    }

    public void rightRotation(int ID)
    {
        treeManager.rightRotation(ID);
    }

    public void markDeletion(int ID)
    {
        treeManager.markDeletion(ID);
    }

    public void chooseDeletion(int ID)
    {
        treeManager.chooseDeletion(ID);
    }
}
