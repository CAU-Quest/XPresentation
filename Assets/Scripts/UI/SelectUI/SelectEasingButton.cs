using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEasingButton : MyButton
{
    [SerializeField] private AnimationPanel animationPanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private EasingButton[] buttons;
    
    private void Awake()
    {
        canvas.SetActive(false);
    }

    public override void OnSelect()
    {
        base.OnSelect();
        if (animationPanel.panelType == PanelType.Previous)
        {
            var xrAnimation = ((PresentationObject)animationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum - 1];
            foreach (var button in buttons)
            {
                button.xrAnimation = xrAnimation;
                if (xrAnimation.ease == button.ease) button.OnSelect();
            }
        }
        else if (animationPanel.panelType == PanelType.Next)
        {
            var xrAnimation = ((PresentationObject)animationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum];
            foreach (var button in buttons)
            {
                button.xrAnimation = xrAnimation;
                if (xrAnimation.ease == button.ease) button.OnSelect();
            }
        }
    }
}
