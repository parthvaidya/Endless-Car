using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource engineSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip brakeClip;
    [SerializeField] private AudioClip engineIdleClip;
    [SerializeField] private AudioClip engineRunningClip;
    [SerializeField] private AudioClip carCrash;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayEngineIdle();
    }

    // --- Public Sound Triggers ---

    public void PlayCoin()
    {
        PlaySFX(coinClip);
    }

    public void PlayBrake()
    {
        PlaySFX(brakeClip);
    }

    public void PlayEngineIdle()
    {
        engineSource.loop = true;
        engineSource.clip = engineIdleClip;
        engineSource.Play();
    }

    public void PlayEngineRunning()
    {
        if (engineSource.clip == engineRunningClip) return;

        engineSource.loop = true;
        engineSource.clip = engineRunningClip;
        engineSource.Play();
    }

    public void StopEngine()
    {
        engineSource.Stop();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayCarCrash()
    {
        PlaySFX(carCrash);
    }

    public void PlayCarDeath()
    {
        PlaySFX(deathSound);
    }
}
