using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

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
    [SerializeField] AudioSource WinMusicSource;


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
    public AudioClip WinMusic;
    public AudioClip SFXButton;
    public AudioClip PlatineButton;




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
    public void StopBossMusic()
    {
        bossMusicSource.Stop();
    }

    public void PlayBing(AudioClip clip)
    {
        if(clip == null) {return;};
        Debug.Log("ArgumentNullException Debuging: Clip ="+clip.ToString());
        bingSource.PlayOneShot(clip);
    }

    public void PlayWinMusic() { WinMusicSource.Play(); }
  

    public void FadeInWinMusic()
    {
        Debug.Log("FadeINMusic");
        WinMusicSource.Play();
        WinMusicSource.volume = 0;
        StartCoroutine(Fade(true, WinMusicSource, 2f, 0.3f));
    }

    public IEnumerator Fade(bool fadeIn, AudioSource source, float duration, float targetVolume)
    {
        if (!fadeIn)
        {
            double lengthOfSource = (double)source.clip.samples / source.clip.frequency;
            yield return new WaitForSecondsRealtime((float)(lengthOfSource - duration));

        }

        float time = 0f;
        float startVol = source.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, targetVolume, time / duration);
            yield return null;

        }
        yield break;

    }

}
