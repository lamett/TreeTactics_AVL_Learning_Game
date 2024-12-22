using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TMPro;
using UnityEditor;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

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
    //public GameObject playerFigureHolder;

    public UnityEvent EndAddPhaseEvent;
    public UnityEvent ChangeToDamageOnPlayerEvent;
    public UnityEvent ChangeToRollChallengeTalk;
    public UnityEvent<float> addPhaseTimeUpdate;
    public UnityEvent EndSpezialPhaseByTimer;
    public UnityEvent<float> specialPhaseTimeUpdate;
    public UnityEvent EndDelAttackByTimer;
    //public UnityEvent<float> delAttackTimeUpdate;

    Timer addPhaseTimer;
    Timer specialPhaseTimer;
    //Timer delAttackTimer;

    public int playerStartHealth = 3;
    public int enemyStartHealth = 4;
    public int amountBalls = 0;
    int leftNodesToAdd = 0;
    public GameObject DiceHolder;
    private NewRotating rotating;
    public ArmBehaviour Arm;
    public TextBox textBox;
    public TextBoxGeneric genericText;

    public GameObject platine;
    public GameObject UndoButtonObject;
    public GameObject EndButtonObject;
    public GameObject TimerObject;
    AudioManager audioManager;  

    private List<GameObject> balls;
    Stack<Tuple<TreeManager.Commands, int>> commandHistory = new Stack<Tuple<TreeManager.Commands, int>>();
    [SerializeField] private RotationButtons rotationButtonsPanel;

    // Start is called before the first frame update

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
        //playerFigureHolder = GameObject.FindGameObjectWithTag("PlayerFigureHolder");
        rotating = DiceHolder.GetComponentInChildren<NewRotating>();
        player.setHealth(playerStartHealth);
        enemy.setHealth(enemyStartHealth);
        addPhaseTimer = new Timer(this, EndAddPhaseEvent, addPhaseTimeUpdate);
        specialPhaseTimer = new Timer(this, EndSpezialPhaseByTimer, specialPhaseTimeUpdate);
        balls = new List<GameObject>();

        //HardMode
        if (Settings.HardModeActivated)
        {
            enemyStartHealth += 2;
            enemy.setHealth(enemyStartHealth);
        }

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

    public void chooseAmountBalls(int num)
    {
        if (num <= 0)
        {
            amountBalls = rotating.diceNumber;
        }
        else
        {
            amountBalls = num;
        }
    }

    //Handle Phases
    async public Task StartRollChallengeTalk()
    {
        await showRotation();
        textBox.index = UnityEngine.Random.Range(3, 5);
        textBox.StartDialogue();
        chooseAmountBalls(-1);
        //setDummyText("Add " + amountBalls + " Nodes");
        //ScreenAnimation
        await SpawnBallsAsync();
    }

    public void StartAddPhase()
    {

        //endButton.show();
        treeManager.backUpTree();
        addPhaseTimer.startTimer(amountBalls * 10, 0.2f);
        audioManager.StartTimer();
        commandHistory.Clear(); // kann eigentlich auch zum Event OnGameStateChanged hinzugefügt werden
    }

    //returns if Challenge was accomplished or not
    public bool EndAddphase()
    {
        audioManager.StopTimer();
        addPhaseTimer.stopTimer();
        leftNodesToAdd = balls.Count;
        clearBowl();
        commandHistory.Clear();

        if (leftNodesToAdd > 0 || !treeManager.isBalanced()) { return false; }
        else { return true; }
    }

    public async Task DamageEnemy()
    {
        textBox.index = UnityEngine.Random.Range(9, 11);
        textBox.StartDialogue();
        enemy.reduceHealth();
        //setDummyText("Damage on Enemy. Remaining Health:" + enemy.Health);
        rotating.rotatingNumber += 1;
        await Task.Delay(2000);
        audioManager.PlaySFX(audioManager.EnemyTakesDamage);
    }
    public async Task DamagePlayer()
    {
        textBox.index = UnityEngine.Random.Range(11, 13);
        textBox.StartDialogue();
        player.reduceHealth();
        //setDummyText("Damage on Player. Remaining Health:" + player.Health);
        await Task.Delay(2000);
        audioManager.PlaySFX(audioManager.EnemyTakesDamage);
    }

    public int HealthCheck()
    {
        if (enemy.isDead())
        {
            textBox.index = UnityEngine.Random.Range(7, 9);
            textBox.StartDialogue();
            return 1;
        }
        else if (player.isDead())
        {
            textBox.index = UnityEngine.Random.Range(5, 7);
            textBox.StartDialogue();
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
        await Task.Delay(1800);
        textBox.index = UnityEngine.Random.Range(13, 15);
        textBox.StartDialogue();
        var node = treeManager.findNodeToDelete();
        Arm.DestroyNode(treeManager.findNode(node).gameObject);
        //setDummyText("Knoten gelöscht, wähle einen Knoten um das Loch zu füllen");
        await Task.Delay(1000);
        //delAttackTimer.startTimer(treeManager.Count() * 1, 0.2f);
        treeManager.markDeletion(node); //makes random Node small
        treeManager.markGapFillers(); //sets higher and smaller neighbourgh to isGapFiller = true
        //Animation
        await Task.Delay(3500);
        audioManager.PlaySFX(audioManager.OrbDestoryed);
    }

    public void StartSpezialAttakDel()
    {
        //specialPhaseTimer.startTimer(30, 0.2f);
    }

    public void EndSpezialAttakDel()
    {
        //delAttackTimer.stopTimer();
        Debug.Log("end delAttak");
        treeManager.resetGapFillers();
        Debug.Log(treeManager.isBalanced());

        ChangeToRollChallengeTalk.Invoke();
    }

    public void RerunSpezialAttakDel()
    {
        //delAttackTimer.stopTimer();
        //delAttackTimer.startTimer(treeManager.Count() * 1, 0.2f);
        treeManager.colorGapFillers();
        ChangeToDamageOnPlayerEvent.Invoke();
    }

    public async Task StartSpezialAttakUnbalanceTalk()
    {
        //setDummyText("Du hast mich noch nicht besiegt!");
        Debug.Log("xxxStartSpezialAttakUnbalanceTalkxxx");
        await Task.Delay(500);
        enemy.GetComponent<Animator>().SetTrigger("JumpOnTable"); //triggers shake and RandomRot as animation event
        
        await Task.Delay(4000);
    }


    public async Task StartSpezialAttakUnbalance()
    {
        audioManager.StartTimer();
        specialPhaseTimer.startTimer(20, 0.2f);
        while (!treeManager.isBalanced())
        {
            await Task.Yield(); // Continue checking each frame
        }
        audioManager.PlayBing(audioManager.TreeBalanced);
        await Task.Delay(1000);
    }

    public async Task StartWin()
    {
     
        audioManager.StopTimer();
        specialPhaseTimer.stopTimer();
        audioManager.StopBossMusic();

        await Task.Delay(500);
        enemy.GetComponent<Animator>().SetTrigger("JumpOffTable");
        await Task.Delay(1200);

        textBox.index = 15;
        textBox.StartDialogue();
        await Task.Delay(8000);
    }

    public async Task StartLose()
    {
        audioManager.StopTimer();
        specialPhaseTimer.stopTimer();
        audioManager.StopBossMusic();

        await Task.Delay(500);
        enemy.GetComponent<Animator>().SetTrigger("JumpOffTable");
        await Task.Delay(2500);

        enemy.GetComponent<Animator>().SetTrigger("TiltHead");
        textBox.index = 16;
        textBox.StartDialogue();
        
        await Task.Delay(2500);
        textBox.index = 17;
        textBox.StartDialogue();
        await Task.Delay(8000);
    }

    public bool endbuttonClicked = false;
    public bool undoButtonClicked = false;
    public bool endDelTutorial = false;
    public bool isEndOfTutorial = false;


    public async Task Tutorial()
    {
        Settings.isTutorial = true;
        rotating.gameObject.SetActive(false);
        await genericText.PrintOnScreen("Willkommen! Du möchtest also mein Spiel lernen.", 1.5f);
        await genericText.PrintOnScreen("Du bekommst Kugeln, die musst du hinzufügen.");
        chooseAmountBalls(4);
        await SpawnBallsAsync();
        treeManager.backUpTree();
        commandHistory.Clear();
        mainCamera.GetComponent<KameraMovement>().MoveToTopView();
        await Task.Delay(1200);
        await genericText.PrintOnScreen("Klicke auf die Kugeln in der Schale.");
        enableBallsClickDelPhase(false);
        enableBallsClickAdd(true);
        Utils.StartPulsing(balls);
        var commandCount = commandHistory.Count;
        while (commandHistory.Count < 3)
        {
            if (commandCount < commandHistory.Count)
            {
                Utils.StopPulsing(treeManager.findNode(commandHistory.Peek().Item2).gameObject);
            }
            await Task.Delay(100);
        }
        Utils.StopPulsing();
        await genericText.PrintOnScreen("Jetzt kannst du keine Kugeln hinzufügen, denn dein Baum ist nicht ausbalanciert", 1.5f);
        Utils.StartPulsing(treeManager.getRoot().gameObject);

        await genericText.PrintOnScreen("Halte die Kugel gedrückt und zieh nach rechts oder links");
        enableBallsClickOperation(true);
        var notBalanced = true;
        var erinnerung = false;
        while (notBalanced)
        {
            notBalanced = !treeManager.isBalanced();
            if (commandHistory.Count > 3)
            {
                Utils.StopPulsing();
            }
            if (commandHistory.Count > 8 && !erinnerung)
            {
                erinnerung = true;
                await genericText.PrintOnScreen("Erinnere Dich, was du im Tutorial der vorherigen Gruppe gelernt hast!");
            }
            await Task.Delay(100);
        }
        enableBallsClickOperation(false);
        commandCount = commandHistory.Count;
        await genericText.PrintOnScreen("Der Baum ist grün, also ist er wieder balanciert", 1.5f);

        Utils.StartPulsing(balls);
        await genericText.PrintOnScreen("Füge nun die letzte Kugel hinzu");
        while (commandCount >= commandHistory.Count)
        {
            await Task.Delay(100);
        }
        Utils.StopPulsing();
        await genericText.PrintOnScreen("Sehr gut, nun beende deinen Zug");
        Utils.StartPulsing(EndButtonObject);
        while (!endbuttonClicked)
        {
            await Task.Delay(100);
        }
        Utils.StopPulsing();
        endbuttonClicked = false;
        commandHistory.Clear();
        await genericText.PrintOnScreen("Du hast diese Runde gewonnen also ziehe ich mir ein Leben ab");
        enemy.reduceHealth();
        await Task.Delay(2000);
        await genericText.PrintOnScreen("Da du so gut warst, hab ich eine kleine Challange für dich", 1.5f);

        var node = treeManager.findNodeToDelete();
        Arm.DestroyNode(treeManager.findNode(node).gameObject);
        await Task.Delay(1000);
        treeManager.markDeletion(node);
        treeManager.markGapFillers();
        await Task.Delay(1000);

        await genericText.PrintOnScreen("Ich habe dir eine Kugel gelöscht. Klicke auf die Kugel, die an den Fehlenden Platz musst");
        var gapFillers = treeManager.getTreeAsGOArray().Where(node => node.GetComponent<AVLNode>().isGapFiller);
        Utils.StartPulsing(gapFillers.ToList());
        foreach (var gapFiller in gapFillers)
        {
            gapFiller.GetComponent<AVLOperations>().setIsChoosableForDel(true);
        }

        while (!endDelTutorial)
        {
            await Task.Delay(100);
        }
        treeManager.resetGapFillers();
        Utils.StopPulsing();
        await genericText.PrintOnScreen("Sehr gut, nun beginnt eine neue Runde");
        chooseAmountBalls(2);
        await SpawnBallsAsync();
        treeManager.backUpTree();
        commandHistory.Clear();
        enableBallsClickDelPhase(false);
        enableBallsClickAdd(true);
        Utils.StartPulsing(balls);
        await genericText.PrintOnScreen("Füge nun diese Kugeln hinzu");
        commandCount = commandHistory.Count;
        while (commandHistory.Count < 2)
        {
            if (commandCount < commandHistory.Count)
            {
                Utils.StopPulsing(treeManager.findNode(commandHistory.Peek().Item2).gameObject);
            }
            await Task.Delay(100);
        }
        Utils.StopPulsing();

        Utils.StartPulsing(platine.GetComponent<showTreeBalance>().LEDs());
        await genericText.PrintOnScreen("Hier kannst du erkennen, wie unbalanciert dein Baum ist", 3);
        Utils.StopPulsing();
        await genericText.PrintOnScreen("Du kannst natürlich auch deinen Zug Rückgängig machen.");
        Utils.StartPulsing(UndoButtonObject);
        while (!undoButtonClicked)
        {
            await Task.Delay(100);
        }
        enableBallsClickOperation(false);
        Utils.StopPulsing();
        Utils.StartPulsing(TimerObject);
        await genericText.PrintOnScreen("Übrigends gibt es auch eine Zeitlimit");
        addPhaseTimer.startTimer(1.5f, 0.2f);
        //TODO start TimerSound
        await Task.Delay(3000);
        Utils.StopPulsing();
        addPhaseTimer.stopTimer();
        player.reduceHealth();
        resetTree();
        await genericText.PrintOnScreen("Ich war gemein, ich wollte dir nur zeigen, du startest immer mit deinem vorherigem Baum neu", 2.5f);

        await genericText.PrintOnScreen("Übe noch so viel du möchtest ohne Zeidruck. Beende die Übung einfach mit dem Knopf");
        endbuttonClicked = false;
        isEndOfTutorial = true; ;
        chooseAmountBalls(15);
        await SpawnBallsAsync();
        treeManager.backUpTree();
        commandHistory.Clear();
        enableBallsClickAdd(true);
        Utils.StartPulsing(EndButtonObject);

        while (!endbuttonClicked)
        {
            await Task.Delay(100);
        }
        Utils.StopPulsing();
        endbuttonClicked = false;
        Settings.isTutorial = false;
        SceneManager.LoadScene("StartMenu");
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

    public async Task showRotation()
    {
        DiceHolder.SetActive(true);
        audioManager.StartMusic(audioManager.CasinoSpin);
        //rotating.GenerateRotation(); //calculate number
        await rotating.RunRotationAsTask(); //start rotation
        await Task.Delay(1000);
        DiceHolder.SetActive(false);
    }

    public void randomRot()
    {
        int i = 0;
        while (treeManager.isBalanced() && i < 10)
        {
            treeManager.rotateRandom(10);
            i++;
        }

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
        audioManager.PlaySFX(audioManager.OrbsMoving);
        treeManager.leftRotation(ID);
    }

    public void rightRotation(int ID)
    {
        audioManager.PlaySFX(audioManager.OrbsMoving);
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
                audioManager.PlaySFX(audioManager.OrbsMoving);
                break;
            case TreeManager.Commands.RotateRight:
                treeManager.leftRotation(command.Item2);
                commandHistory.Pop();
                audioManager.PlaySFX(audioManager.OrbsMoving);
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

        if (Settings.isTutorial)
        {
            undoButtonClicked = true;
        }
    }

    public void showAllBF()
    {
        treeManager.showAllBF();
    }

    //#####-Methode zu Test zwecken-#############
    async public void addFromButton()
    {
        chooseAmountBalls(-1);
        await SpawnBallsAsync();
        enableBallsClickAdd(true);
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
    public bool isBling()
    {
        if (balls.Count == 0 && treeManager.isBalanced())
        {
            return true;
        }
        else { return false; }
    }

    /*private void setDummyText(string text)
    {
        dummyText.text = text;
    }*/
}
