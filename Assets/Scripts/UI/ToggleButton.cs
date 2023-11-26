using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour, ISelectedObjectModifier
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonText;

    private bool _isOn;
    private SelectUI _selectUI;

    public void InitProperty(SelectUI selectUI)
    {
        _selectUI = selectUI;
        
        _isOn = selectUI.selectedObject.isGrabbableInPresentation;
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
        _selectUI.selectedObject.isGrabbableInPresentation = _isOn;
        buttonText.text = (_isOn) ? "V" : "";
    }

    public PresentationObject SelectedObject { get; set; }
    public SlideObjectData CurrentSlideObjectData { get; set; }
    public SlideObjectData NewSlideObjectData { get; set; }
    public Action<PresentationObject, SlideObjectData> WhenHasModification { get; set; }
    public void InitProperty(PresentationObject selectedObject)
    {
        throw new NotImplementedException();
    }

    public void UpdateSelectedObjectData(PresentationObject selectedObject, SlideObjectData data)
    {
        throw new NotImplementedException();
    }
}
