using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_Text finishedText;

    [SerializeField] Button setButton;
    [SerializeField] Button raceButton;

    [SerializeField] Slider speedSlider;
    [SerializeField] TMP_Text speedText;

    [SerializeField] List<TMP_Text> stepsText;

    void Update()
    {
        speedText.text = $"Velocidad (1 - 3): {GetSpeed()}";
    }

    public float GetSpeed()
    {
        return speedSlider.value;
    }

    public void UpdateStepsText(int index, float value)
    {
        if ((uint)index >= (uint)stepsText.Count || stepsText[index] == null) return;

        stepsText[index].text = Mathf.RoundToInt(value).ToString();
    }

    public void EnableSetButton(bool value)
    {
        setButton.interactable = value;
    }

    public void EnableRaceUI(bool value)
    {
        raceButton.interactable = value;
        speedSlider.interactable = value;
    }

    public void Racing(bool value, float alpha)
    {
        raceButton.interactable = value;
        finishedText.color = new Color(finishedText.color.r, finishedText.color.g, finishedText.color.b, alpha);
    }
}