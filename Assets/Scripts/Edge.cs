using System.Collections;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Material standardMaterial;
    public Material alternativMaterial;
    new MeshRenderer renderer;
    SpriteRenderer spriteRenderer;
    public Vector3 headPos;
    public Vector3 tailPos;
    public float markingDuration = 1f;

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.material = standardMaterial;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        renderer.enabled = false;
    }

    void Update()
    {
        if (Vector3.Distance(headPos, tailPos) > 0.01)
        {
            transform.position = (headPos + tailPos) / 2;
            transform.rotation = Quaternion.LookRotation((headPos - tailPos).normalized) * Quaternion.Euler(90, 0, 0);
            var scale = ((headPos - tailPos).magnitude / 2) - 0.5f;
            transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
            transform.GetChild(0).transform.localScale = new Vector3(10, 1 / scale, 10);
            renderer.enabled = true;
        }
    }

    public void markInsert(float startSecond)
    {
        StartCoroutine(changeMaterial(startSecond));
    }

    IEnumerator changeMaterial(float startTime)
    {
        float time = 0;
        float duration = startTime + markingDuration;
        while (time < startTime)
        {
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

    public void showArrow(bool flipped)
    {
        spriteRenderer.enabled = true;
        if (flipped)
        {
            transform.GetChild(0).transform.localEulerAngles = new Vector3(180, 0, 0);
        }
    }

    public void hideArrow()
    {
        transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
        spriteRenderer.enabled = false;
    }
}
