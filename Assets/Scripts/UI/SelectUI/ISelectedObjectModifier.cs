using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectedObjectModifierInitializer
{
    void InitializeProperty(PresentationObject selectedObject);
    void FinalizeProperty();
}

public interface ISelectedObjectModifier : ISelectedObjectModifierInitializer
{
    PresentationObject SelectedObject { get; set; }
    SlideObjectData CurrentSlideObjectData { get; set; }
    SlideObjectData NewSlideObjectData { get; set; }
    Action<PresentationObject, SlideObjectData> WhenHasModification { get; set; }
    
    void UpdateSelectedObjectData(PresentationObject selectedObject, SlideObjectData data);
}
