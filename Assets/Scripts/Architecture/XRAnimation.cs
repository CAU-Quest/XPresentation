using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class XRAnimation : XRIAnimation
{
    public PresentationObject presentationObject;
    private uint id;

    public SlideObjectData previousData;
    public SlideObjectData nextData;
    public Ease ease = Ease.Linear;
    
    public void Play()
    {
        float t = DOVirtual.EasedValue(0f, 1f, MainSystem.Instance.slideInterval, ease);

        SlideObjectData lerpData = new SlideObjectData();
        
        lerpData.position = Vector3.LerpUnclamped(previousData.position, nextData.position, t);
        lerpData.rotation = Quaternion.LerpUnclamped(previousData.rotation, nextData.rotation, t);
        lerpData.scale = Vector3.LerpUnclamped(previousData.scale, nextData.scale, t);
        lerpData.color = Color.LerpUnclamped(previousData.color, nextData.color, t);
        
        presentationObject.ApplyDataToObject(lerpData);
    }

    public void SetEase(Ease newEase)
    {
        ease = newEase;
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
