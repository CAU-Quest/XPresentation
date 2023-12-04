using System;
using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using UnityEngine;
using UnityEngine.Serialization;

public enum BoundObjectType
{
    TwoDimension = 0,
    ThreeDimension = 1
}

public class SelectObject : MonoBehaviour
{
    [SerializeField] 
    [Header("Object Property")]
    public DeployType deployType;
    public BoundObjectType BoundObjectType = BoundObjectType.ThreeDimension;

    [HideInInspector] public TransformByVertexHandler transformByVertexHandler; 
    [HideInInspector] public BoundBox boundBox;
    [HideInInspector] public CenterPositionByVertex centerPositionByVertex;
    public IPresentationObject presentationObject;

    [HideInInspector] public string objectPath;
    [HideInInspector] public string imagePath;
    
    
    public BoundObjectType GetBoundObjectType()
    {
        return BoundObjectType;
    }

    public void Start()
    {
        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            if (transformByVertexHandler == null) transformByVertexHandler = GetComponent<TransformByVertexHandler>();
            if (boundBox == null) boundBox = GetComponent<BoundBox>();
            if (centerPositionByVertex == null) centerPositionByVertex = GetComponent<CenterPositionByVertex>();
            if (presentationObject == null) presentationObject = GetComponentInChildren<IPresentationObject>();
        }
        if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            if (transformByVertexHandler == null) transformByVertexHandler = GetComponent<TransformByVertexHandler>();
            if (presentationObject == null) presentationObject = GetComponentInChildren<IPresentationObject>();
        }
    }


    public void Unselect()
    {
        int lineLength = XRSelector.Instance.lineList.Length;
        int vertexLength = XRSelector.Instance.vertexList.Length;

        for (int i = 0; i < lineLength; i++)
        {
            XRSelector.Instance.lineList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < vertexLength; i++)
        {
            XRSelector.Instance.vertexList[i].gameObject.SetActive(false);
        }
    }
    public void Select()
    {
        if(XRSelector.Instance.transformByVertexHandler) XRSelector.Instance.transformByVertexHandler.enabled = false;
        if(XRSelector.Instance.centerPositionByVertex) XRSelector.Instance.centerPositionByVertex.enabled = false;
        if(XRSelector.Instance.boundBox) XRSelector.Instance.boundBox.enabled = false;
        XRSelector.Instance.selectedObject = gameObject;

        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            XRSelector.Instance.transformByVertexHandler = transformByVertexHandler;
            XRSelector.Instance.centerPositionByVertex = centerPositionByVertex;
            XRSelector.Instance.boundBox = boundBox;
            XRSelector.Instance.presentationObject = presentationObject;
            XRSelector.Instance.selectObject = this;
        }
        else
        {
            XRSelector.Instance.transformByVertexHandler = transformByVertexHandler;
            XRSelector.Instance.presentationObject = presentationObject;
            XRSelector.Instance.selectObject = this;
        }

        
        XRSelector.Instance.SetComponent(this, BoundObjectType);
        
        XRSelector.Instance.transform.rotation = transform.rotation;

        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            XRSelector.Instance.ActivateBoundBox();
        }
        else if(BoundObjectType == BoundObjectType.TwoDimension)
        {
            XRSelector.Instance.DeactivateBoundBox();
            
            XRSelector.Instance.lineList[0].gameObject.SetActive(true);
            XRSelector.Instance.lineList[2].gameObject.SetActive(true);
            XRSelector.Instance.lineList[4].gameObject.SetActive(true);
            XRSelector.Instance.lineList[5].gameObject.SetActive(true);
            
            
            XRSelector.Instance.vertexList[0].gameObject.SetActive(true);
            XRSelector.Instance.vertexList[1].gameObject.SetActive(true);
            XRSelector.Instance.vertexList[4].gameObject.SetActive(true);
            XRSelector.Instance.vertexList[5].gameObject.SetActive(true);
        }
    }
}
