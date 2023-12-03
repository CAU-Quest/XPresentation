using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimationPlayButton : ATypeToggleButton
{
    [SerializeField] private AnimationPanel animationPanel;
    
    public override void OnSelect()
    {
        base.OnSelect();
        
        if (animationPanel.panelType == PanelType.Previous)
        {
            var xrAnimation = ((PresentationObject)animationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum - 1];
            
            var prevTrans = XRSelector.Instance.beforeAnimationGhost.transform;
            var prevMat = prevTrans.GetComponentInChildren<MeshRenderer>().material;

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
        else if (animationPanel.panelType == PanelType.Next)
        {
            var xrAnimation = ((PresentationObject)animationPanel.selectedObject).animationList[
                MainSystem.Instance.currentSlideNum];

            var prevTrans = ((PresentationObject)animationPanel.selectedObject).transform;
            var prevMat = prevTrans.GetComponentInChildren<MeshRenderer>().material;

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
    }

    public override void FinalizeProperty()
    {
        if(isOn) OnSelect();
    }
}
