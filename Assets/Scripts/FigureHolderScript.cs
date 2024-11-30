using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FigureHolderScript : MonoBehaviour
{
    public GameObject Figur;
    public ArmBehaviour Arm;

    List<GameObject> allFigures = new List<GameObject>();
    public void init(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var figur = Instantiate(Figur, gameObject.transform);
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
