using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState;
    public GameState prevGameState;

    private GameController gameController;
    AudioManager audioManager;

    public static event Action<GameState> OnGameStateChanged;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Instance = this;
    }

    IEnumerator Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (Settings.isTutorial)
        {
            StartTutorial();
        }
        else
        {
            yield return new WaitForEndOfFrame();
            UpdateGameState(GameState.RollChallengeTalk);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        prevGameState = gameState;
        gameState = newState;



        switch (newState)
        {
            case GameState.StartMenu:
                Debug.Log("StartMenu");
                break;
            case GameState.RollChallengeTalk:
                Debug.Log("RollChallengeTalk");
                HandleRollChallangeTalk();
                break;
            case GameState.AddPhase:
                Debug.Log("AddPhase");
                HandleAddPhase();
                break;
            case GameState.DamageOnPlayer:
                Debug.Log("DamagePlayer");
                HandleDamageOnPlayer();
                break;
            case GameState.DamageOnEnemy:
                Debug.Log("DamageEnemy");
                HandleDamageOnEnemy();
                break;
            case GameState.SpezialAttakUnbalanceTalk:
                Debug.Log("SpezialAttakUnbalanceTalk");
                HandleSpezialAttakUnbalanceTalk();
                break;
            case GameState.SpezialAttakUnBalance:
                Debug.Log("SpezialAttakUnBalance");
                HandleSpezialAttakUnbalance();
                break;
            case GameState.SpezialAttakDelTalk:
                Debug.Log("SpezialAttakDelTalk");
                HandleSpezialAttakDelTalk();
                break;
            case GameState.SpezialAttakDel:
                Debug.Log("SpezialAttakDel");
                HandleSpezialAttakDel();
                break;
            case GameState.Lose:
                Debug.Log("Lose");
                HandleLose();
                break;
            case GameState.Win:
                HandleWin();
                Debug.Log("Win");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState); //ManageCameraMovementOnGameStateChanged, ManageAVLOperationsOnGameStateChanged
    }

    public async void HandleRollChallangeTalk()
    {
        await gameController.StartRollChallengeTalk();
        UpdateGameState(GameState.AddPhase);
    }

    private async void HandleAddPhase()
    {
        gameController.StartAddPhase();
        bool blinged = false;
        while (gameState == GameState.AddPhase)
        {

            if (gameController.isBling() & blinged == false)
            {
                audioManager.PlayBing(audioManager.TreeBalanced);// play bling
                blinged = true;
            }
            else if(!gameController.isBling() & blinged == true)
            {
                blinged = false;
            }

            await Task.Yield(); // Continue checking each frame
        }
    }

    public void HandleAddPhaseEnd()
    {
        if (Settings.isTutorial)
        {
            gameController.endbuttonClicked = true;
            return;
        }
        bool isAccomplished = gameController.EndAddphase();
        if (isAccomplished)
        {
            UpdateGameState(GameState.DamageOnEnemy);
        }
        else
        {
            UpdateGameState(GameState.DamageOnPlayer);
        }
    }
    private async void HandleDamageOnEnemy()
    {
        await gameController.DamageEnemy();
        if (gameController.HealthCheck() == 1)
        {
            UpdateGameState(GameState.SpezialAttakUnbalanceTalk);
        }
        else
        {
            UpdateGameState(GameState.SpezialAttakDelTalk);
        }

    }

    private async void HandleDamageOnPlayer()
    {
        await gameController.DamagePlayer();
        if (gameController.HealthCheck() == -1)
        {
            UpdateGameState(GameState.Lose);
        }
        else
        {
            switch (prevGameState)
            {
                case GameState.AddPhase:
                    gameController.resetTree();
                    UpdateGameState(GameState.RollChallengeTalk);
                    break;
                case GameState.SpezialAttakDel:
                    UpdateGameState(GameState.SpezialAttakDel);
                    HandleSpezialAttakDel();
                    break;
                case GameState.SpezialAttakUnBalance:
                    throw new NotImplementedException();
                //break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(prevGameState), prevGameState, null);
            }
        }

    }

    private async void HandleSpezialAttakDelTalk()
    {
        await gameController.StartSpezialAttakDelTalk();
        UpdateGameState(GameState.SpezialAttakDel);
    }

    private void HandleSpezialAttakDel()
    {
        gameController.StartSpezialAttakDel();
    }

    public async void HandleSpezialAttakUnbalanceTalk()
    {
        await gameController.StartSpezialAttakUnbalanceTalk();
        UpdateGameState(GameState.SpezialAttakUnBalance);
    }

    private async void HandleSpezialAttakUnbalance()
    {
        await gameController.StartSpezialAttakUnbalance();
        //UpdateGameState(GameState.Win);
    }

    private async void HandleWin()
    {
        await gameController.StartWin();
        GameObject.FindGameObjectWithTag("SideCam").GetComponent<Animator>().SetTrigger("FadeOut");
        await Task.Delay(4000);
        SceneManager.LoadScene("StartMenu");
    }

    private async void HandleLose()
    {
        await gameController.StartLose();
        GameObject.FindGameObjectWithTag("SideCam").GetComponent<Animator>().SetTrigger("FadeOut");
        await Task.Delay(4000);
        SceneManager.LoadScene("StartMenu");
    }

    public void ChangeToDamageOnPlayer()
    {
        UpdateGameState(GameState.DamageOnPlayer);
    }

    public void ChangeToRollChallengeTalk()
    {
        UpdateGameState(GameState.RollChallengeTalk);
    }

    public void ChangeToLose()
    {
        UpdateGameState(GameState.Lose);
    }

    public void ChangeToWin()
    {
        UpdateGameState(GameState.Win);
    }

    public async void StartTutorial()
    {
        await gameController.Tutorial();
    }
}

public enum GameState
{
    StartMenu,
    RollChallengeTalk, //sideView, rolls Amount of Balls to add, generate Balls in Bowl
    AddPhase, // topView add, balance balls
    DamageOnPlayer,
    DamageOnEnemy,
    SpezialAttakUnbalanceTalk, // sideView, Animation Grip
    SpezialAttakUnBalance, // topView balance
    SpezialAttakDelTalk, //sideView, Animation Tableshake
    SpezialAttakDel, // topView, choose Ball
    Lose,
    Win
}
