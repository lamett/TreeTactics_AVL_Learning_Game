using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour 
{

    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var pos = mainCamera.transform.position;
        if(mainCamera.transform.position.y > 6){
            pos += new Vector3(0,20,-20);
        }
        transform.GetChild(0).transform.LookAt(pos);
        transform.GetChild(0).transform.Rotate(0,180,0);
        transform.GetChild(1).transform.LookAt(pos);
        transform.GetChild(1).transform.Rotate(0,180,0);
    }
 
}
