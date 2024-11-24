using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public GameController controller;
    public HealthScript healthScript;
    public GameObject heartPrefab;
    List<HealthHeart> hearts = new List<HealthHeart>();


    private void Start()
    {
        DrawHearts();
    }


    public void DrawHearts()
    {
        ClearHeart();

        int heartsToMake = (int)controller.enemyStartHealth;
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();

        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(healthScript.Health - (i * 1), 0, 1);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }


    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform, false);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HeartStatus.empty);
        hearts.Add(heartComponent);

    }

    public void ClearHeart()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    public void LoseHeatlh()
    {
        healthScript.Health -= 1;
        DrawHearts();
    }

    public void GainHealth()
    {
        healthScript.Health += 1;
        DrawHearts();
    }


}