using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TMPro;

public class GameController : MonoBehaviour
{
    TreeManager treeManager;

    private GameObject mainCamera;
    private HealthScript player;
    private HealthScript enemy;
    private TMP_Text dummyText;
    public GameObject nodePrefab;
    private Button endButton;
    public UnityEvent<int> updateTreeBalance;

    public UnityEvent EndAddPhaseEvent;
    public UnityEvent ChangeToDamageOnPlayerEvent;
    public UnityEvent ChangeToRollChallengeTalk;
    public UnityEvent<float> addPhaseTimeUpdate;
    //public UnityEvent EndSpezialPhaseByTimer;
    //public UnityEvent<float> specialPhaseTimeUpdate;

    Timer addPhaseTimer;
    //Timer specialPhaseTimer;

    public int playerStartHealth = 3;
    public int enemyStartHealth = 3;
    public int amountBalls = 0;
    int leftNodesToAdd = 0;
    public NewRotating rotating;
    public RotationTextScript rotationTextScript;
    public EnemyHurtText enemyHurtText;
    public PlayerHurtText playerHurtText;  
    public SpecialAttackText specialAttackText;
    public CheckText checkText;
    public EnemyWinText enemyWinText;
    public PlayerWinText playerWinText; 


    private List<GameObject> balls;
    Stack<Tuple<TreeManager.Commands, int>> commandHistory = new Stack<Tuple<TreeManager.Commands, int>>();
    [SerializeField] private RotationButtons rotationButtonsPanel;

    // Start is called before the first frame update

