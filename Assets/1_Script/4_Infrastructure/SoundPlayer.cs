using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    static SoundPlayer instance;

    AudioSource audioSource;
    readonly Dictionary<string, AudioClip> clipCache = new();

    static SoundPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SoundPlayer>();
                if (instance == null)
                {
                    var go = new GameObject(nameof(SoundPlayer));
                    instance = go.AddComponent<SoundPlayer>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public static void Play(string soundName, float volume = 1f)
    {
        if (string.IsNullOrEmpty(soundName)) return;
        Instance.PlayInternal(soundName, volume);
    }

    void PlayInternal(string soundName, float volume)
    {
        if (!clipCache.TryGetValue(soundName, out var clip))
        {
            clip = Resources.Load<AudioClip>($"Sounds/{soundName}");
            if (clip == null) return;
            clipCache[soundName] = clip;
        }

        audioSource.PlayOneShot(clip, volume);
    }
}