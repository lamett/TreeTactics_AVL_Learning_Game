using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBehaviour : MonoBehaviour
{

    public Transform restPosition;
    public Transform targetPosition;

    public Transform Mover;
    public Transform Arm;
    Animator anim;

    public GameObject prefab;

    private float speed = 2;
    private GameObject testNode;
    

    // Start is called before the first frame update
    void Start()
    {
       testNode = Instantiate(prefab, targetPosition.position, Quaternion.identity);
       anim = Arm.GetComponent<Animator>();
       StartCoroutine(LerpPosition(restPosition.position, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LerpPosition(Vector3 newPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = Mover.transform.position;

        while (time < duration)
        {
            Mover.transform.position = Vector3.Lerp(startPosition, newPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Mover.transform.position = newPosition;
    }


    IEnumerator MoveToPositionPlayAnim(Vector3 pos, string trigger)
    {
        Debug.Log("Gehe zur Node");
        yield return StartCoroutine(LerpPosition(pos, 1f));
        anim.SetTrigger(trigger);       
    }


    IEnumerator MoveArmThenDestroy(GameObject node)
    {
        Vector3 targetPos = node.transform.position;

        yield return StartCoroutine(MoveToPositionPlayAnim(targetPos, "ClawGrab"));
        yield return new WaitForSeconds(speed);

        GameObject spawnedPrefab = Instantiate(prefab, targetPos, Quaternion.identity);
        spawnedPrefab.transform.SetParent(Mover);
        spawnedPrefab.transform.localPosition = Vector3.zero;
        Debug.Log("Zerstöre Node");
        Destroy(node);

        Vector3 restPos = restPosition.position;
        yield return StartCoroutine(MoveToPositionPlayAnim(restPos, "ClawDestroy"));
        yield return new WaitForSeconds(0.53f);
        Destroy(spawnedPrefab);

    }

    public void TestMovement()
    {
        StartCoroutine(MoveArmThenDestroy(testNode));
    }
}
