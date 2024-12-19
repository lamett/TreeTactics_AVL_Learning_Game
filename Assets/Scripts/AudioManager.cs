using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource loopSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource timerSource;
    [SerializeField] AudioSource enemySource;
    [SerializeField] AudioSource bingSource;
    [SerializeField] AudioSource bossMusicSource;


    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip EnemyWriting;
    public AudioClip EnemyTakesDamage;
    public AudioClip PlayerTakesDamage;
    public AudioClip OrbsMoving;
    public AudioClip CameraSound;
    public AudioClip OrbDestoryed;
    public AudioClip TimerSounds;
    public AudioClip TreeBalanced;
    public AudioClip CasinoSpin;
    public AudioClip JumpOnTable;
    public AudioClip EndBossMusic;




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

    public void StartEnemySpeak()
    {
        enemySource.clip = EnemyWriting;
        enemySource.Play();

    }

    public void StopEnemySpeak()
    {
        enemySource.clip = EnemyWriting;
        enemySource.Stop();

    }

    public void StartBossMusic()
    {
        musicSource.Stop();
        bossMusicSource.clip = EndBossMusic;
        bossMusicSource.Play();
    }

    public void PlayBing(AudioClip clip)
    {
        bingSource.PlayOneShot(clip);
    }
}
