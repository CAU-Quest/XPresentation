using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonText;
    
    private SelectUI _selectUI;
    private bool _isOn;
    
    private void Awake()
    {
        _selectUI = GetComponentInParent<SelectUI>();
        _selectUI.initUI += InitProperty;
    }

    private void InitProperty()
    {
        _isOn = _selectUI.selectedProperty.grabbableInPresentation;
        buttonImage.DOColor((_isOn) ? ColorManager.ToggleSelected : ColorManager.ToggleUnselected, 0.3f);
        buttonText.text = (_isOn) ? "V" : "";
    }

    public void OnHover()
    {
        buttonImage.DOColor((_isOn) ? ColorManager.ToggleSelectedHover : ColorManager.ToggleUnselectedHover, 0.3f);
    }

    public void OnUnhover()
    {
        buttonImage.DOColor((_isOn) ? ColorManager.ToggleSelected : ColorManager.ToggleUnselected, 0.3f);
    }
    
    public void Onselect()
    {
        buttonImage.DOColor(ColorManager.ToggleSelect, 0.3f);
        _isOn = !_isOn;
        _selectUI.selectedProperty.grabbableInPresentation = _isOn;
        buttonText.text = (_isOn) ? "V" : "";
    }
}
