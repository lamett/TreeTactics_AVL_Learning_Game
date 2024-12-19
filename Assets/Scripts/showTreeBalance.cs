using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class showTreeBalance : MonoBehaviour
{
    public GameObject[] leds;
    public Material[] materials;
    public Material standardMaterial;
    AudioManager audioManager;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        foreach (var led in leds)
        {
            led.GetComponent<Outline>().enabled = false;
        }
    }

    void Start()
    {
        foreach (var led in leds)
        {
            led.transform.GetChild(0).gameObject.SetActive(false);
            led.GetComponent<Renderer>().material = standardMaterial;
        }
    }

    public void updateTreeBalance(int balanceFactor)
    {
        var balance = Mathf.Clamp(balanceFactor, 0, 20);
        bool light;
        Material material;
        if (balance >= 0)
        {
            light = true;
            material = materials[0];
        }
        else
        {
            
            light = false;
            material = standardMaterial;
        }
        leds[0].transform.GetChild(0).gameObject.SetActive(light);
        leds[0].GetComponent<Renderer>().material = material;
        if (balance >= 1)
        {
            light = true;
            material = materials[1];
        }
        else
        {
            //audioManager.PlayBing(audioManager.TreeBalanced);
            light = false;
            material = standardMaterial;
        }
        leds[1].transform.GetChild(0).gameObject.SetActive(light);
        leds[1].GetComponent<Renderer>().material = material;
        if (balance >= 3)
        {
            light = true;
            material = materials[2];
        }
        else
        {
            light = false;
            material = standardMaterial;
        }
        leds[2].transform.GetChild(0).gameObject.SetActive(light);
        leds[2].GetComponent<Renderer>().material = material;
        if (balance >= 8)
        {
            light = true;
            material = materials[3];
        }
        else
        {
            light = false;
            material = standardMaterial;
        }
        leds[3].transform.GetChild(0).gameObject.SetActive(light);
        leds[3].GetComponent<Renderer>().material = material;
        if (balance >= 15)
        {
            light = true;
            material = materials[4];
        }
        else
        {
            light = false;
            material = standardMaterial;
        }
        leds[4].transform.GetChild(0).gameObject.SetActive(light);
        leds[4].GetComponent<Renderer>().material = material;
    }

    public List<GameObject> LEDs()
    {
        return leds.ToList();
    }
}
