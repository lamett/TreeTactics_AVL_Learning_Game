using System.Collections;
using UnityEngine;

public class ArmMoverButtonBehaviour : MonoBehaviour
{
    public Transform Mover; // Der Mover, der das IK-Target steuert
    public Transform Arm; // Der Arm mit Animator
    public GameObject prefab; // Prefab, das gespawnt wird

    private Vector3 ballPosition = new Vector3(-3, 0, -3); // Zielposition
    private Vector3 restPosition = new Vector3(5, 0, 0); // Ruheposition des Movers
    private Animator my_Animator; // Animator des Arms
    private GameObject spawnedPrefab; // Referenz zum gespannten Prefab

    void Start()
    {
        my_Animator = Arm.GetComponent<Animator>(); // Animator speziell für den Arm
        Mover.position = restPosition; // Initialisiere den Mover an der Ruheposition
    }

    public void MoveArm()
    {
        // Starte die Bewegungs- und Animationssequenz
        StartCoroutine(MoveThenWaitSecondsThenReturn(ballPosition, restPosition, 2f));
    }

    IEnumerator MoveThenWaitSecondsThenReturn(Vector3 targetPosition, Vector3 restPosition, float waitTime)
    {
        float duration = 1f;

        // 1. Bewege Mover zur Zielposition
        yield return StartCoroutine(LerpPosition(targetPosition, duration));

        // 2. Spiele die "Grab"-Animation ab
        my_Animator.SetTrigger("Grab");
        yield return new WaitForSeconds(1f); // Warte, bis die Animation abgeschlossen ist

        // 3. Spawne das Prefab an der Position des Movers
        spawnedPrefab = Instantiate(prefab, Mover.position, Quaternion.identity);
        spawnedPrefab.transform.SetParent(Mover);
        spawnedPrefab.transform.localPosition = Vector3.zero;

        // 4. Warte an der Position
        yield return new WaitForSeconds(waitTime);

        // 5. Bewege Mover zurück zur Ruheposition
        yield return StartCoroutine(LerpPosition(restPosition, duration));

        // 6. Spiele die "Destroy"-Animation ab
        my_Animator.SetTrigger("Destroy");
        yield return new WaitForSeconds(1.5f); // Warte, bis die Animation abgeschlossen ist

        // 7. Zerstöre das Prefab
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);
        }
    }

    IEnumerator LerpPosition(Vector3 newPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = Mover.position;

        while (time < duration)
        {
            Mover.position = Vector3.Lerp(startPosition, newPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Mover.position = newPosition;
    }
}
