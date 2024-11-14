using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{   
    public HealthBarEnemy healthBarEnemy;
    public HealthBarPlayer healthBarPlayer;

    public int Health;

    public void reduceHealth()
    {
        Health -= 1;
        healthBarPlayer.DrawHearts();
        healthBarEnemy.DrawHearts();

    }

    public void setHealth(int value)
    {
        Health = value;
    }
}
