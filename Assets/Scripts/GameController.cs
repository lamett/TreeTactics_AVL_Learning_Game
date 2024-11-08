using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    TreeManager treeManager;

    private GameObject mainCamera;
    
    public GameObject nodePrefab;
    public UnityEvent<int> updateTreeBalance;

    public int amountBalls = 6;
    int leftNodesToAdd = 0;
    private List<GameObject> balls;

    // Start is called before the first frame update
    void Start()
    {
        treeManager = new TreeManager(nodePrefab, updateTreeBalance);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        balls = new List<GameObject>();
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
    public void addFromButton()
    {
        int ID = treeManager.calculateIDHard();
        treeManager.add(ID);
    }

    //GameLoop AddPhase
    async public void addPhase()
    {
        await SpawnBallsAsync();
        mainCamera.GetComponent<KameraMovement>().MoveRotateAndRise();
        enableBallsClick();
    }

    private async Task SpawnBallsAsync()
    {
        for (int i = 0; i < amountBalls; i++)
        {
            GameObject ball = treeManager.instantiateBallForBowl();
            balls.Add(ball);
            ball.GetComponent<AVLNode>().hideID();
            ball.GetComponent<AVLNode>().hideBF();
            await Task.Delay(300);
        }
        await Task.Delay(1000);
    }

    private void enableBallsClick()
        {
            foreach (GameObject ball in balls)
            {
                ball.GetComponent<AVLOperations>().setIsAddable(true);
            }
        }

    public bool addFromBowl(GameObject ball)
    {
        int ID = treeManager.calculateID();

        if(treeManager.addObject(ball, ID)){
            balls.Remove(ball);
            return true;
        }
        return false;
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
