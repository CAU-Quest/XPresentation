using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGrabbable : ATypeToggleButton
{
    public override void InitializeProperty(PresentationObject selectedObject)
    {
        isOn = selectedObject.isGrabbableInPresentation;
        base.InitializeProperty(selectedObject);
    }
    
    public override void OnSelect()
    {
        base.OnSelect();
        
        SelectedObject.isGrabbableInPresentation = isOn;
        NewSlideObjectData = new SlideObjectData(CurrentSlideObjectData, isOn);
        WhenHasModification.Invoke(SelectedObject, NewSlideObjectData);
        CurrentSlideObjectData = NewSlideObjectData;
    }
}
