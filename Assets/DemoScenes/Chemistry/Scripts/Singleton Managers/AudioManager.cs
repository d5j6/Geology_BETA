using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource source;

    public AudioSource AudioSourceSettings(AudioSource audioSource)
    {
        audioSource.playOnAwake = false;
        audioSource.dopplerLevel = 0.0f;
        audioSource.spatialBlend = 1.0f;
        audioSource.spatialize = true;
        audioSource.volume = 0.9f;
        AudioClip audioClip = null;
        audioClip = Resources.Load<AudioClip>("Button_Press");
        audioSource.clip = audioClip;

        return audioSource;
    }

    public void PlayAudio(AudioClip audioClip)
    {
        source.PlayOneShot(audioClip);
    }
}
