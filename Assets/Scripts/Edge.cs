using System.Collections;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Material standardMaterial;
    public Material alternativMaterial;
    new MeshRenderer renderer;
    public Vector3 headPos;
    public Vector3 tailPos;
    public float markingDuration = 1f;

    void Awake(){
        renderer = GetComponent<MeshRenderer>();
        renderer.material = standardMaterial;
    }

    void Update()
    {
        if (Vector3.Distance(headPos, tailPos) > 0.01)
        {
            transform.position = (headPos + tailPos) / 2;
            transform.rotation = Quaternion.LookRotation((headPos - tailPos).normalized) * Quaternion.Euler(90, 0, 0);
            transform.localScale = new Vector3(transform.localScale.x, ((headPos - tailPos).magnitude / 2) - 0.5f, transform.localScale.z);
        }
    }

    public void markInsert(float startSecond){
        StartCoroutine(changeMaterial(startSecond));
    }

    IEnumerator changeMaterial(float startTime)
    {
        float time = 0;
        float duration = startTime + markingDuration;
        while(time < startTime){
            time += Time.deltaTime;
            yield return null;
        }
        renderer.material = alternativMaterial;

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        renderer.material = standardMaterial;
    }
}
