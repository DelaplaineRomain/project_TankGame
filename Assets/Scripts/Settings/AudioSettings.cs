using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettings : MonoBehaviour
{
    public GameObject AudioSlider;
    public GameObject AudioTextInput;

    public void UpdateTextValue()
    {
        float NewValue = AudioSlider.GetComponent<Slider>().value;
        AudioTextInput.GetComponent<TMP_InputField>().text = NewValue.ToString();
        ChangeAudioFactor(NewValue / 100f);
    }

    public void UpdateSlideValue()
    {
        string NewValue = AudioTextInput.GetComponent<TMP_InputField>().text;
        AudioSlider.GetComponent<Slider>().value = float.Parse(NewValue);
        ChangeAudioFactor(float.Parse(NewValue) / 100f);
    }

    private void ChangeAudioFactor(float NewAudioFactor)
    {
        GameManager.VolumeFactor = NewAudioFactor;
    }
}