    void Awake()
    {
        GameManager.OnGameStateChanged += ManageAVLOperationsOnGameStateChanged;
    }
    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= ManageAVLOperationsOnGameStateChanged;
    }
    void Start()
    {
        treeManager = new TreeManager(nodePrefab, updateTreeBalance, commandHistory);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScript>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<HealthScript>();
        player.setHealth(playerStartHealth);
        enemy.setHealth(enemyStartHealth);
        addPhaseTimer = new Timer(this, EndAddPhaseEvent, addPhaseTimeUpdate);
        //specialPhaseTimer = new Timer(this, EndSpezialPhaseByTimer, specialPhaseTimeUpdate);
        balls = new List<GameObject>();
        dummyText = GameObject.FindGameObjectWithTag("DummyText").GetComponent<TMP_Text>();
        endButton = GameObject.FindGameObjectWithTag("EndButton").GetComponent<Button>();
        endButton.hide();
    }
    private void ManageAVLOperationsOnGameStateChanged(GameState gameState)
    {
        if (balls != null)
        {
            switch (gameState)
            {
                case GameState.AddPhase:
                    enableBallsClickDelPhase(false);
                    enableBallsClickAdd(true);
                    enableBallsClickOperation(true);
                    break;
                case GameState.SpezialAttakUnBalance:
                    enableBallsClickOperation(true);
                    enableBallsClickDelPhase(false);
                    break;
                case GameState.SpezialAttakDel:
                    enableBallsClickDelPhase(true);
                    break;
                default:
                    enableBallsClickAdd(false);
                    enableBallsClickOperation(false);
                    enableBallsClickDelPhase(false);
                    break;
            }
        }
    }

    public void chooseAmountBalls()
    {
        amountBalls = rotating.diceNumber;
        /*var rnd = new System.Random();
        amountBalls = rnd.Next(4, 8);
        */
    }

    //Handle Phases
    async public Task StartRollChallengeTalk()
    {
        rotating.GenerateRotation(); // Startet rotieren der Nummern
        await rotating.WaitRotating();// wartet bis fertig gerollt (bis jetzt nur 12s)
        rotationTextScript.StartDialogue();
        chooseAmountBalls();
        setDummyText("Add " + amountBalls + " Nodes");
        //ScreenAnimation
        await SpawnBallsAsync();
    }

    public void StartAddPhase()
    {
        endButton.show();
        treeManager.backUpTree();
        addPhaseTimer.startTimer(amountBalls * 10, 0.2f);
        commandHistory.Clear(); // kann eigentlich auch zum Event OnGameStateChanged hinzugefügt werden
    }

    //returns if Challenge was accomplished or not
    public bool EndAddphase()
    {
        checkText.StartDialogue();
        addPhaseTimer.stopTimer();
        leftNodesToAdd = balls.Count;
        clearBowl();
        commandHistory.Clear();

        if (leftNodesToAdd > 0 || !treeManager.isBalanced()) { return false; }
        else { return true; }
    }

    public async Task DamageEnemy()
    {
        enemyHurtText.StartDialogue();
        enemy.reduceHealth();
        setDummyText("Damage on Enemy. Remaining Health:" + enemy.Health);
        rotating.rotatingNumber += 1;
        await Task.Delay(2000);
    }
    public async Task DamagePlayer()
    {
        playerHurtText.StartDialogue();
        player.reduceHealth();
        setDummyText("Damage on Player. Remaining Health:" + player.Health);
        await Task.Delay(2000);
    }

    public int HealthCheck()
    {
        if (enemy.isDead())
        {
            playerWinText.StartDialogue();
            return 1;
        }
        else if (player.isDead())
        {
            enemyWinText.StartDialogue();   
            return -1;
        }
        else { return 0; }
    }

    public void resetTree()
    {
        treeManager.destroyTree();
        treeManager.rebuildTree();
    }

    public async Task StartSpezialAttakDelTalk()
    {
        specialAttackText.StartDialogue();
        treeManager.markDeletion(treeManager.findNodeToDelete()); //makes random Node small
        treeManager.markGapFillers(); //sets higher and smaller neighbourgh to isGapFiller = true
        setDummyText("Knoten gelöscht, wähle einen Knoten um das Loch zu füllen");
        await Task.Delay(1000);
        //Animation
    }

    public void StartSpezialAttakDel()
    {
        //specialPhaseTimer.startTimer(30, 0.2f);
    }

    public void EndSpezialAttakDel()
    {
        //specialPhaseTimer.stopTimer();
        Debug.Log("end delAttak");
        treeManager.resetGapFillers();
        Debug.Log(treeManager.isBalanced());

        ChangeToRollChallengeTalk.Invoke();


    }

    public void RerunSpezialAttakDel()
    {
        treeManager.colorGapFillers();
        ChangeToDamageOnPlayerEvent.Invoke();
    }


    //async public void startAddPhase()
    //{
    //    treeManager.backUpTree(); //hier soll der back up tree gespeichert werden...methode ist im momment noch leer
    //    chooseAmountBalls();
    //    await SpawnBallsAsync();
    //    mainCamera.GetComponent<KameraMovement>().MoveToTopView();
    //    enableBallsClickAddPhase();
    //    addPhaseTimer.startTimer(amountBalls * 10, 0.2f);
    //    commandHistory.Clear();
    //}

    //public void specialAttack()
    //{
    //    if (currentRound % 2 == 0)
    //    {
    //        startSpecialAttackDelete();
    //    }
    //    else
    //    {
    //        specialAttackUnbalance(); //noch leer
    //        Debug.Log("SpecialAttak Unbalance, add phase kann erneut gestartet werden");
    //    }
    //}

    //public async void startSpecialAttackDelete()
    //{
    //    enableBallsClickDelPhase(true);
    //    //mainCamera.GetComponent<KameraMovement>().MoveToTopView(); //bugfix
    //    await Task.Delay(300);
    //    treeManager.markDeletion(treeManager.findNodeToDelete());//treemanager.delete() // jetzt soll vom computer ein knoten gel�scht werden
    //    specialPhaseTimer.startTimer(20, 0.2f); //actuell ist das noch ein bug -> siehe hacknplan
    //}

    //public void endSpecialAttackDeletion(bool gotDeletionRight)
    //{
    //    bool isBalanced = treeManager.isBalanced();
    //    specialPhaseTimer.stopTimer();
    //    disableBallsClick();
    //    if (gotDeletionRight && !isBalanced)
    //    {
    //        //noch mal in die addphase aber ohne kugeln in der sch�ssel und ohne timer

    //    }
    //    if (gotDeletionRight && isBalanced)
    //    {
    //        //zurUck zur default stage
    //    }
    //    if (!gotDeletionRight)
    //    {
    //        //correktur
    //        //damage
    //        //defaultstage
    //    }
    //}

    //public void specialAttackUnbalance()
    //{
    //    //
    //}

    //public void gameOver(string msg)
    //{
    //    Debug.Log(msg);
    //}
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

    public void OpenRotationPanel(AVLOperations operations)
    {
        rotationButtonsPanel.gameObject.SetActive(true);
        rotationButtonsPanel.SetNode(operations);
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
        if (balls.Count != 0)
        {
            foreach (GameObject ball in balls)
            {
                ball.GetComponent<AVLOperations>().setIsAddable(activate);
                ball.GetComponent<AVLOperations>().setIsOperatable(false);
            }
        }
    }

    //controlls all balls in Tree
    public void enableBallsClickOperation(bool activate)
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

    public bool chooseDeletion(int ID)
    {
        return treeManager.chooseDeletion(ID);
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
                enableBallsClickAdd(true);
                enableBallsClickOperation(true);
                break;
            case TreeManager.Commands.Delete:
                treeManager.addObject(treeManager.instantiateBallForBowl(), command.Item2);
                commandHistory.Pop();
                break;
        }
    }

    public void showAllBF()
    {
        treeManager.showAllBF();
    }

    //#####-Methode zu Test zwecken-#############
    async public void addFromButton()
    {
        chooseAmountBalls();
        await SpawnBallsAsync();
        enableBallsClickAdd(true);
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

    private void setDummyText(string text)
    {
        dummyText.text = text;
    }
}
