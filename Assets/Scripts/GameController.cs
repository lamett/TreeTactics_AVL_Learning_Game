using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    TreeManager treeManager;

    private GameObject mainCamera;
    private GameObject player;
    private GameObject enemy;
    public GameObject nodePrefab;
    public UnityEvent<int> updateTreeBalance;

    public UnityEvent addPhaseEnd;
    public UnityEvent<float> addPhaseTimeUpdate;

    Timer AddPhaseTimer;

    public int amountBalls = 6;
    int leftNodesToAdd = 0;
    private List<GameObject> balls;

    // Start is called before the first frame update
    void Start()
    {
        treeManager = new TreeManager(nodePrefab, updateTreeBalance);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        AddPhaseTimer = new Timer(this, addPhaseEnd, addPhaseTimeUpdate);
        balls = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void chooseAmountBalls()
    {
        var rnd = new System.Random();
        amountBalls = rnd.Next(2, 6);
    }

    bool dealDamage()
    {
        if(leftNodesToAdd > 0){
            player.GetComponent<PlayerScript>().reduceHealth();
            return true;
        } 
        return false;
    }

    void checkHealth(){
        if(player.GetComponent<PlayerScript>().Health <= 0){
            gameOver();
        }
    }

    //#########-Methoden GameLoop-#################
    public void endAddphase()
    {
        AddPhaseTimer.stopTimer();
        disableBallsClick();
        mainCamera.GetComponent<KameraMovement>().MoveToSideView();
        leftNodesToAdd = balls.Count;
        clearBowl();
        if(!dealDamage()){
            specialAttack();
        }
        checkHealth();
    }

    async public void startAddPhase()
    {
        await SpawnBallsAsync();
        mainCamera.GetComponent<KameraMovement>().MoveToTopView();
        enableBallsCLick();
        AddPhaseTimer.startTimer(amountBalls * 1000, 0.2f);
    }

    public void specialAttack(){

    }

    public void gameOver(){
        Debug.Log("GameOver!");
    }
    //#############################################

    private async Task SpawnBallsAsync()
    {
        chooseAmountBalls();
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

    private void clearBowl()
    {
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
        balls.Clear();
    }

    public bool addFromBowl(GameObject ball)
    {
        int ID = treeManager.calculateID();

        if (treeManager.addObject(ball, ID))
        {
            balls.Remove(ball);
            return true;
        }
        return false;
    }

    public void disableBallsClick()
    {
        enableBallsClickAdd(false);
        enableBallsClickOperation(false);
    }

    public void enableBallsCLick()
    {
        enableBallsClickAdd(true);
        enableBallsClickOperation(true);
    }

    //controlls all Balls in Bowl
    private void enableBallsClickAdd(bool activate)
    {
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<AVLOperations>().setIsAddable(activate);
            ball.GetComponent<AVLOperations>().setIsOperatable(false);
        }
    }

    //controlls all balls in Tree
    private void enableBallsClickOperation(bool activate)
    {
        foreach (GameObject ball in treeManager.getTreeAsGOArray())
        {
            ball.GetComponent<AVLOperations>().setIsOperatable(activate);
            ball.GetComponent<AVLOperations>().setIsAddable(false);
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

    //#####-Methode zu Test zwecken-#############
    async public void addFromButton()
    {
        await SpawnBallsAsync();
        enableBallsCLick();
    }

    public void randomRot(){
        treeManager.rotateRandom(10);
    }
    public void balance(){
        treeManager.balanceTreeCompletly();
    }
}
