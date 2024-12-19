using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class NewRotating : MonoBehaviour
{ 
    
    
    private float timeInterval;

    public bool rowStopped;
    public int diceNumber;
    public int rotatingNumber;  
    AudioManager audioManager;


    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rotatingNumber = 1;
        rowStopped = true;
    }
    
   
    public void GenerateRotation()
    {
        if (rotatingNumber == 4)
        {
            rotatingNumber = 1;
        }
        else if (rotatingNumber == 1)
        {
            diceNumber = Random.Range(4, 8);
            StartRotating();
        }
        else if (rotatingNumber == 2)
        {
            diceNumber = Random.Range(5, 9);
            StartRotating();
        }
        else if (rotatingNumber == 3)
        {
            diceNumber  = Random.Range(6, 10);
            StartRotating();
        }
    }

    public void StartRotating()
    {
        audioManager.StartMusic(audioManager.CasinoSpin);
        StartCoroutine("Rotate");
    }

    private IEnumerator Rotate()
    {
        rowStopped = false;
        timeInterval = 0.005f;
        transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

        for (int i = 0; i < 160; i++)
        {
            if (transform.localPosition.y <= -5.7f)
                transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.1f, 0);

            yield return new WaitForSeconds(timeInterval);
        }

        if (diceNumber == 4)
        {
            for (int i = 0; i < 140; i++)
            {
                if (transform.localPosition.y <= -5.7f)
                    transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.1f, 0);


                if (i > Mathf.RoundToInt(59 * 0.04f))
                    timeInterval = 0.0065f;
                if (i > Mathf.RoundToInt(59 * 0.08f))
                    timeInterval = 0.0085f;
                if (i > Mathf.RoundToInt(59 * 0.12f))
                    timeInterval = 0.01f;
                if (i > Mathf.RoundToInt(59 * 0.15f))
                    timeInterval = 0.015f;


                yield return new WaitForSeconds(timeInterval);
            }
        }

        else if (diceNumber == 5)
        {
            for (int i = 0; i < 123; i++)
            {
                if (transform.localPosition.y <= -5.7f)
                    transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.1f, 0);


                if (i > Mathf.RoundToInt(59 * 0.04f))
                    timeInterval = 0.0065f;
                if (i > Mathf.RoundToInt(59 * 0.08f))
                    timeInterval = 0.0085f;
                if (i > Mathf.RoundToInt(59 * 0.12f))
                    timeInterval = 0.01f;
                if (i > Mathf.RoundToInt(59 * 0.15f))
                    timeInterval = 0.015f;


                yield return new WaitForSeconds(timeInterval);
            }

        }

        else if (diceNumber == 6)
        {
            for (int i = 0; i < 106; i++)
            {
                if (transform.localPosition.y <= -5.7f)
                    transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.1f, 0);


                if (i > Mathf.RoundToInt(59 * 0.04f))
                    timeInterval = 0.0065f;
                if (i > Mathf.RoundToInt(59 * 0.08f))
                    timeInterval = 0.0085f;
                if (i > Mathf.RoundToInt(59 * 0.12f))
                    timeInterval = 0.01f;
                if (i > Mathf.RoundToInt(59 * 0.15f))
                    timeInterval = 0.015f;


                yield return new WaitForSeconds(timeInterval);
            }

        }

        else if (diceNumber == 7)
        {
            for (int i = 0; i < 89; i++)
            {
                if (transform.localPosition.y <= -5.7f)
                    transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.1f, 0);


                if (i > Mathf.RoundToInt(59 * 0.04f))
                    timeInterval = 0.0065f;
                if (i > Mathf.RoundToInt(59 * 0.08f))
                    timeInterval = 0.0085f;
                if (i > Mathf.RoundToInt(59 * 0.12f))
                    timeInterval = 0.01f;
                if (i > Mathf.RoundToInt(59 * 0.15f))
                    timeInterval = 0.015f;


                yield return new WaitForSeconds(timeInterval);
            }

        }

        else if (diceNumber == 8)
        {
            for (int i = 0; i < 72; i++)
            {
                if (transform.localPosition.y <= -5.7f)
                    transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.1f, 0);


                if (i > Mathf.RoundToInt(59 * 0.04f))
                    timeInterval = 0.0065f;
                if (i > Mathf.RoundToInt(59 * 0.08f))
                    timeInterval = 0.0085f;
                if (i > Mathf.RoundToInt(59 * 0.12f))
                    timeInterval = 0.01f;
                if (i > Mathf.RoundToInt(59 * 0.15f))
                    timeInterval = 0.015f;


                yield return new WaitForSeconds(timeInterval);
            }

        }

        else if (diceNumber == 9)
        {
            for (int i = 0; i < 55; i++)
            {
                if (transform.localPosition.y <= -5.7f)
                    transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.1f, 0);


                if (i > Mathf.RoundToInt(59 * 0.04f))
                    timeInterval = 0.0065f;
                if (i > Mathf.RoundToInt(59 * 0.08f))
                    timeInterval = 0.0085f;
                if (i > Mathf.RoundToInt(59 * 0.12f))
                    timeInterval = 0.01f;
                if (i > Mathf.RoundToInt(59 * 0.15f))
                    timeInterval = 0.015f;


                yield return new WaitForSeconds(timeInterval);
            }

        }
        audioManager.StopMusic(audioManager.CasinoSpin);
        rowStopped = true;
        
    }
    
    public async Task WaitRotating()
    {
        await Task.Delay((8-rotatingNumber)*1000);
    }
}
