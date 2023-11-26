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
    public DeployType deployType;

    public void Start()
    {
        UnityAction<PointerEvent> pointerAction = (args) => UpdateCurrentObjectDataInSlide();
        GetComponent<PointableUnityEventWrapper>().WhenUnselect.AddListener(pointerAction);
    }

    public void applyTransform()
    {
        if(parentObject && parentObject.GetCurrentSlide() + 1 < MainSystem.Instance.GetSlideCount())
            ApplyDataToObject(parentObject.slideData[parentObject.GetCurrentSlide() + 1]);
    }
    public void OnEnable()
    {
        applyTransform();
    }
    
    public void UpdateCurrentObjectDataInSlide()
    {
        SlideObjectData transformData = new SlideObjectData();
        transformData.position = transform.parent.position;
        transformData.rotation = transform.parent.rotation;
        transformData.scale = transform.parent.localScale;
        parentObject.slideData[parentObject.GetCurrentSlide() + 1] = transformData;
        parentObject.animationList[parentObject.GetCurrentSlide() + 1].SetPreviousSlideObjectData(transformData);
        if(parentObject.GetCurrentSlide() >= 0)
            parentObject.animationList[parentObject.GetCurrentSlide()].SetNextSlideObjectData(transformData);
        
        Debug.Log("Ghost Save Complete");
    }

    public void ApplyDataToSlide(SlideObjectData data)
    {
        throw new NotImplementedException();
    }

    public uint GetID()
    {
        return id;
    }
    public void SetID(uint id)
    {
        this.id = id;
    }
    
    public void ApplyDataToObject(SlideObjectData data)
    {
        transform.SetPositionAndRotation(data.position, data.rotation);
        transform.parent.localScale = data.scale;
        if (data.isVisible)
        {
            gameObject.SetActive(false);
        }
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
