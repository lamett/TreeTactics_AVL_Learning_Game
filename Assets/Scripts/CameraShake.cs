using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //private IEnumerator Shake(float duration, float magnitude)
    //{
    //    Vector3 originalpose = transform.localPosition;

    //    float elapsed = 0.0f;

    //    while (elapsed < duration)
    //    {
    //        float x = Random.Range(-1f, 1f) * magnitude;
    //        float y = Random.Range(-1f, 1f) * magnitude;

    //        transform.localPosition = new Vector3(x, y, originalpose.z);

    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    transform.localPosition = originalpose;
    //}

    //public void StartShake()
    //{
    //    Debug.Log("StartShake");
    //    StartCoroutine(Shake(100f, 10f));
    //}

}
