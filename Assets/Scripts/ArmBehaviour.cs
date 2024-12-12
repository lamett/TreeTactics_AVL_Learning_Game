using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ArmBehaviour : MonoBehaviour
{
    [SerializeField] VisualEffect effect;

    public Transform restPosition;
    public Transform targetPosition;

    public Transform Arm;
    Animator anim;

    public GameObject prefab;

    private float speed = 2;
    private GameObject testNode;


    // Start is called before the first frame update
    void Start()
    {
       anim = Arm.GetComponent<Animator>();
       StartCoroutine(LerpPosition(restPosition.position, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LerpJumpPosition(Vector3 newPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 bowPosition = newPosition + Vector3.up * 10;

        while (time < duration)
        {
            var tempPos = Vector3.Lerp(startPosition, bowPosition, time / duration);
            transform.position = Vector3.Lerp(tempPos, newPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = newPosition;
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


    IEnumerator MoveToPositionPlayAnim(Vector3 pos, string trigger)
    {
        //Debug.Log("Gehe zur Node");
        yield return StartCoroutine(LerpJumpPosition(pos, 1f));
        anim.SetTrigger(trigger);       
    }


    IEnumerator MoveArmThenDestroy(GameObject node)
    {
        Vector3 targetPos = node.transform.position;

        yield return StartCoroutine(MoveToPositionPlayAnim(targetPos, "ClawGrab"));
        yield return new WaitForSeconds(speed);

        GameObject spawnedPrefab = Instantiate(prefab, targetPos, Quaternion.identity);
        spawnedPrefab.transform.SetParent(gameObject.transform);
        spawnedPrefab.transform.localPosition = Vector3.zero;
        spawnedPrefab.GetComponent<Rigidbody>().isKinematic = true;
        node.SetActive(false);
        //Debug.Log("Zerst�re Node");
        //Destroy(node);

        Vector3 restPos = restPosition.position;
        yield return StartCoroutine(MoveToPositionPlayAnim(restPos, "ClawDestroy"));
        yield return new WaitForSeconds(0.53f);
        Destroy(spawnedPrefab);
        effect.Play();

    }

    public void DestroyNode(GameObject node)
    {
        StartCoroutine(MoveArmThenDestroy(node));
    }

    IEnumerator MoveArmThenSnap(GameObject node)
    {
        Vector3 targetPos = node.transform.position + new Vector3(-2.5f,0,1);

        yield return StartCoroutine(MoveToPositionPlayAnim(targetPos, "ClawSnap"));
        yield return new WaitForSeconds(0.883f);

        var rb = node.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(new Vector3(1000, 200, 0));

        yield return new WaitForSeconds(0.5f);

        Vector3 restPos = restPosition.position;
        yield return StartCoroutine(LerpPosition(restPos, 1f));
        yield return new WaitForSeconds(0.53f);
        Destroy(node);
    }

    public void SnapFigure(GameObject node)
    {
        StartCoroutine(MoveArmThenSnap(node));
    }
}
