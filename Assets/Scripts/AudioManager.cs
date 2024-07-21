using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get;private set;}
    [SerializeField] private AudioClip [] deathLines = new AudioClip[8];
    [SerializeField] private AudioClip [] voiceLines;
    [SerializeField] private AudioClip gunShotSfx;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioSource audioPrefab;
    [SerializeField] private AudioSource audioBackgroundPrefab;
    [SerializeField] [Range(0,1)] private float volume;
    [SerializeField] [Range(0,1)] private float backgroundVolume;

    private int voiceIdx;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }

    public void PlayAudio(AudioClip audioClip, Transform spawnTransform)
    {   
        AudioSource audio = Instantiate(audioPrefab, spawnTransform.position, Quaternion.identity);
        audio.clip = audioClip;
        audio.volume = volume;
        audio.Play();
        float clipLength = audio.clip.length;
        Destroy(audio.gameObject, clipLength);
    }

    public void PlayDeathAudio(Transform audioSpawn)
    {
        PlayAudio(deathLines[Random.Range(0,deathLines.Length)], audioSpawn);
    }

    public void PlayGunShot(Transform audioSpawn)
    {
        PlayAudio(gunShotSfx, audioSpawn);
    }

    public void PlayBackgroundMusic()
    {
        AudioSource audio = Instantiate(audioBackgroundPrefab, transform.position, Quaternion.identity);
        audio.clip = backgroundMusic;
        audio.Play();
        StartCoroutine(FadeIn(audio,3f));
    }

    public IEnumerator FadeIn(AudioSource audio, float duration)
    {
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            audio.volume = backgroundVolume * (timer/duration);
            yield return null;
        }
    }

    public void PlayVoiceLine(int idx)
    {
        PlayAudio(voiceLines[idx], transform);
    }

    public float GetCurrentLineLength()
    {
        return voiceLines[voiceIdx].length;
    }
}
