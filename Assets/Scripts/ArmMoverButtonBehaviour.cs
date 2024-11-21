using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMoverButtonBehaviour : MonoBehaviour
{

    private Vector3 ballPosition = new Vector3(-3, 0, -3);
    private Vector3 restPosition = new Vector3(3,0,0);
    Animator my_Animator;
    public GameObject prefab;
    public Transform Mover;
    public Transform Arm;
    

   
    // Start is called before the first frame update
    void Start()
    {
        my_Animator = Arm.GetComponent<Animator>();
        StartCoroutine(LerpPosition(restPosition, 1f));
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator LerpPosition(Vector3 newPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = newPosition;

    }


    IEnumerator MoveThenWaitSecondsThenReturn(Vector3 targetposition, Vector3 restPosition, float seconds)
    {
        float duration = 1f;
        yield return StartCoroutine(LerpPosition(targetposition, duration));
  
        my_Animator.SetTrigger("Grab");
        
        GameObject spawnedPrefab = Instantiate(prefab, targetposition, Quaternion.identity);
        spawnedPrefab.transform.SetParent(Mover);  // Das Prefab wird zum Kind des Mover-Objekts
        spawnedPrefab.transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(seconds);

        //Gehe zurück an Position mit Node

        yield return StartCoroutine(LerpPosition(restPosition, duration));
        my_Animator.SetTrigger("Destroy");

        yield return new WaitForSeconds(1);
        Destroy(spawnedPrefab);
        
       
    }

    public void MoveArm()
    {
        ballPosition += new Vector3(0, 0, -1);

        StartCoroutine(MoveThenWaitSecondsThenReturn(ballPosition, restPosition, 2f));

    }

}
