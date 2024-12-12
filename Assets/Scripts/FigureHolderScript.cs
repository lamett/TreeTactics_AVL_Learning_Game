using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FigureHolderScript : MonoBehaviour
{
    public GameObject Figur1;
    public GameObject Figur2;
    public GameObject Figur3;
    public ArmBehaviour Arm;

    List<GameObject> allFigures = new List<GameObject>();
    public void init(int num)
    {
        GameObject[] figure_arr = new GameObject[3];
        figure_arr[0] = Figur1;
        figure_arr[1] = Figur2;
        figure_arr[2] = Figur3;
        for (int i = 0; i < num; i++)
        {
            var figur = Instantiate(figure_arr[(i % 3)], gameObject.transform);
            figur.transform.localPosition = new Vector3(0, 0, i);
            figur.GetComponent<Rigidbody>().isKinematic = true;
            allFigures.Add(figur);
        }
    }

    public void remove()
    {
        if (allFigures.Count < 1) return;
        var figur = allFigures.Last();
        Arm.SnapFigure(figur);
        allFigures.Remove(figur);
    }
}
