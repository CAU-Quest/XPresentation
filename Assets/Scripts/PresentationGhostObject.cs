using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.XR.Interaction.Toolkit;

public class PresentationGhostObject : MonoBehaviour
{
    private int id = 1;

    public PresentationObject parentObject;

    public void Start()
    {
        //UnityAction<SelectExitEventArgs> action = SaveTransformToSlide;
        //GetComponent<XRGrabInteractable>().selectExited.AddListener(action);
    }

    public void applyTransform()
    {
        if(parentObject.GetCurrentSlide() + 1 < MainSystem.Instance.GetSlideCount())
            SetTransform(parentObject.slideData[parentObject.GetCurrentSlide() + 1].position, parentObject.slideData[parentObject.GetCurrentSlide() + 1].rotation, parentObject.slideData[parentObject.GetCurrentSlide() + 1].scale);
    }
    public void OnEnable()
    {
        applyTransform();
    }
    /*
    public void SaveTransformToSlide(SelectExitEventArgs args)
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
    */

    public int GetID()
    {
        return id;
    }
    public void SetID(int id)
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
