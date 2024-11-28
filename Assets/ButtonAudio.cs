using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    [SerializeField]
    string srcTag;

    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GameObject.FindGameObjectWithTag(srcTag).GetComponent<AudioSource>();
        Debug.Log(audio);
        
    }
    
    public void OnChanged()
    {
        audio.Play();
    }
}
