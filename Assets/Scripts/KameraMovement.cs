using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraMovement : MonoBehaviour
{
    public int rotationAngle = 90;
    public float moveDistance = 2f;       // Entfernung nach vorne
    public float moveUpDistance = 1f;     // Entfernung nach oben
    public float moveDuration = 1f;       // Dauer der gesamten Bewegung und Rotation in Sekunden

    private bool isMoving = false;
    private Quaternion targetRotation;
    private Vector3 targetPosition;

    private float rotationSpeed;  // Berechnete Rotationsgeschwindigkeit (Grad pro Sekunde)
    private float moveSpeed;      // Berechnete Bewegungsgeschwindigkeit (Einheiten pro Sekunde)

    void Start()
    {
        // Zielrotation und -position initialisieren
        targetRotation = transform.rotation;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            // Animation der Rotation und Bewegung
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Überprüfen, ob die Zielposition und -rotation erreicht wurden
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f &&
                Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    public void MoveRotateAndRise()
    {
        if (isMoving) return; // Ignoriert weitere Aufrufe während der Bewegung

        isMoving = true;

        // Zielrotation um 90 Grad um die Y-Achse
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(90, 0, 0));
        
        // Zielposition: ein Stück nach vorne und nach oben
        targetPosition = transform.position + transform.forward * moveDistance + Vector3.up * moveUpDistance;

        // Berechnet die Rotations- und Bewegungsgeschwindigkeit basierend auf der gewünschten Dauer
        rotationSpeed = 90f / moveDuration;  // 90 Grad in `moveDuration` Sekunden
        moveSpeed = Vector3.Distance(transform.position, targetPosition) / moveDuration;
    }
}
