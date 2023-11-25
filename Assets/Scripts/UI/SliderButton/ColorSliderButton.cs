using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using UnityEngine;
using UnityEngine.UI;

public class ColorSliderButton : SliderButton
{
    private enum Usage
    {
        ColorR, ColorG, ColorB, ColorA
    };
    
    [SerializeField] private Usage usage;
    [SerializeField] private Slider slider;
    [SerializeField] private RayInteractable sliderPanel;
        
    private Material _selectedMaterial;
    
    private void Start()
    {
        SetHandleValuePanel(false);
        SetGraduation(false);
        SetValuePanel(true);
        handle.gameObject.SetActive(false);
        sliderPanel.enabled = false;

        slider.onValueChanged.AddListener(OnValueChanged);
        slider.gameObject.SetActive(false);
    }

    protected override void InitProperty()
    {
        _selectedMaterial = selectUI.selectedProperty.Material;
        base.InitProperty();
    }
    
    protected override void SetInitialValue()
    {
        switch (usage)
        {
            case Usage.ColorR:
                initialValue = _selectedMaterial.color.r;
                break;
            case Usage.ColorG:
                initialValue = _selectedMaterial.color.g;
                break;
            case Usage.ColorB:
                initialValue = _selectedMaterial.color.b;
                break;
            case Usage.ColorA:
                initialValue = _selectedMaterial.color.a;
                break;
        }

        initialValue *= 255f;
    }
    
    public override void OnHoverButton() //onHover
    {
        transform.SetAsLastSibling();
        base.OnHoverButton();

        sliderPanel.enabled = true;
        slider.gameObject.SetActive(true);
        SetHoverColor();
        SetGraduation(true);
    }

    public void OnUnhoverSlider()
    {
        sliderPanel.enabled = false;
        handle.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        SetDefaultColor();
        SetGraduation(false);
    }

    public void OnSelectSlider()
    {
        SetSelectColor();
        
        DOKill();
        SetHandleValuePanel(true);
        SetValuePanel(false);
    }

    public void OnUnselectSlider()
    {
        SetHoverColor();
        
        DOKill();
        SetHandleValuePanel(false);
        SetValuePanel(true);
    }

    private void OnValueChanged(float value)
    {
        UpdateValue(value);
    }
    
    protected override void UpdateValue(float value) //value = -500 ~ 500
    {
        var color = _selectedMaterial.color;
        
        switch (usage)
        {
            case Usage.ColorR:
                _selectedMaterial.color = new Color(value, color.g, color.b, color.a);
                break;
            case Usage.ColorG:
                _selectedMaterial.color = new Color(color.r, value, color.b, color.a);
                break;
            case Usage.ColorB:
                _selectedMaterial.color = new Color(color.r, color.g, value, color.a);
                break;
            case Usage.ColorA:
                _selectedMaterial.color = new Color(color.r, color.g, color.b, value);
                break;
        }

        value *= 255f;
        base.UpdateValue(value);
    }
}