using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class XRAnimation : XRIAnimation
{
    public PresentationObject presentationObject;
    private uint id;

    public SlideObjectData previousData;
    public SlideObjectData nextData;
    public AnimationCurve lerpFunction = AnimationCurve.Linear(0, 0, 1, 1);
    
    
    public void Play()
    {
        float t = lerpFunction.Evaluate(MainSystem.Instance.slideInterval);

        SlideObjectData lerpData = new SlideObjectData();
        
        lerpData.position = Vector3.LerpUnclamped(previousData.position, nextData.position, t);
        lerpData.rotation = Quaternion.LerpUnclamped(previousData.rotation, nextData.rotation, t);
        lerpData.scale = Vector3.LerpUnclamped(previousData.scale, nextData.scale, t);
        lerpData.color = Color.LerpUnclamped(previousData.color, nextData.color, t);
        
        presentationObject.ApplySlideObjectData(lerpData);
    }

    public void SetParentObject(PresentationObject presentationObject)
    {
        this.presentationObject = presentationObject;
        this.id = presentationObject.GetID();
    }

    public void SetPreviousSlideObjectData(SlideObjectData slideObjectData)
    {
        this.previousData = slideObjectData;
    }
    public void SetNextSlideObjectData(SlideObjectData slideObjectData)
    {
        this.nextData = slideObjectData;
    }
    public SlideObjectData GetPreviousSlideObjectData()
    {
        return previousData;
    }
    public SlideObjectData GetNextSlideObjectData()
    {
        return nextData;
    }
}
