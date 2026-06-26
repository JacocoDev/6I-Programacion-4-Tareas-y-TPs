using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Referencias Principal")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject sliderGroup;

    [Header("Referencias de Datos")]
    [SerializeField] private VolumeData musicData;
    [SerializeField] private VolumeData sfxData;

    [Header("Referencias UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        ConfigureSliders();
    }

    private void ConfigureSliders()
    {
        musicSlider.minValue = 0f;
        musicSlider.maxValue = 10f;
        musicSlider.value = musicData.volume;
        musicSlider.onValueChanged.AddListener(UpdateMusic);

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 10f;
        sfxSlider.value = sfxData.volume;
        sfxSlider.onValueChanged.AddListener(UpdateSFX);
    }

    public void ToggleSliderGroup()
    {
        audioManager.PlayUIClick();
        bool isActive = sliderGroup.activeSelf;
        sliderGroup.SetActive(!isActive);
    }

    private void UpdateMusic(float value)
    {
        audioManager.SetMusicVolume(value);
    }

    private void UpdateSFX(float value)
    {
        audioManager.SetSFXVolume(value);
    }

    public void PlaySliderClick()
    {
        audioManager.PlayUIClick();
    }
}