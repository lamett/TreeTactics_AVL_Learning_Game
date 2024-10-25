
using UnityEngine;
using UnityEngine.UIElements;

public class Edge : MonoBehaviour
{
    public Vector3 headPos;
    public Vector3 tailPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (headPos + tailPos) / 2;
        transform.rotation = Quaternion.LookRotation((headPos - tailPos).normalized);
        //transform.rotation = Quaternion.
        transform.localScale = new Vector3(transform.localScale.x,  (headPos - tailPos).magnitude, transform.localScale.z);
    }
}
