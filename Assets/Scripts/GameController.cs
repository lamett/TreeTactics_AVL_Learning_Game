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
using UnityEngine.VFX;

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

    public UnityEvent ChangeToWin;
    public UnityEvent ChangeToLose;
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
        //HardMode
        if (Settings.HardModeActivated)
        {
            enemyStartHealth += 2;
        }
        DeleteAllInstancesOfNodes();
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

    void DeleteAllInstancesOfNodes()
    {
        // Alle GameObjects in der Szene finden
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Node");

        // Prüfen, ob ein GameObject mit dem angegebenen Prefab übereinstimmt
        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }

        Debug.Log($"Alle Instanzen des Prefabs {nodePrefab.name} wurden gelöscht.");
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
    async public Task StartRollChallengeTalk(GameState prevGameState)
    {
        if (prevGameState == GameState.StartMenu)
        {
            if (Settings.HardModeActivated) 
            {
                await Task.Delay(3000);
                textBox.index = 20;
                textBox.StartDialogue();
                await Task.Delay(1000);
            }
            else
            {
                await Task.Delay(3000);
                textBox.index = 1;
                textBox.StartDialogue();
            }
        }
        else
        {
            await Task.Delay(2000);
            textBox.index = 2;
            textBox.StartDialogue();
        }
        await Task.Delay(3000);
        await showRotation();
        await Task.Delay(1500);
        textBox.index = UnityEngine.Random.Range(3, 5);
        textBox.StartDialogue();
        chooseAmountBalls(-1);
        //setDummyText("Add " + amountBalls + " Nodes");
        //ScreenAnimation
        await SpawnBallsAsync();
    }

    public void StartAddPhase()
    {
        endless = true;
        //endButton.show();
        treeManager.backUpTree();
        float timeleft = Settings.HardModeActivated ? amountBalls * 4 : amountBalls * 10;
        addPhaseTimer.startTimer(timeleft, 0.2f);
        audioManager.StartTimer();
        commandHistory.Clear(); // kann eigentlich auch zum Event OnGameStateChanged hinzugefügt werden
    }

    //returns if Challenge was accomplished or not
    public bool EndAddphase()
    {
        endless = false;
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
    }
    public async Task DamagePlayer()
    {
        textBox.index = UnityEngine.Random.Range(11, 13);
        textBox.StartDialogue();
        player.reduceHealth();
        //setDummyText("Damage on Player. Remaining Health:" + player.Health);
        await Task.Delay(2000);
    }

    public int HealthCheck()
    {
        if (enemy.isDead())
        {

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
        await Task.Delay(2000);

        enemy.GetComponent<Animator>().SetTrigger("NoFigures");

        await Task.Delay(500);

        textBox.index = 18;
        textBox.StartDialogue();

        await Task.Delay(2700);

        textBox.index = 19;
        textBox.StartDialogue();

        await Task.Delay(3000);

        textBox.index = UnityEngine.Random.Range(7, 9);
        textBox.StartDialogue();

        await Task.Delay(1500);
        enemy.GetComponent<Animator>().SetTrigger("JumpOnTable"); //triggers shake and RandomRot as animation event

        await Task.Delay(4000);
    }


    public async Task StartSpezialAttakUnbalance()
    {
        audioManager.StartTimer();
        float time = Settings.HardModeActivated ? treeManager.BalancedFactorOfTree() : treeManager.BalancedFactorOfTree() * 2;
        Debug.Log(time);
        specialPhaseTimer.startTimer(time, 0.2f);

        await DuringSpezialAttakUnbalance();
        if (treeManager.isBalanced())
        {
            audioManager.PlayBing(audioManager.TreeBalanced);
            await Task.Delay(1000);
            Debug.Log("invoke win");
            ChangeToWin.Invoke();
        }
        else
        {
            await Task.Delay(1000);
            Debug.Log("invoke lose");
            //ChangeToLose.Invoke();
        }
    }

    public async Task DuringSpezialAttakUnbalance()
    {
        while (!treeManager.isBalanced())
        {
            await Task.Yield(); // Continue checking each frame
        }
    }

    public async Task StartWin()
    {

        audioManager.StopTimer();
        specialPhaseTimer.stopTimer();
        audioManager.StopBossMusic();

        await Task.Delay(1000);

        await ExplodeNodes();

        await Task.Delay(1000);

        enemy.GetComponent<Animator>().SetTrigger("JumpOffTable");
        await Task.Delay(2500);
        enemy.GetComponent<Animator>().SetTrigger("Happy");
        await Task.Delay(500);
        textBox.index = 15;
        textBox.StartDialogue();
        await Task.Delay(5000);
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
        await Task.Delay(5000);
    }

    public bool endbuttonClicked = false;
    public bool undoButtonClicked = false;
    public bool endDelTutorial = false;
    public bool isEndOfTutorial = false;
    bool endless = false;


    public async Task Tutorial()
    {
        Settings.isTutorial = true;
        textBox.gameObject.SetActive(false);
        rotating.gameObject.SetActive(false);
        await genericText.PrintOnScreen("Willkommen! Du möchtest also mein Spiel lernen.", 1.5f);
        await genericText.PrintOnScreen("Hier ist der erste Datensatz.");
        chooseAmountBalls(4);
        await SpawnBallsAsync();
        treeManager.backUpTree();
        commandHistory.Clear();
        mainCamera.GetComponent<KameraMovement>().MoveToTutorialView();
        await Task.Delay(1200);
        await genericText.PrintOnScreen("Mit einem Klick fügst du die Elemente dem AVL-Baum hinzu.");
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
        await genericText.PrintOnScreen("Stop! Hier ist etwas aus dem Gleichgewicht geraten.", 1.5f);
        Utils.StartPulsing(treeManager.getRoot().gameObject);

        await genericText.PrintOnScreen("Halte die Kugel gedrückt und zieh nach rechts oder links zum Rotieren.");
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
                await genericText.PrintOnScreen("Ajajaj, das braucht aber lange. Erinnere dich, was du im Tutorial der vorherigen Gruppe gelernt hast!");
            }
            await Task.Delay(100);
        }
        enableBallsClickOperation(false);
        commandCount = commandHistory.Count;
        await genericText.PrintOnScreen("Sehr gut.", 1f);

        Utils.StartPulsing(balls);
        await genericText.PrintOnScreen("Füge nun das letzte Element hinzu.");
        while (commandCount >= commandHistory.Count)
        {
            await Task.Delay(100);
        }
        audioManager.PlayBing(audioManager.TreeBalanced);
        Utils.StopPulsing();
        endbuttonClicked = false;
        await genericText.PrintOnScreen("Beende deinen Zug.");
        Utils.StartPulsing(EndButtonObject);
        while (!endbuttonClicked)
        {
            await Task.Delay(100);
        }
        Utils.StopPulsing();
        endbuttonClicked = false;
        commandHistory.Clear();
        await genericText.PrintOnScreen("Diese Runde geht an dich.");
        enemy.reduceHealth();
        await Task.Delay(6000);

        var node = treeManager.findNodeToDelete();
        Arm.DestroyNode(treeManager.findNode(node).gameObject);
        await Task.Delay(1000);
        treeManager.markDeletion(node);
        treeManager.markGapFillers();
        await Task.Delay(4000);
        await genericText.PrintOnScreen("Ups...", 1.5f);
        await genericText.PrintOnScreen("Mit welcher Kugel kannst du die Lücke schließen?");
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
        await genericText.PrintOnScreen("Auf in die nächste Runde.");
        chooseAmountBalls(2);
        await SpawnBallsAsync();
        treeManager.backUpTree();
        commandHistory.Clear();
        enableBallsClickDelPhase(false);
        enableBallsClickAdd(true);
        enableBallsClickOperation(true);
        //Utils.StartPulsing(balls);
        //await genericText.PrintOnScreen("Füge nun diese Kugeln hinzu");
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
        await genericText.PrintOnScreen("Hier siehst du wie unbalanciert dein Baum ist.", 3);
        Utils.StopPulsing();
        await genericText.PrintOnScreen("Da kannst du deinen Schritt rückgängig machen.");
        Utils.StartPulsing(UndoButtonObject);
        while (!undoButtonClicked)
        {
            await Task.Delay(100);
        }
        enableBallsClickOperation(false);
        Utils.StopPulsing();
        Utils.StartPulsing(TimerObject);
        await genericText.PrintOnScreen("Vorsicht! Die Zeit läuft.");
        audioManager.StartTimer();
        addPhaseTimer.startTimer(1.5f, 0.2f);
        //TODO start TimerSound
        await Task.Delay(3000);
        Utils.StopPulsing();
        addPhaseTimer.stopTimer();
        audioManager.StopTimer();
        player.reduceHealth();
        resetTree();
        await genericText.PrintOnScreen("Diese Runde geht an mich.", 1.5f);

        await genericText.PrintOnScreen("Übe noch so viel du möchtest. Beende die Übung einfach mit dem Knopf.");
        endbuttonClicked = false;
        isEndOfTutorial = true;
        chooseAmountBalls(15);
        endless = true;
        await SpawnBallsAsync();
        treeManager.backUpTree();
        commandHistory.Clear();
        enableBallsClickAdd(true);
        enableBallsClickOperation(true);
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

    public async Task StartSandbox()
    {
        Settings.isSandbox = true;
        textBox.gameObject.SetActive(false);
        rotating.gameObject.SetActive(false);
        mainCamera.GetComponent<KameraMovement>().MoveToTutorialView();
        await genericText.PrintOnScreen("Sandbox!");

        chooseAmountBalls(5);
        await SpawnBallsAsync();
        treeManager.backUpTree();
        commandHistory.Clear();
        enableBallsClickAdd(true);

        while (true)
        {
            if (balls.Count <= 0)
            {
                chooseAmountBalls(5);
                await SpawnBallsAsync();
                enableBallsClickAdd(true);
                await genericText.PrintOnScreen("Sandbox!\n\n\nKlick mich.");
            }
            await Task.Delay(100);
        }
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

    private async Task ExplodeNodes()
    {
        GameObject[] nodes = treeManager.getTreeAsGOArray();
        int delay = 100;
        float round = 0;
        foreach (GameObject node in nodes) {
            node.GetComponentInChildren<VisualEffect>().Play();
            audioManager.PlaySFX(audioManager.OrbDestoryed);
            
            node.GetComponent<MeshRenderer>().enabled = false;
            MeshRenderer[] childs = node.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer child in childs)
            { 
                child.enabled = false;
            }

            node.GetComponent<AVLNode>().DestroyEdges();
            double currentDelay = delay*Math.Cos(round/10);
            Debug.Log(currentDelay);
            if(currentDelay > 50) {
                await Task.Delay((int) Math.Round(currentDelay));
            }
            else
            {
                await Task.Delay(50);
            }

            round++;
        }

    }
    public async Task showRotation()
    {
        textBox.EmptyText();
        DiceHolder.SetActive(true);
        audioManager.StartMusic(audioManager.CasinoSpin);
        //rotating.GenerateRotation(); //calculate number
        await rotating.RunRotationAsTask(); //start rotation
        await Task.Delay(2000);
        DiceHolder.SetActive(false);
    }

    public void randomRot()
    {
        if (treeManager.Count() <= 0) return;
        treeManager.rotateRandom(10);
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
            audioManager.PlaySFX(audioManager.InsertBall);
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
        if (endless)
        {
            audioManager.PlayBing(audioManager.TreeBalanced);
            chooseAmountBalls(5);
            await SpawnBallsAsync();
            enableBallsClickAdd(true);
        }
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
