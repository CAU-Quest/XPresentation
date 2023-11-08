using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface XRIAnimation
{
    public void Play();
    public TransformData GetPreviousTransformData();
    public TransformData GetNextTransformData();

    public void SetParentObject(PresentationObject presentationObject);

    public void SetPreviousTransform(TransformData transformData);
    public void SetNextTransform(TransformData transformData);
}
