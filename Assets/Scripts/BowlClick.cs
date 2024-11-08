using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BowlClick : MonoBehaviour
{

    public UnityEvent OnClickBowl;

    // Start is called before the first frame update
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
            OnClickBowl.Invoke();
        }
    }
}
