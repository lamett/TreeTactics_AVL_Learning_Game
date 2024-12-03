using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHurtText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public int index;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                gameObject.SetActive(false);
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }

    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        textComponent.text = string.Empty;
        index = Random.Range(0, lines.Length); 
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

    }
}
