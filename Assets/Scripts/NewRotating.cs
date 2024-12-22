using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

public class NewRotating : MonoBehaviour
{ 
    
    
    public bool rowStopped;
    public int diceNumber;
    public int rotatingNumber;  
    AudioManager audioManager;

    float stepdistance = 0.1f;
    private float v = 3f;
    float timeInterval = 0.005f;

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
            rotatingNumber = 1; //zurücksetzen auf 1. Runde
        }
        else if (rotatingNumber == 1) //erste Runde
        {
            diceNumber = Random.Range(4, 8);
            StartRotating();
        }
        else if (rotatingNumber == 2) //zweite Runde
        {
            diceNumber = Random.Range(5, 9);
            StartRotating();
        }
        else if (rotatingNumber == 3) //dritte Runde
        {
            diceNumber  = Random.Range(6, 10);
            StartRotating();
        }
    }

    public void StartRotating()
    {
        audioManager.StartMusic(audioManager.CasinoSpin);
        StartCoroutine("RotateNew");
    }

    private IEnumerator RotateNew()
    {
        diceNumber = 8;

        rowStopped = false;
        Vector3 endpos = new Vector3(transform.localPosition.x, 4.2f, 0);
        int num = 0;

        transform.localPosition = endpos; //reset position

        for (int i = 0; i < 1*(99/v); i++)
        {
            if (transform.localPosition.y <= -5.7f) { transform.localPosition = new Vector3(transform.localPosition.x, 4.2f, 0);}
                
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - stepdistance*v, 0);

            yield return new WaitForSeconds(timeInterval);
        }

        transform.localPosition = endpos; //reset position

        switch (diceNumber)
        {
            case 4:
                endpos = new Vector3(transform.localPosition.x, -5.7f, 0);
                num = 32;
                break;
            case 5:
                num = 26;
                endpos = new Vector3(transform.localPosition.x, -3.9f, 0);
                break;
            case 6:
                num = 20;
                endpos = new Vector3(transform.localPosition.x, -2.3f, 0);
                break;
            case 7:
                num = 13;
                endpos = new Vector3(transform.localPosition.x, - 0.6f, 0);
                break;
            case 8:
                num = 11;
                endpos = new Vector3(transform.localPosition.x, 0.9f, 0);
                break;
            case 9:
                num = 5;
                endpos = new Vector3(transform.localPosition.x, 2.7f , 0);
                break;
        }

        for (int i = 0; i < num; i++)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - stepdistance * v, 0);

            if (i > Mathf.RoundToInt(num * 0.1f))
                timeInterval = 0.01f;
            if (i > Mathf.RoundToInt(num * 0.25f))
                timeInterval = 0.02f;
            if (i > Mathf.RoundToInt(num * 0.5f))
                timeInterval = 0.03f;
            if (i > Mathf.RoundToInt(num * 0.75f))
                timeInterval = 0.04f;

            yield return new WaitForSeconds(timeInterval);
        }

        transform.localPosition = endpos; //set endposition

        audioManager.StopMusic(audioManager.CasinoSpin);
        rowStopped = true;
    }
    
    public async Task WaitRotating()
    {
        await Task.Delay((8-rotatingNumber)*1000);
    }
}
