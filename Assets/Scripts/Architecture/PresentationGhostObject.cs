using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PresentationGhostObject : MonoBehaviour
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
            SetTransform(parentObject.slideData[parentObject.GetCurrentSlide() + 1].position, parentObject.slideData[parentObject.GetCurrentSlide() + 1].rotation, parentObject.slideData[parentObject.GetCurrentSlide() + 1].scale);
    }
    public void OnEnable()
    {
        applyTransform();
    }
    
    public void SaveTransformToSlide()
    {
        TransformData transformData = new TransformData();
        transformData.position = transform.position;
        transformData.rotation = transform.rotation;
        transformData.scale = transform.localScale;
        MainSystem.Instance.slideList[parentObject.GetCurrentSlide() + 1].AddObjectData(this.id, transformData);
        parentObject.slideData[parentObject.GetCurrentSlide() + 1] = transformData;
        parentObject.animationList[parentObject.GetCurrentSlide() + 1].SetPreviousTransform(transformData);
        if(parentObject.GetCurrentSlide() >= 0)
            parentObject.animationList[parentObject.GetCurrentSlide()].SetNextTransform(transformData);
        
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
    
    public void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.SetPositionAndRotation(position, rotation);
        transform.localScale = scale;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public Quaternion GetRotation()
    {
        return this.transform.rotation;
    }
}
