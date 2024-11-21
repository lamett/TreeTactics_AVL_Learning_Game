using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RotatingNumbers : MonoBehaviour
{

    private int randomValue;
    private float timeInterval;

    public bool rowStopped;
    public int diceNumber;
    


    // Start is called before the first frame update
    void Start()
    {
        rowStopped = true;
        /*StartRotating();*/
    }

    public void StartRotating()
    {
        
        StartCoroutine("Rotate");
    }

    private IEnumerator Rotate()
    {
        rowStopped = false;
        timeInterval = 0.06f;


        for (int i = 0; i < 30; i++)
        {
            if (transform.localPosition.y <= -3f)
                transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

           transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.6f, 0);

            yield return new WaitForSeconds(timeInterval);
        }

        randomValue = Random.Range(20, 50);

        switch (randomValue % 2)
        {
            case 1:
                randomValue += 1; 
                break;
            /*case 2:
                randomValue += 1;
                break;*/
            
        }
        

        for (int i = 0;i < randomValue;i++)
        {
            if(transform.localPosition.y <= -3f) 
                transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);
            
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.6f,0);

            
            if (i > Mathf.RoundToInt(randomValue * 0.04f))
                timeInterval = 0.05f;
            if (i > Mathf.RoundToInt(randomValue * 0.08f))
                timeInterval = 0.1f;
            if (i > Mathf.RoundToInt(randomValue * 0.12f))
                timeInterval = 0.15f;
            if (i > Mathf.RoundToInt(randomValue * 0.15f))
                timeInterval = 0.2f;
            

            yield return new WaitForSeconds(timeInterval);
        }
        

        if (transform.localPosition.y == -3f)
            diceNumber = 1;
        else if (transform.localPosition.y == -1.8f)
            diceNumber = 2;
        else if (transform.localPosition.y == -0.6f)
            diceNumber = 3;
        else if (transform.localPosition.y == 0.6f)
            diceNumber = 4;
        else if (transform.localPosition.y == 1.8f)
            diceNumber = 5;
        else if (transform.localPosition.y == 3f)
            diceNumber = 6;
        else if (transform.localPosition.y == 4.2f)
            diceNumber = 1;

        rowStopped = true;

        
    }
    public async Task WaitRotating()
    {
        do
        {

        }while(diceNumber == 0);

        await Task.Delay(100);
        

    }
        
    



    // Update is called once per frame
    void Update()
    {
        
    }
}
