using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToggleAnimationPlayButton : ATypeToggleButton
{
    [SerializeField] private AnimationPanel myAnimationPanel;
    [SerializeField] private AnimationPanel[] otherAnimationPanels;
    [SerializeField] private GameObject cannotUsePanel;
    private bool _canUse;
    [SerializeField] private ToggleAnimationPlayButton otherToggle;

    public void SetUsable(bool isUsable)
    {
        _canUse = isUsable;
        cannotUsePanel.SetActive(!isUsable);
    }

    public override void OnHover()
    {
        if(!_canUse) return;
        base.OnHover();
    }

    public override void OnUnhover()
    {
        if(!_canUse) return;
        base.OnUnhover();
    }
    
    public override void OnSelect()
    {
        if(!_canUse) return;
        base.OnSelect();
        
        if (myAnimationPanel.panelType == PanelType.Previous)
        {
            var xrAnimation = ((PresentationObject)myAnimationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum - 1];
            
            var prevTrans = XRSelector.Instance.beforeAnimationGhost.transform;
            var prevMat = XRSelector.Instance.beforeAnimationGhost.meshRenderer.material;

            if (isOn)
            {
                xrAnimation.PlayPreview(prevTrans, prevMat);
                XRSelector.Instance.DeactivateBoundBox();
            }
            else
            {
                xrAnimation.StopPreview();
                XRSelector.Instance.ActivateBoundBox();
            }
        }
        else if (myAnimationPanel.panelType == PanelType.Next)
        {
            var xrAnimation = ((PresentationObject)myAnimationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum];

            var prevTrans = ((PresentationObject)myAnimationPanel.selectedObject).transform.parent;
            var prevMat = ((PresentationObject)myAnimationPanel.selectedObject).Material;

            if (isOn)
            {
                xrAnimation.PlayPreview(prevTrans, prevMat);
                XRSelector.Instance.DeactivateBoundBox();
            }
            else
            {
                xrAnimation.StopPreview();
                XRSelector.Instance.ActivateBoundBox();
            }
        }
        
        
        for (int i = 0; i < myAnimationPanel.buttons.Length; i++)
        {
            myAnimationPanel.cannotUsePanel.SetActive(isOn);
            myAnimationPanel.buttons[i].canUse = !isOn;
        }

        foreach (var animationPanel in otherAnimationPanels)
        {
            for (int i = 0; i < animationPanel.buttons.Length; i++)
            {
                myAnimationPanel.cannotUsePanel.SetActive(isOn);
                animationPanel.buttons[i].canUse = !isOn;
            }
        }
        
        otherToggle.SetUsable(!isOn);
    }

    public override void FinalizeProperty()
    {
        if(isOn) OnSelect();
    }
}
