using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEasingButton : MyButton
{
    [SerializeField] private AnimationPanel animationPanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private EasingButton[] buttons;

    public override void OnSelect()
    {
        base.OnSelect();
        canvas.SetActive(true);
        if (animationPanel.panelType == PanelType.Previous)
        {
            var xrAnimation = ((PresentationObject)animationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum - 1];
            foreach (var button in buttons)
            {
                button.InitButton(xrAnimation);
            }
        }
        else if (animationPanel.panelType == PanelType.Next)
        {
            var xrAnimation = ((PresentationObject)animationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum];
            foreach (var button in buttons)
            {
                button.InitButton(xrAnimation);
            }
        }
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
