using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FartMeter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Slider _slider;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _fartIcon;

    [Header("Settings")]
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private Color _barNotFilledColor = Color.green;
    [SerializeField] private Color _barFilledColor = Color.red;

    private void Start()
    {
        _fartIcon.rectTransform.anchoredPosition = new Vector3(
        (_slider.GetComponentInParent<RectTransform>().rect.width / 100) * FartGameManager.instance.FartValue,
        0,
        0);
    }

    private void Update()
    {
        if (FartGameManager.instance.GameState == FartGameState.GameActive)
        {
            _slider.value = FartGameManager.instance.MeterValue;
            Debug.Log($"> {_slider.value}");
        }
    }

    private void UpdateBarColor(float value)
    {
        // Interpolate the color based on the slider value (0 to 1)
        Color lerpedColor = Color.Lerp(_barNotFilledColor, _barFilledColor, value);

        // Update the fill color of the health bar image
        _fill.color = lerpedColor;
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(UpdateBarColor);
        UpdateBarColor(0);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(UpdateBarColor);
    }
}
