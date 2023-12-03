using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ATypeToggleButton : MonoBehaviour, ISelectedObjectModifier
{
    [SerializeField] protected Image buttonImage;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite whenOn, whenOff;

    protected bool isOn;
    
    public PresentationObject SelectedObject { get; set; }
    public SlideObjectData CurrentSlideObjectData { get; set; }
    public SlideObjectData NewSlideObjectData { get; set; }
    public Action<PresentationObject, SlideObjectData> WhenHasModification { get; set; }
    
    public virtual void InitializeProperty(PresentationObject selectedObject)
    {
        SelectedObject = selectedObject;
        buttonImage.DOColor((isOn) ? ColorManager.ToggleSelected : ColorManager.ToggleUnselected, 0.3f);
        ToggleSprite();
    }

    public virtual void FinalizeProperty() { }

    public void UpdateSelectedObjectData(PresentationObject selectedObject, SlideObjectData data)
    {
        selectedObject.ApplyDataToSlide(data);
    }

    public virtual void OnHover()
    {
        buttonImage.DOColor((isOn) ? ColorManager.ToggleSelectedHover : ColorManager.ToggleUnselectedHover, 0.3f);
    }

    public virtual void OnUnhover()
    {
        buttonImage.DOColor((isOn) ? ColorManager.ToggleSelected : ColorManager.ToggleUnselected, 0.3f);
    }
    
    public virtual void OnSelect()
    {
        buttonImage.DOColor(ColorManager.ToggleSelect, 0.3f);
        isOn = !isOn;
        ToggleSprite();
    }

    protected void ToggleSprite()
    {
        icon.sprite = (isOn) ? whenOn : whenOff;
    }
}
