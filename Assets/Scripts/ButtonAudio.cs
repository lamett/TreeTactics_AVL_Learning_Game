using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    [SerializeField]
    string srcTag;

    AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GameObject.FindGameObjectWithTag(srcTag).GetComponent<AudioSource>();
        Debug.Log(audioSrc);
        
    }
    
    public void OnChanged()
    {
        audioSrc.Play();
    }
}
