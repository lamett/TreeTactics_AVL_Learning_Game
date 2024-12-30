using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TextBoxGeneric : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    string line = "";
    public float textSpeed;
    public bool finished = true;
    AudioManager audioManager;


    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        textComponent = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!textComponent.text.Equals(line))
            {
                StopAllCoroutines();
                textComponent.text = line;
                finished = true;
            }
        }
    }


    public async Task PrintOnScreen(string line, float waitForSeconds = 0f)
    {
        StopAllCoroutines();
        finished = false;
        this.line = line;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());

        while (!finished) await Task.Delay(100);
        audioManager.StopEnemySpeak();
        await Task.Delay((int)(waitForSeconds * 1000));
        return;
    }

    IEnumerator TypeLine()
    {
        audioManager.StartEnemySpeak();
        for (int i = 0; i < line.Count(); i++)
        {
            
            textComponent.text += line[i];
            yield return new WaitForSeconds(textSpeed);
        }
        audioManager.StopEnemySpeak();
        finished = true;
    }

}
