using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerResourceManagement : MonoBehaviour
{
    // Player stat
    public float StartingHealthPoint = 50f;
    private float HealthPoint;
    public int PlayerId;
    [HideInInspector]
    public bool ShieldActivated = false;
    public int ShieldTime = 5;

    // Player status
    public bool IsDead;

    // Graphics
    public Slider HealthSlider;
    public Image FillImage;             // The fill image component of the slider
    private Color FullHealthColor = Color.green;       
    private Color ZeroHealthColor = Color.red;

    private void Start()
    {
        this.gameObject.GetComponent<PlayerController>().PlayerId = PlayerId;
        this.gameObject.GetComponent<PlayerShootManagement>().PlayerId = PlayerId;
    }

    private void OnEnable()
    {
        HealthPoint = StartingHealthPoint;
        IsDead = false;

        // Set and Update UI element
        HealthSlider.maxValue = StartingHealthPoint;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        // Set the slider's value appropriately.
        HealthSlider.value = HealthPoint;

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        FillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, HealthPoint / StartingHealthPoint);
    }

    public void TakeDammage(float DammageNb)
    {
        if (!ShieldActivated)
        {
            HealthPoint -= DammageNb;    
        }

        // Refresh UI element
        UpdateHealthUI();

        if (HealthPoint <= 0f)
        {
            IsDead = true;
        }
    }

    public void ResetHealth ()
    {
        HealthPoint = StartingHealthPoint;
        UpdateHealthUI();
    }

    public void PU_TurnOnShield()
    {
        ShieldActivated = true;
        Invoke("PU_TurnOffShield", ShieldTime);
    }

    public void PU_TurnOffShield()
    {
        ShieldActivated = false;
    }

}
