using UnityEngine;

public class HealthScript : MonoBehaviour
{

    public int Health;
    public bool isEnemy;
    public FigureHolderScript FigureHolder;
    public ArmBehaviour Arm;

    public void reduceHealth()
    {
        Health -= 1;
        FigureHolder.remove();
    }

    public void setHealth(int value)
    {
        Health = value;
        FigureHolder.init(value,isEnemy);
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
