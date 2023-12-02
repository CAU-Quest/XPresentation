using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGrabbable : ToggleButton
{
    public override void InitProperty(PresentationObject selectedObject)
    {
        isOn = selectedObject.isGrabbableInPresentation;
        base.InitProperty(selectedObject);
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
