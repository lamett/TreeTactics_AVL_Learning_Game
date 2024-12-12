using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
    static List<Outline> pulsingObjects = new();

    public static void StopPulsing()
    {
        pulsingObjects.Clear();
    }
    public static void StopPulsing(GameObject gO)
    {
        pulsingObjects.Remove(gO.GetComponent<Outline>());
    }
    public static void StartPulsing(List<GameObject> gameObjects)
    {
        foreach (var gO in gameObjects)
        {
            StartPulsing(gO);
        }
    }
    public static void StartPulsing(GameObject gO)
    {
        var outline = gO.GetComponent<Outline>();
        pulsingObjects.Add(outline);
        Pulsing(outline);
    }

    static async void Pulsing(Outline outline)
    {
        outline.OutlineWidth = 5;
        while (pulsingObjects.Contains(outline))
        {
            outline.enabled = true;
            await Task.Delay(500);
            outline.enabled = false;
            await Task.Delay(500);
        }
    }
}
