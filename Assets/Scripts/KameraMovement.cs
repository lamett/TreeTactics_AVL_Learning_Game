using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class KameraMovement : MonoBehaviour
{
    public int rotationAngle = 70;
    public float moveDistance = 13f;     // Entfernung nach vorne
    public float moveUpDistance = 16f;     // Entfernung nach oben
    public float moveDuration = 0.9f;       // Dauer der gesamten Bewegung und Rotation in Sekunden

    private bool isMoving = false;
    private Quaternion targetRotation;
    private Vector3 targetPosition;

    private Quaternion originRotation; //save previous rotation to comeback
    private Vector3 originPosition;

    private float rotationSpeed;  // Berechnete Rotationsgeschwindigkeit (Grad pro Sekunde)
    private float moveSpeed;      // Berechnete Bewegungsgeschwindigkeit (Einheiten pro Sekunde)

    public ViewState viewState;

    void Awake()
    {
        //GameManager.OnGameStateChanged += ManageCameraMovementOnGameStateChanged;
    }
    void OnDestroy()
    {
        //GameManager.OnGameStateChanged -= ManageCameraMovementOnGameStateChanged;
    }

    void Start()
    {
        viewState = ViewState.SideView;

        originPosition = transform.position;
        originRotation = transform.rotation;

        targetRotation = transform.rotation;
        targetPosition = transform.position;
    }

    //void Update()
    //{
    //    if (isMoving)
    //    {
    //        // Animation der Rotation und Bewegung
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

    //        // Überprüfen, ob die Zielposition und -rotation erreicht wurden
    //        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f &&
    //            Vector3.Distance(transform.position, targetPosition) < 0.1f)
    //        {
    //            isMoving = false;
    //        }
    //    }
    //}

    void Update()
    {
        if (isMoving)
        {
            // Animate rotation and movement
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the target position and rotation have been reached
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f &&
                Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                viewState = (viewState == ViewState.SideView) ? ViewState.TopView : ViewState.SideView;
            }
        }
    }

    private void ManageCameraMovementOnGameStateChanged(GameState gameState)
    {
        Debug.Log(viewState);
        if (gameState == GameState.AddPhase || gameState == GameState.SpezialAttakUnBalance || gameState == GameState.SpezialAttakDel)
        {
            
            if (viewState == ViewState.SideView)
            {
                Debug.Log("Start to move to TopView");
                MoveToTopView();
                viewState = ViewState.TopView;
            }
        }
        else
        {
            
            if (viewState == ViewState.TopView)
            {
                Debug.Log("Start to move to SideView");
                MoveToSideView();
                viewState = ViewState.SideView;
            }
        }
    }

    public void MoveToTopView()
    {
        //if (isMoving) return; // Ignore additional calls while moving

        isMoving = true;

        // Set the target rotation by adding the rotation angle to the current rotation
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(rotationAngle, 0, 0));

        // Set the target position by moving forward and up
        targetPosition = transform.position + transform.forward * moveDistance + Vector3.up * moveUpDistance;

        // Calculate rotation and movement speed based on the target and duration
        rotationSpeed = Quaternion.Angle(transform.rotation, targetRotation) / moveDuration;
        moveSpeed = Vector3.Distance(transform.position, targetPosition) / moveDuration;
    }

    //public void MoveToTopView()
    //{
    //    if (isMoving) return; // Ignoriert weitere Aufrufe während der Bewegung

    //    isMoving = true;

    //    // Zielrotation um 90 Grad um die Y-Achse
    //    targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(rotationAngle, 0, 0));

    //    // Zielposition: ein Stück nach vorne und nach oben
    //    targetPosition = transform.position + transform.forward * moveDistance + Vector3.up * moveUpDistance;

    //    // Berechnet die Rotations- und Bewegungsgeschwindigkeit basierend auf der gewünschten Dauer
    //    rotationSpeed = (float) rotationAngle / moveDuration;  // Grad in `moveDuration` Sekunden
    //    moveSpeed = Vector3.Distance(transform.position, targetPosition) / moveDuration;
    //}

    public void MoveToSideView()
    {
        
        //if (isMoving) return; // Ignore additional calls while moving
        isMoving = true;

        // Set the target rotation and position back to the original values
        targetRotation = originRotation;
        targetPosition = originPosition;

        // Calculate rotation and movement speed based on the current difference and duration
        rotationSpeed = Quaternion.Angle(transform.rotation, targetRotation) / moveDuration;
        moveSpeed = Vector3.Distance(transform.position, targetPosition) / moveDuration;
    }

    //public void MoveToSideView()
    //{
    //    if (isMoving) return; // Ignoriert weitere Aufrufe während der Bewegung

    //    isMoving = true;

    //    targetRotation = originRotation;
    //    targetPosition = originPosition;

    //    // Berechnet die Rotations- und Bewegungsgeschwindigkeit basierend auf der gewünschten Dauer
    //    rotationSpeed = Quaternion.Angle(transform.rotation, targetRotation) / moveDuration;  // Grad in `moveDuration` Sekunden
    //    moveSpeed = Vector3.Distance(transform.position, targetPosition) / moveDuration;
    //}


}
public enum ViewState
{
    SideView,
    TopView
}
