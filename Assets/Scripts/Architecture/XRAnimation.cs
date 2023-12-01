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
    private Sequence _sequence;

    private Transform _previousTransform;
    private Material _previousMaterial;

    public XRAnimation() 
    {
        _sequence = DOTween.Sequence().SetAutoKill(false)
        .OnStart(() =>
        {
            _previousTransform.position = previousData.position;
            _previousTransform.rotation = previousData.rotation;
            _previousTransform.localScale = previousData.localScale;
            _previousMaterial.color = previousData.color;
        })
        .Append(_previousTransform.DOMove(nextData.position, 1f).SetEase(ease))
        .Join(_previousTransform.DORotate(nextData.rotation, 1f).SetEase(ease))
        .Join(_previousTransform.DOLocalScale(nextData.localScale, 1f).SetEase(ease))
        .Join(_previousMaterial.DOColor(nextData.color, 1f).SetEase(ease))
        .AppendInterval(0.7f)
        .SetLoops(-1, LoopType.Restart);
    }

    public void Play()
    {
        float t = DOVirtual.EasedValue(0f, 1f, MainSystem.Instance.slideInterval, ease);
        Debug.Log("Playing t : " + t);

        SlideObjectData lerpData = new SlideObjectData();

        lerpData.position = Vector3.LerpUnclamped(previousData.position, nextData.position, t);
        lerpData.rotation = Quaternion.LerpUnclamped(previousData.rotation, nextData.rotation, t);
        lerpData.scale = Vector3.LerpUnclamped(previousData.scale, nextData.scale, t);
        lerpData.color = Color.LerpUnclamped(previousData.color, nextData.color, t);
        lerpData.isVisible = nextData.isVisible;
        lerpData.isGrabbable = nextData.isGrabbable;
        lerpData.isVideo = false;


        presentationObject.ApplyDataToObject(lerpData);
    }

    public void PlayPreview(Transform prevTrans, Material prevMat)
    {
        _previousTransform= prevTrans;
        _previousMaterial= prevMat;

        _sequence.Play();
    }

    public void StopPreview() 
    {
        _previousTransform.position = previousData.position;
        _previousTransform.rotation = previousData.rotation;
        _previousTransform.localScale = previousData.localScale;
        _previousMaterial.color = previousData.color;
        _sequence.Kill();
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
