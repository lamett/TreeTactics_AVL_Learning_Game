
using UnityEngine;
using TMPro;


public class RandomNumber : MonoBehaviour
{

    [SerializeField] int randomNumber;
    [SerializeField] TMP_Text displaynumber;

    public void NumberGeneration()
    {
        CreateRandomNumber();
    }

    private void CreateRandomNumber()
    {
        randomNumber = Random.Range(1, 7);
        displaynumber.text = randomNumber.ToString();
    }
}
