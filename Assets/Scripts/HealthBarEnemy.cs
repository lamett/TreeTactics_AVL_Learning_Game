using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBarEnemy : MonoBehaviour
{
    
    public GameObject hearthPrefab;
    public float health, maxhealth;
    List<HealthHeart> hearts = new List<HealthHeart>();


    private void Start()
    {
        DrawHearts();
    }


    public void DrawHearts()
    {
        ClearHeart();

        int heartsToMake =(int) maxhealth;
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();

        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int) Mathf.Clamp(health - (i * 1), 0, 1);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }   


    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(hearthPrefab);
        newHeart.transform.SetParent(transform);

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
        health -= 1;
        DrawHearts();
       }

        public void GainHealth()
        {
        health += 1;
        DrawHearts() ;  
        }


}
