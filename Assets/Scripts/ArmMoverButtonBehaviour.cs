using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArmMoverButtonBehaviour : MonoBehaviour
{
    public Vector3 restPosition;
    //Vector3 restPosition = new Vector3(3.2887f, -0.772f, 4.561f);
    //Animator my_Animator;
    public GameObject prefab;
    public Transform Arm;
    public Transform Container;
    public GameObject testNode;

    // Start is called before the first frame update
    void Start()
    {
        //my_Animator = Arm.GetComponent<Animator>();
        //StartCoroutine(LerpPosition(restPosition, 1f));
    }

    IEnumerator LerpPosition(Vector3 newPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 bowPostion = newPosition + Vector3.up * 8;

        while (time < duration)
        {
            Vector3 tempPos = Vector3.Lerp(startPosition, bowPostion, time / duration);
            transform.position = Vector3.Lerp(tempPos, newPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = newPosition;

    }


    IEnumerator MoveThenWaitSecondsThenReturn(Vector3 targetposition, Vector3 restPosition, float seconds)
    {
        float duration = 1f;
        yield return StartCoroutine(LerpPosition(targetposition, duration));

        //my_Animator.SetTrigger("Grab");

        testNode.SetActive(false);
        GameObject spawnedPrefab = Instantiate(prefab, targetposition, Quaternion.identity);
        spawnedPrefab.transform.SetParent(Container);  // Das Prefab wird zum Kind des Mover-Objekts
        spawnedPrefab.transform.localPosition = Vector3.zero;
        spawnedPrefab.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(seconds);

        //Gehe zur�ck an Position mit Node

        yield return StartCoroutine(LerpPosition(restPosition, duration));
        //my_Animator.SetTrigger("Destroy");

        yield return new WaitForSeconds(1);
        Destroy(spawnedPrefab);


    }

    public void MoveArm(GameObject node)
    {
        restPosition = transform.position;
        Vector3 targetPosition = node.transform.Find("GripPoint").transform.position; ;
        StartCoroutine(MoveThenWaitSecondsThenReturn(targetPosition, restPosition, 2f));
    }

    public void MoveArmTest(){
        MoveArm(testNode);
    }

}
