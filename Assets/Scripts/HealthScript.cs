using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{

    public int Health;
    public EnemyHealthBar enemyHealthBar;
    public PlayerHealthBar playerHealthBar;

    public void reduceHealth()
    {
        Health -= 1;
        enemyHealthBar.DrawHearts();
        playerHealthBar.DrawHearts();
       
    }

    public void setHealth(int value)
    {
        Health = value;
    }

    public bool isDead()
    {
        if (Health <= 0)
        {
            return true;
        }
        return false;
    }
}
