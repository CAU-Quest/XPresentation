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
    [SerializeField] private BoundsClipper handleBoundClipper;
        
    private Material _selectedMaterial;
    private WaitForSeconds _waitForSeconds;
    private bool _isHandleHovered, _isReadyToCloseSlider;

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(0.2f);
        
        SetHandleValuePanel(false);
        SetGraduation(false);
        SetValuePanel(true);
        handle.gameObject.SetActive(false);
        sliderPanel.enabled = false;
        ResetHandleBounds();
        
        slider.onValueChanged.AddListener(OnValueChanged);
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
    }
    
    public override void OnHoverButton() //onHover
    {
        transform.SetAsLastSibling();
        base.OnHoverButton();

        sliderPanel.enabled = true;
        SetHoverColor();
        SetGraduation(true);
    }

    public void OnUnhoverSlider()
    {
        StartCoroutine(CloseSlider());
    }

    private IEnumerator CloseSlider(float time = 0.2f)
    {
        if(time != 0f) yield return _waitForSeconds;
        
        if (_isHandleHovered)
        {
            _isReadyToCloseSlider = true;
        }
        else
        {
            _isReadyToCloseSlider = false;
            sliderPanel.enabled = false;
            handle.gameObject.SetActive(false);
            SetDefaultColor();
            SetGraduation(true);
        }
    }

    public void OnHoverHandle()
    {
        _isHandleHovered = true;
    }

    public void OnUnhoverHandle()
    {
        _isHandleHovered = false;
        if (_isReadyToCloseSlider) StartCoroutine(CloseSlider(0f));
    }

    public void OnSelectHandle()
    {
        SetSelectColor();
        SetHandleBounds();
        
        DOKill();
        SetHandleValuePanel(true);
        SetValuePanel(false);
    }

    public void OnUnselectHandle()
    {
        SetHoverColor();
        ResetHandleBounds();
        
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

    private void ResetHandleBounds()
    {
        handleBoundClipper.Position = handle.localPosition;
        handleBoundClipper.Size = new Vector3(100f, 100f, 1f);
    }
    
    private void SetHandleBounds()
    {
        handleBoundClipper.Position = new Vector3();
        handleBoundClipper.Size = new Vector3(2000f, 500f, 1f);
    }
}
