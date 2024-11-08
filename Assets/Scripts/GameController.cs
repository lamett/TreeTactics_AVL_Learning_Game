using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    TreeManager treeManager;

    private GameObject mainCamera;

    public GameObject nodePrefab;

    public int amountBalls = 6;

    private List<GameObject> balls;

    // Start is called before the first frame update
    void Start()
    {
        treeManager = new TreeManager(nodePrefab);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        balls = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

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
