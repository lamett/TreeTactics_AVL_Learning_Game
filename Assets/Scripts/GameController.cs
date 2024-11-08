using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    int leftNodesToAdd = 0;

    public GameObject nodePrefab;
    public UnityEvent<int> updateTreeBalance;

    TreeManager treeManager;
    // Start is called before the first frame update
    void Start()
    {
        treeManager = new TreeManager(nodePrefab, updateTreeBalance);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void chooseCountOfNodes(){
        var rnd = new System.Random();
        leftNodesToAdd = rnd.Next(2, 5);
    }

    int damageToTake(){
        return leftNodesToAdd == 0 ? 0 : 1;
    }
    int damageToDeal(){
        return leftNodesToAdd == 0 ? 1 : 0;
    }




    //Schleust aktuell die interaktive Methoden zum Baum durch
    public void addRandom()
    {
        treeManager.addHard(true);
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
