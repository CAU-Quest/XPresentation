using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour, ISelectedObjectModifier
{
    [SerializeField] protected float maxOffset = 1f;
    [SerializeField] protected Transform handle;
    
    [SerializeField] private Image buttonImage, handleImage, graduationImage, graduationShadowImage, valuePanelImage, handleValuePanelImage;
    [SerializeField] private TextMeshProUGUI valueText, handleValueText;
    
    protected float initialValue;
    protected bool turnOffWhenUnselect, isSelectingHandle;
    protected Vector3 initialHandleLocalPos;

    public PresentationObject SelectedObject { get; set; }
    public SlideObjectData CurrentSlideObjectData { get; set; }
    public SlideObjectData NewSlideObjectData { get; set; }
    public Action<PresentationObject, SlideObjectData> WhenHasModification { get; set; }

    public virtual void InitProperty(PresentationObject selectedObject)
    {
        SetInitialValue();
        CurrentSlideObjectData = selectedObject.GetCurrentSlideObjectData();
        valueText.text = initialValue.ToString("0.0");
        handleValueText.text = initialValue.ToString("0.0");
    }

    public void UpdateSelectedObjectData(PresentationObject selectedObject, SlideObjectData data)
    {
        selectedObject.ApplyDataToSlide(data);
    }

    protected virtual void SetInitialValue() { }

    private void Start()
    {
        initialHandleLocalPos = handle.localPosition;
        OnUnselectButton();
        OnUnhoverButton();
    }

    protected virtual void UpdateValue(float value)
    {
        valueText.text = value.ToString("0.0");
        handleValueText.text = value.ToString("0.0");
    }

    public virtual void OnHoverButton()
    {
        handle.gameObject.SetActive(true);
    }
    
    public virtual void OnUnhoverButton()
    {
        if (isSelectingHandle) turnOffWhenUnselect = true;
        else handle.gameObject.SetActive(false);
    }
    
    public virtual void OnSelectButton()
    {
        XRSelector.Instance.DeactivateBoundBox();
        transform.SetAsLastSibling();
        SetInitialValue();
        isSelectingHandle = true;
    }

    public virtual void OnUnselectButton()
    {
        XRSelector.Instance.Reselect();
        isSelectingHandle = false;
        if (turnOffWhenUnselect)
        {
            handle.gameObject.SetActive(false);
            turnOffWhenUnselect = false;
        }
    }
    
    protected void DOKill()
    {
        graduationImage.DOKill();
        graduationShadowImage.DOKill();
        handleValuePanelImage.DOKill();
        handleValueText.DOKill();
        valuePanelImage.DOKill();
        valueText.DOKill();
    }

    protected virtual void SetGraduation(bool isOn)
    {
        if(graduationImage) graduationImage.DOFade(isOn? 1f : 0f, 0.2f).SetEase(isOn ? Ease.OutCirc : Ease.InCirc);
        graduationShadowImage.DOFade(isOn? 1f : 0f, 0.2f).SetEase(isOn ? Ease.OutCirc : Ease.InCirc);
    }
    
    protected void SetValuePanel(bool isOn)
    {
        valuePanelImage.DOFade(isOn? 1f : 0f, 0.2f).SetEase(isOn ? Ease.OutCirc : Ease.InCirc);
        valueText.DOFade(isOn? 1f : 0f, 0.2f).SetEase(isOn ? Ease.OutCirc : Ease.InCirc);
    }
    
    protected void SetHandleValuePanel(bool isOn)
    {
        handleValuePanelImage.DOFade(isOn? 1f : 0f, 0.2f).SetEase(isOn ? Ease.OutCirc : Ease.InCirc);
        handleValueText.DOFade(isOn? 1f : 0f, 0.2f).SetEase(isOn ? Ease.OutCirc : Ease.InCirc);
    }

    protected void SetHoverColor()
    {
        buttonImage.DOColor(ColorManager.SliderHover, 0.2f);
        handleImage.DOColor(ColorManager.Default, 0.2f);
    }

    protected void SetSelectColor()
    {
        buttonImage.DOColor(ColorManager.SliderSelect, 0.2f);
        handleImage.DOColor(ColorManager.Select, 0.2f);
    }

    protected void SetDefaultColor()
    {
        buttonImage.DOColor(ColorManager.SliderDefault, 0.2f);
        handleImage.DOColor(ColorManager.Default, 0f);
    }
}