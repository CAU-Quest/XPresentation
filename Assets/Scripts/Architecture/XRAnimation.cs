using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class XRAnimation : XRIAnimation
{
    public PresentationObject presentationObject;
    private uint id;

    public TransformData previousTransform;
    public TransformData nextTransform;
    public AnimationCurve lerpFunction = AnimationCurve.Linear(0, 0, 1, 1);
    
    
    public void Play()
    {
        float t = lerpFunction.Evaluate(MainSystem.Instance.slideInterval);

        Vector3 newPosition = Vector3.LerpUnclamped(previousTransform.position, nextTransform.position, t);
        Quaternion newRotation = Quaternion.LerpUnclamped(previousTransform.rotation, nextTransform.rotation, t);
        Vector3 newScale = Vector3.LerpUnclamped(previousTransform.scale, nextTransform.scale, t);
        
        presentationObject.SetTransform(newPosition, newRotation, newScale);
    }

    public void SetParentObject(PresentationObject presentationObject)
    {
        this.presentationObject = presentationObject;
        this.id = presentationObject.GetID();
    }

    public void SetPreviousTransform(TransformData transformData)
    {
        this.previousTransform = transformData;
    }
    public void SetNextTransform(TransformData transformData)
    {
        this.nextTransform = transformData;
    }
    public TransformData GetPreviousTransformData()
    {
        return previousTransform;
    }
    public TransformData GetNextTransformData()
    {
        return previousTransform;
    }
}
