using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
    Stack<Tuple<TreeManager.Commands, int>> commandHistory = new Stack<Tuple<TreeManager.Commands, int>>();

    // Start is called before the first frame update
    void Start()
    {
        treeManager = new TreeManager(nodePrefab, updateTreeBalance, commandHistory);
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
        if (leftNodesToAdd > 0 || !treeManager.isBalanced())
        {
            player.GetComponent<HealthScript>().reduceHealth();
            checkHealth();
            return true;
        }
        else
        {
            enemy.GetComponent<HealthScript>().reduceHealth();
            checkHealth();
            return false;
        }
    }

    void checkHealth()
    {
        if (player.GetComponent<HealthScript>().Health <= 0)
        {
            gameOver("You Lose");
        }
        if (enemy.GetComponent<HealthScript>().Health <= 0)
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
        commandHistory.Clear();
        if (!dealDamage())
        {
            Debug.Log("starte special Attack");
            specialAttack();
        }
        else
        {
            treeManager.destroyTree();
            treeManager.rebuildTree();
            enableBallsClickAddPhase();
            startAddPhase();
        }
    }

    async public void startAddPhase()
    {
        treeManager.backUpTree(); //hier soll der back up tree gespeichert werden...methode ist im momment noch leer
        chooseAmountBalls();
        await SpawnBallsAsync();
        mainCamera.GetComponent<KameraMovement>().MoveToTopView();
        enableBallsClickAddPhase();
        timer.startTimer(amountBalls * 10, 0.2f);
        commandHistory.Clear();
    }

    public void specialAttack()
    {
        if (currentRound % 2 == 0)
        {
            startSpecialAttackDelete();
        }
        else
        {
            specialAttackUnbalance(); //noch leer
            Debug.Log("SpecialAttak Unbalance, add phase kann erneut gestartet werden");
        }
    }

    public async void startSpecialAttackDelete()
    {
        enableBallsClickDelPhase(true);
        //mainCamera.GetComponent<KameraMovement>().MoveToTopView(); //bugfix
        await Task.Delay(300);
        treeManager.markDeletion(treeManager.findNodeToDelete());//treemanager.delete() // jetzt soll vom computer ein knoten gel�scht werden
        timer.startTimer(20, 0.2f); //actuell ist das noch ein bug -> siehe hacknplan
    }

    public void endSpecialAttackDeletion(bool gotDeletionRight, bool isBalanced)
    {
        disableBallsClick();
        if (gotDeletionRight && !isBalanced)
        {
            //noch mal in die addphase aber ohne kugeln in der sch�ssel und ohne timer

        }
        if (gotDeletionRight && isBalanced)
        {
            //zurUck zur default stage
        }
        if (!gotDeletionRight)
        {
            //correktur
            //damage
            //defaultstage
        }
    }

    public void specialAttackUnbalance()
    {
        //
    }

    public void gameOver(string msg)
    {
        Debug.Log(msg);
    }
    //#############################################

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
        enableBallsClickDelPhase(false);
    }

    public void enableBallsClickAddPhase()
    {
        enableBallsClickDelPhase(false);
        enableBallsClickAdd(true);
        enableBallsClickOperation(true);
    }

    private void enableBallsClickDelPhase(bool activate)
    {
        foreach (GameObject ball in treeManager.getTreeAsGOArray())
        {
            ball.GetComponent<AVLOperations>().setIsChoosableForDel(activate);
        }
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

    public async void undo()
    {
        if (commandHistory.Count <= 0)
        {
            return;
        }
        var command = commandHistory.Pop();
        switch (command.Item1)
        {
            case TreeManager.Commands.RotateLeft:
                treeManager.rightRotation(command.Item2);
                commandHistory.Pop();
                break;
            case TreeManager.Commands.RotateRight:
                treeManager.leftRotation(command.Item2);
                commandHistory.Pop();
                break;
            case TreeManager.Commands.Insert:
                treeManager.markDeletion(command.Item2);
                commandHistory.Pop();
                amountBalls = 1;
                await SpawnBallsAsync();
                enableBallsClickAddPhase();
                break;
            case TreeManager.Commands.Delete:
                treeManager.addObject(treeManager.instantiateBallForBowl(), command.Item2);
                commandHistory.Pop();
                break;
        }
    }

    //#####-Methode zu Test zwecken-#############
    async public void addFromButton()
    {
        chooseAmountBalls();
        await SpawnBallsAsync();
        enableBallsClickAddPhase();
    }

    public void randomRot()
    {
        treeManager.rotateRandom(10);
    }
    public void balance()
    {
        treeManager.balanceTreeCompletly();
    }

    public void killTree()
    {
        treeManager.destroyTree();
        treeManager.rebuildTree();
    }
}
