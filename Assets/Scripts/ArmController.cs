using System.Collections;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public Transform mover; // Referenz auf den Mover
    public Transform restPosition; // Position, wo der Arm ruht
    public Transform targetPosition; // Zielposition, wo der Arm hingeht
    public GameObject prefab; // Prefab, das gespawnt wird
    public Animator armAnimator; // Animator für Grab/Destroy Animationen
    private GameObject spawnedPrefab;

    public float moveDuration = 2f;

    void Start()
    {
        // Arm startet an der Ruheposition
        mover.position = restPosition.position;
    }

    public void StartSequence()
    {
        StartCoroutine(ArmSequence());
    }

    IEnumerator ArmSequence()
    {
        // 1. Bewege den Mover zur Zielposition
        yield return MoveToPosition(targetPosition.position);

        // 2. Führe die "Grab"-Animation aus
        armAnimator.SetTrigger("Grab");
        yield return new WaitForSeconds(1f); // Warte, bis die Animation abgeschlossen ist

        // 3. Spawne das Prefab an der Zielposition
        spawnedPrefab = Instantiate(prefab, mover.position, Quaternion.identity);

        // 4. Bewege den Arm zurück zur Ruheposition
        yield return MoveToPosition(restPosition.position);

        // 5. Führe die "Destroy"-Animation aus
        armAnimator.SetTrigger("Destroy");
        yield return new WaitForSeconds(1f); // Warte, bis die Animation abgeschlossen ist

        // 6. Zerstöre das gespawnte Prefab
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPos)
    {
        Vector3 startPos = mover.position;
        float elapsed = 0;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            mover.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            yield return null;
        }

        mover.position = targetPos;
    }
}
