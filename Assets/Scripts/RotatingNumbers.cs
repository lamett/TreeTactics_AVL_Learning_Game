using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingNumbers : MonoBehaviour
{

    private int randomValue;
    private float timeInterval;

    public bool rowStopped;
    public string stoppedRow;


    // Start is called before the first frame update
    void Start()
    {
        rowStopped = true;
        StartRotating();
    }

    public void StartRotating()
    {
        stoppedRow = "";
        StartCoroutine("Rotate");
    }

    private IEnumerator Rotate()
    {
        rowStopped = false;
        timeInterval = 0.051f;


        for (int i = 0; i < 30; i++)
        {
            if (transform.position.y <= -2.6f)
                transform.position = new Vector3(transform.position.x, 7.6f, 7.4f);

           transform.position = new Vector3(transform.position.x, transform.position.y - 0.51f, 7.4f);

            yield return new WaitForSeconds(timeInterval);
        }

        randomValue = Random.Range(60, 100);

        switch (randomValue % 4)
        {
            case 1:
                randomValue += 3; 
                break;
            case 2:
                randomValue += 2;
                break;
            case 3: 
                randomValue += 1;
                break;
        }
        

        for (int i = 0;i < randomValue;i++)
        {
            if(transform.position.y <= -2.6f) 
                transform.position = new Vector3(transform.position.x, 7.6f, 7.4f);
            
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.51f, 7.4f);

            
            if (i > Mathf.RoundToInt(randomValue * 0.051f))
                timeInterval = 0.05f;
            if (i > Mathf.RoundToInt(randomValue * 0.102f))
                timeInterval = 0.1f;
            if (i > Mathf.RoundToInt(randomValue * 0.153f))
                timeInterval = 0.15f;
            if (i > Mathf.RoundToInt(randomValue * 0.204f))
                timeInterval = 0.2f;
            

            yield return new WaitForSeconds(timeInterval);
        }
        

        if (transform.position.y == -3f)
            stoppedRow = "1";
        else if (transform.position.y == -1.8f)
            stoppedRow = "2";
        else if (transform.position.y == -0.6f)
            stoppedRow = "3";
        else if (transform.position.y == 0.6f)
            stoppedRow = "4";
        else if (transform.position.y == 1.8f)
            stoppedRow = "5";
        else if (transform.position.y == 3f)
            stoppedRow = "6";
        else if (transform.position.y == 4.2f)
            stoppedRow = "1";

        rowStopped = true;
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
