using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface XRIAnimation
{
    public void Play();
    public void CollectTransformFromSlide();
    public TransformData GetPreviousTransformData();
    public TransformData GetNextTransformData();

    public void SetSlide(ISlide beforeSlide, ISlide afterSlide);
    public void SetParentObject(PresentationObject presentationObject);

    public void SetPreviousTransform(TransformData transformData);
    public void SetNextTransform(TransformData transformData);
}
