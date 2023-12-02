using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SelectEasingButton : BTypeButton, ISelectedObjectModifierInitializer
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private AnimationPanel animationPanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private EasingButton[] buttons;
    [SerializeField] private Ease selectedEase;

    private bool _isButtonsFirstInitialized = true;
    
    public void InitProperty(PresentationObject selectedObject)
    {
        if (MainSystem.Instance.currentSlideNum - 1 < 0 && animationPanel.panelType == PanelType.Previous ||
            MainSystem.Instance.currentSlideNum > MainSystem.Instance.GetSlideCount() && animationPanel.panelType == PanelType.Next) return;
        
        if (animationPanel.panelType == PanelType.Previous)
        {
            selectedEase = selectedObject.animationList[MainSystem.Instance.currentSlideNum - 1].ease;
        }
        else if (animationPanel.panelType == PanelType.Next)
        {
            selectedEase = selectedObject.animationList[MainSystem.Instance.currentSlideNum].ease;
        }

        UpdateText(selectedEase);
    }

    public void UpdateText(Ease ease)
    {
        text.text = (ease == Ease.Unset) ? "Linear" : Enum.GetName(typeof(Ease), ease);
    }

    public override void OnSelect()
    {
        base.OnSelect();
        canvas.SetActive(true);
        var xrAnimation = new XRAnimation();
        
        if (animationPanel.panelType == PanelType.Previous)
        {
            xrAnimation = ((PresentationObject)animationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum - 1];
        }
        else if (animationPanel.panelType == PanelType.Next)
        {
            xrAnimation = ((PresentationObject)animationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum];
        }
        
        foreach (var button in buttons)
        {
            button.InitButton(xrAnimation);
            if (_isButtonsFirstInitialized)
            {
                button.onSelect += UpdateText;
            }
        }
        if(_isButtonsFirstInitialized) _isButtonsFirstInitialized = false;
        animationPanel.transform.SetAsLastSibling();
    }

    public void OnCanvasClose()
    {
        canvas.SetActive(false);
        foreach (var button in buttons)
        {
            button.CloseButton();
        }
    }
}
