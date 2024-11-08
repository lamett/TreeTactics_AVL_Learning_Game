using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartCube : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent switchToAddPhase;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
         if (Input.GetMouseButtonDown(0))
        {
            switchToAddPhase.Invoke();
            Destroy(gameObject);
        }
    }

}
