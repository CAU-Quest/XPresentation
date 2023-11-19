using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface XRIAnimation
{
    public void Play();
    public SlideObjectData GetPreviousSlideObjectData();
    public SlideObjectData GetNextSlideObjectData();

    public void SetParentObject(PresentationObject presentationObject);

    public void SetPreviousSlideObjectData(SlideObjectData slideObjectData);
    public void SetNextSlideObjectData(SlideObjectData slideObjectData);
}
