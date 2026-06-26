using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Referencias de Datos")]
    [SerializeField] private VolumeData musicData;
    [SerializeField] private VolumeData sfxData;

    [Header("Referencias de Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips de Sonido")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip punchClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip bellClip;
    [SerializeField] private AudioClip UIClickClip;

    private void Start()
    {
        ApplyInitialVolumes();
        Playmusic();
    }

    private void ApplyInitialVolumes()
    {
        musicSource.volume = musicData.volume / 10f;
        sfxSource.volume = sfxData.volume / 10f;
    }

    public void SetMusicVolume(float value)
    {
        musicData.volume = value;
        musicSource.volume = value / 10f;
    }

    public void SetSFXVolume(float value)
    {
        sfxData.volume = value;
        sfxSource.volume = value / 10f;
    }

    private void Playmusic()
    {
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayPunch()
    {
        sfxSource.PlayOneShot(punchClip, sfxData.volume / 10f);
    }

    public void PlayHit()
    {
        sfxSource.PlayOneShot(hitClip, sfxData.volume / 10f);
    }

    public void PlayBell()
    {
        sfxSource.PlayOneShot(bellClip, sfxData.volume / 10f);
    }

    public void PlayUIClick()
    {
        sfxSource.PlayOneShot(UIClickClip, sfxData.volume / 10f);
    }
}