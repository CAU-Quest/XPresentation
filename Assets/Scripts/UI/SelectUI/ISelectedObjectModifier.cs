using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectedObjectModifier
{
    PresentationObject SelectedObject { get; set; }
    SlideObjectData CurrentSlideObjectData { get; set; }
    SlideObjectData NewSlideObjectData { get; set; }
    Action<PresentationObject, SlideObjectData> WhenHasModification { get; set; }

    void InitProperty(PresentationObject selectedObject);
    void UpdateSelectedObjectData(PresentationObject selectedObject, SlideObjectData data);
}
