using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{

    public int Health;

    public void reduceHealth()
    {
        Health -= 1;
    }

    public void setHealth(int value)
    {
        Health = value;
    }
}
