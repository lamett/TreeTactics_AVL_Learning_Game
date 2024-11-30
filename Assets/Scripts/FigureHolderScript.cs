using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FigureHolderScript : MonoBehaviour
{
    public GameObject Figur;
    
    List<GameObject> allFigures = new List<GameObject>();
    public void init(int num){
        for(int i = 0; i < num; i++){
            var figur = Instantiate(Figur, gameObject.transform);
            figur.transform.localPosition = new Vector3(0,0,i);
            allFigures.Add(figur);
        }
    }

    public void remove(){
        if(allFigures.Count < 1) return;
        var figur = allFigures.Last();
        Destroy(figur);
        allFigures.Remove(figur);
    }
}
