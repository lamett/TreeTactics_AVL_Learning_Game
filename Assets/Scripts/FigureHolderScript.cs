using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class FigureHolderScript : MonoBehaviour
{
    public GameObject Figur1;
    public GameObject Figur2;
    public GameObject Figur3;
    public GameObject Figur31;
    public ArmBehaviour Arm;

    List<GameObject> allFigures = new List<GameObject>();
    public void init(int num, bool isEnemy)
    {
        GameObject[] figure_arr = new GameObject[4];
        figure_arr[0] = Figur1;
        figure_arr[1] = Figur2;
        figure_arr[2] = Figur3;
        figure_arr[3] = Figur31;

        int m = 0;
        int i = 0;


        if (!isEnemy)
        {
            while (i < num)
            {
                int xm = 0;
                int ym = m;

                while (xm <= m && i < num)
                {
                    if (i != 3)
                    {
                        var figur = Instantiate(figure_arr[(i % figure_arr.Length)], gameObject.transform);
                        figur.transform.localPosition = new Vector3(ym * 3, 0, xm * 3);
                        figur.GetComponent<Rigidbody>().isKinematic = true;
                        allFigures.Add(figur);
                    }
                    else { num++; }
                    i++;
                    xm++;
                    ym--;
                }
                m++;
            }
        }


        else {
            for (int j = 0; j < num; j++)
            {
                var figur = Instantiate(figure_arr[(j % figure_arr.Length)], gameObject.transform);
                if (j > 3) { figur.transform.localPosition = new Vector3(2, 0, (j % 4) * 3); } //hardmode: 2 rows
                else { figur.transform.localPosition = new Vector3(0, 0, j * 3); }
                figur.GetComponent<Rigidbody>().isKinematic = true;
                allFigures.Add(figur);
            }
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
