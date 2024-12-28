using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class KameraMovement : MonoBehaviour
{
    public ViewState viewState;
    //private Animator animator;
    //public Animator animatorScreen;
    AudioManager audioManager;
    public CinemachineVirtualCamera TopCam;
    public CinemachineVirtualCamera SideCam;
    public CinemachineVirtualCamera TutorialCam;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        GameManager.OnGameStateChanged += ManageCameraMovementOnGameStateChanged;
    }
    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= ManageCameraMovementOnGameStateChanged;
    }

    void Start()
    {
        //animator = GetComponent<Animator>();
        viewState = ViewState.SideView;
    }

    private void ManageCameraMovementOnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.AddPhase || gameState == GameState.SpezialAttakUnBalance || gameState == GameState.SpezialAttakDel)
        {
            if (viewState == ViewState.SideView)
            {
                audioManager.PlaySFX(audioManager.CameraSound);
                MoveToTopView();
                viewState = ViewState.TopView;
            }
        }
        else
        {
            if (viewState == ViewState.TopView)
            {
                audioManager.PlaySFX(audioManager.CameraSound);
                MoveToSideView();
                viewState = ViewState.SideView;
            }
        }
    }

    public void MoveToTopView()
    {
        SideCam.Priority = 0;
        TopCam.Priority = 1;
        
        //animator.SetTrigger("ToTopView");
        //animatorScreen.SetTrigger("ToTop");
    }

    public void MoveToSideView()
    {

        SideCam.Priority = 1;
        TopCam.Priority = 0;
        //animator.SetTrigger("ToSideView");
        //animatorScreen.SetTrigger("ToSide");
    }

    public void MoveToTutorialView(){
        TutorialCam.Priority = 2;
    }
}
public enum ViewState
{
    SideView,
    TopView
}
