using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPhaseController : MonoBehaviour
{

    public int ballCount {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        ballCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call when Switching into Addphase to reset the ballCount to the amount of balls that the player has to add 
    public void setBallCount(int ballsToAdd){
        ballCount = ballsToAdd;
    }

    public void decreaseBallCount(){
        ballCount --;
    }
}
