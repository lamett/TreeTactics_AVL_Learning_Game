using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

    public GameObject nodePrefab;
    TreeManager treeManager;
    private int[] IDs;
    private List<GameObject> balls;

    // Start is called before the first frame update
    void Start()
    {
        treeManager = new TreeManager(nodePrefab);
        balls = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addRandom()
    {
        treeManager.addRandom();
    }

    public void addFromBowl()
    {
        if(balls.Count > 0){
            GameObject ball = balls[0];
            if(treeManager.addObject(ball)){
                balls.RemoveAt(0);    
            }
        }
    }

    private void setIDs()
    {
        int[] h = {1,2,3,4,5};
        this.IDs = h;
    }

    public void startGenerateMultiple(){
        setIDs();
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls(){
        foreach(int ID in this.IDs){
            GameObject ball = treeManager.generate(ID);
            balls.Add(ball);
            yield return new WaitForSeconds(0.7f);
        }
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
