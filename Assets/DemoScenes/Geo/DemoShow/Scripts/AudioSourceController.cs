using UnityEngine;
using HoloToolkit.Unity;

public class AudioSourceController : Singleton<AudioSourceController>
{
    AudioSource currentPlaying;
    float volume = 0;

    public void FollowGameObject(GameObject go)
    {
        transform.parent = go.transform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public void StartImmediately(GameObject go)
    {
        switch(LanguageManager.Instance.CurrentLanguage)
        {
            case Language.Russian:
                currentPlaying = go.GetComponents<AudioSource>()[0];
                break;
            case Language.English:
                currentPlaying = go.GetComponents<AudioSource>()[1];
                break;
        }
        currentPlaying.enabled = true;
        volume = 1;
        currentPlaying.volume = volume;
        currentPlaying.Play();
    }

    public void StopImmediately(GameObject go)
    {
        currentPlaying.Stop();
        currentPlaying.enabled = false;
    }

    public void StartPlaying(GameObject go, System.Action callback = null)
    {
        switch (LanguageManager.Instance.CurrentLanguage)
        {
            case Language.Russian:
                currentPlaying = go.GetComponents<AudioSource>()[0];
                break;
            case Language.English:
                currentPlaying = go.GetComponents<AudioSource>()[1];
                break;
        }
        currentPlaying.enabled = true;
        currentPlaying.Play();
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, volume, 1, 0.2f).setOnUpdate((float val) =>
        {
            volume = val;
            currentPlaying.volume = volume;
        }).setOnComplete(() =>
        {
            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }

    public void StopPlaying(System.Action callback = null)
    {
        LeanTween.cancel(gameObject);
        if (currentPlaying != null)
        {
            LeanTween.value(gameObject, volume, 0, 0.3f).setOnUpdate((float val) =>
            {
                volume = val;
                currentPlaying.volume = volume;
            }).setOnComplete(() =>
            {
                currentPlaying.Stop();
                currentPlaying.enabled = false;
                if (callback != null)
                {
                    callback.Invoke();
                }
            });
        }
        else
        {
            callback.Invoke();
        }
    }
}
