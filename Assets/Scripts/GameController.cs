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

    Timer timer;

    public int playerStartHealth = 3;
    public int enemyStartHealth = 5;
    public int amountBalls = 6;
    int leftNodesToAdd = 0;
    public int currentRound = 1;

    private List<GameObject> balls;

    // Start is called before the first frame update
    void Start()
    {
        treeManager = new TreeManager(nodePrefab, updateTreeBalance);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player.GetComponent<HealthScript>().setHealth(playerStartHealth);
        enemy.GetComponent<HealthScript>().setHealth(enemyStartHealth);
        timer = new Timer(this, addPhaseEnd, addPhaseTimeUpdate);
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
        if(leftNodesToAdd > 0 || !treeManager.isBalanced()){
            player.GetComponent<HealthScript>().reduceHealth();
            checkHealth();
            return true;
        }
        else{
            enemy.GetComponent<HealthScript>().reduceHealth();
            checkHealth();
            return false;
        }
    }

    void checkHealth(){
        if(player.GetComponent<HealthScript>().Health <= 0){
            gameOver("You Lose");
        }
        if(enemy.GetComponent<HealthScript>().Health <= 0)
        {
            gameOver("You win");
        }
    }

    //#########-Methoden GameLoop-#################
    public void endAddphase()
    {
        currentRound++;
        timer.stopTimer();
        disableBallsClick();
        mainCamera.GetComponent<KameraMovement>().MoveToSideView();
        leftNodesToAdd = balls.Count;
        clearBowl();
        if(!dealDamage()){
            specialAttack();
        }
        else
        {
            // treeMananger.rewind() //baum = oldBaum.Copy()
            // currentRound --;
            // startAddphase()
        }
    }

    async public void startAddPhase()
    {
        treeManager.backUpTree();
        await SpawnBallsAsync();
        mainCamera.GetComponent<KameraMovement>().MoveToTopView();
        enableBallsCLick();
        timer.startTimer(amountBalls * 10, 0.2f);
    }

    public void specialAttack()
    {
        if (!(currentRound % 2 == 0)){
            specialAttackDelete();
        }
        else{
            specialAttackUnbalance();
        }
    }

    public void specialAttackDelete(){
        //
    }

    public void specialAttackUnbalance() { 
        //
    }

    public void gameOver(string msg){
        Debug.Log(msg);
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
}
