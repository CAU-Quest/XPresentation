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
    [SerializeField] private RawImage graduationImageA, graduationImageB;
        
    private Material _selectedMaterial;
    
    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
        slider.gameObject.SetActive(false);
    }

    public override void InitProperty(PresentationObject selectedObject)
    {
        SelectedObject = selectedObject;
        _selectedMaterial = selectedObject.Material;
        
        SetHandleValuePanel(false);
        SetGraduation(false);
        SetValuePanel(true);
        handle.gameObject.SetActive(false);
        
        base.InitProperty(selectedObject);
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
    
    public override void OnHoverButton()
    {
        transform.SetAsLastSibling();
        base.OnHoverButton();
        
        slider.gameObject.SetActive(true);
        SetHoverColor();
        SetGraduation(true);
    }
    
    public void OnUnhoverSlider()
    {
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
    
    protected override void UpdateValue(float value) //value = 0~255
    {
        base.UpdateValue(value);
        value /= 255f;
        var color = _selectedMaterial.color;
        var newColor = color;
        
        switch (usage)
        {
            case Usage.ColorR:
                newColor = new Color(value, color.g, color.b, color.a);
                break;
            case Usage.ColorG:
                newColor = new Color(color.r, value, color.b, color.a);
                break;
            case Usage.ColorB:
                newColor = new Color(color.r, color.g, value, color.a);
                break;
            case Usage.ColorA:
                newColor = new Color(color.r, color.g, color.b, value);
                break;
        }
        _selectedMaterial.color = newColor;
        
        NewSlideObjectData = new SlideObjectData(CurrentSlideObjectData, newColor);
        WhenHasModification.Invoke(SelectedObject, NewSlideObjectData);
        CurrentSlideObjectData = NewSlideObjectData;
    }
    
    protected override void SetGraduation(bool isOn)
    {
        base.SetGraduation(isOn);
        graduationImageA.DOFade(isOn? 1f : 0f, 0.2f).SetEase(isOn ? Ease.OutCirc : Ease.InCirc);
        graduationImageB.DOFade(isOn? 1f : 0f, 0.2f).SetEase(isOn ? Ease.OutCirc : Ease.InCirc);
    }
}