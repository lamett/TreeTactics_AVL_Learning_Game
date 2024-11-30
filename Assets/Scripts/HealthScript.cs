using UnityEngine;

public class HealthScript : MonoBehaviour
{

    public int Health;
    public FigureHolderScript FigureHolder;

    public void reduceHealth()
    {
        Health -= 1;
        FigureHolder.remove();
    }

    public void setHealth(int value)
    {
        Health = value;
        FigureHolder.init(value);
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
