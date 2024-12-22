using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public int index;
    AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        textComponent = gameObject.GetComponent<TextMeshProUGUI>();
    }


    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
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
                audioManager.StopEnemySpeak();
            }
        }

    }

    public void StartDialogue()
    {
        if (gameObject == null) { return; };
        gameObject.SetActive(true);

        textComponent.text = string.Empty;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        audioManager.StartEnemySpeak();
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioManager.StopEnemySpeak();
    }

}