using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSlideButton : ATypeToggleButton
{
    [HideInInspector] public PresentationObject presentationObject;
    [HideInInspector] public int currentSlideNumber;
    public SelectEasingButton selectEasingButton;
    public ToggleAnimationPlayButton toggleAnimationPlayButton;

    public override void OnSelect()
    {
        base.OnSelect();
        
        var so = presentationObject.GetSlideObjectDataByIndex(currentSlideNumber);
        so.isVisible = isOn;
        presentationObject.ApplyDataToSlideWithIndex(so, currentSlideNumber);
        XRSelector.Instance.NotifySlideObjectDataChangeToObservers();

        if (isOn && selectEasingButton) selectEasingButton.InitializeProperty(presentationObject);
        if(toggleAnimationPlayButton) toggleAnimationPlayButton.SetUsable(isOn);
    }

    public void SetActive(bool isActive)
    {
        isOn = isActive;
        ToggleSprite();
        OnUnhover();
        
        if (isOn && selectEasingButton) selectEasingButton.InitializeProperty(presentationObject);
        if(toggleAnimationPlayButton) toggleAnimationPlayButton.SetUsable(isOn);
    }
}
