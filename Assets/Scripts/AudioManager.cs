
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource loopSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource timerSource;


    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip EnemyWriting;
    public AudioClip EnemyTakesDamage;
    public AudioClip PlayerTakesDamage;
    public AudioClip OrbsMoving;
    public AudioClip OrbsFallInBowl;
    public AudioClip OrbDestoryed;
    public AudioClip TimerSounds;
    public AudioClip PickFalseOrb;
    public AudioClip TreeBalanced;
    public AudioClip CasinoSpin;
    public AudioClip JumpOnTable;
    public AudioClip WaterTap;
    public AudioClip Rustle;
    public AudioClip CameraMoving;




    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void StartMusic(AudioClip clip)
    {
        loopSource.clip = clip;
        loopSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopMusic(AudioClip clip)
    {
        loopSource.clip = clip;
        loopSource.Stop();
    }
    public void StartTimer()
    {
        timerSource.clip = TimerSounds;
        timerSource.Play();
    }
    public void StopTimer()
    {
        timerSource.clip = TimerSounds;
        timerSource.Stop();
    }
}
