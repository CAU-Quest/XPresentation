using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PresentationGhostObject : MonoBehaviour, IPresentationObject
{
    private uint id = 1;

    public PresentationObject parentObject;

    public void Start()
    {
        UnityEngine.Events.UnityAction<Oculus.Interaction.PointerEvent> pointerAction = (args) => SaveTransformToSlide();
        GetComponent<PointableUnityEventWrapper>().WhenUnselect.AddListener(pointerAction);
    }

    public void applyTransform()
    {
        if(parentObject && parentObject.GetCurrentSlide() + 1 < MainSystem.Instance.GetSlideCount())
            SetSlideObjectData(parentObject.slideData[parentObject.GetCurrentSlide() + 1]);
    }
    public void OnEnable()
    {
        applyTransform();
    }
    
    public void SaveTransformToSlide()
    {
        SlideObjectData transformData = new SlideObjectData();
        transformData.position = transform.parent.position;
        transformData.rotation = transform.parent.rotation;
        transformData.scale = transform.parent.localScale;
        MainSystem.Instance.slideList[parentObject.GetCurrentSlide() + 1].AddObjectData(this.id, transformData);
        parentObject.slideData[parentObject.GetCurrentSlide() + 1] = transformData;
        parentObject.animationList[parentObject.GetCurrentSlide() + 1].SetPreviousSlideObjectData(transformData);
        if(parentObject.GetCurrentSlide() >= 0)
            parentObject.animationList[parentObject.GetCurrentSlide()].SetNextSlideObjectData(transformData);
        
        Debug.Log("Ghost Save Complete");
    }

    public uint GetID()
    {
        return id;
    }
    public void SetID(uint id)
    {
        this.id = id;
    }
    
    public void SetSlideObjectData(SlideObjectData slideObjectData)
    {
        transform.SetPositionAndRotation(slideObjectData.position, slideObjectData.rotation);
        transform.parent.localScale = slideObjectData.scale;
    }

    public SlideObjectData GetSlideObjectData()
    {
        SlideObjectData data = new SlideObjectData();
        data.position = transform.parent.position;
        data.rotation = transform.parent.rotation;
        data.scale = transform.parent.localScale;
        
        return data;
    }
}
