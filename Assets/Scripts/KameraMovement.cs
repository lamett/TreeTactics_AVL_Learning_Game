using UnityEngine;

public class KameraMovement : MonoBehaviour
{
    public ViewState viewState;
    private Animator animator;
    public Animator animatorScreen;

    void Awake()
    {
        GameManager.OnGameStateChanged += ManageCameraMovementOnGameStateChanged;
    }
    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= ManageCameraMovementOnGameStateChanged;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        viewState = ViewState.SideView;
    }

    private void ManageCameraMovementOnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.AddPhase || gameState == GameState.SpezialAttakUnBalance || gameState == GameState.SpezialAttakDel)
        {
            if (viewState == ViewState.SideView)
            {
                MoveToTopView();
                viewState = ViewState.TopView;
            }
        }
        else
        {
            if (viewState == ViewState.TopView)
            {
                MoveToSideView();
                viewState = ViewState.SideView;
            }
        }
    }

    public void MoveToTopView()
    {
        animator.SetTrigger("ToTopView");
        animatorScreen.SetTrigger("ToTop");
    }

    public void MoveToSideView()
    {
        animator.SetTrigger("ToSideView");
        animatorScreen.SetTrigger("ToSide");
    }
}
public enum ViewState
{
    SideView,
    TopView
}
