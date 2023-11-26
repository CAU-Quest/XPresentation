using System;
using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using UnityEngine;

public enum BoundObjectType
{
    TwoDimension = 0,
    ThreeDimension = 1
}

public class SelectObject : MonoBehaviour
{
    [SerializeField] 
    [Header("Object Property")]
    public DeployType objectType;
    public BoundObjectType BoundObjectType = BoundObjectType.ThreeDimension;

    [HideInInspector]
    public TransformByVertexHandler _transformByVertexHandler;
    [HideInInspector]
    public BoundBox _boundBox;
    [HideInInspector]
    public CenterPositionByVertex _centerPositionByVertex;
    public IPresentationObject _presentationObject;
    
    
    public BoundObjectType GetBoundObjectType()
    {
        return BoundObjectType;
    }

    public void Start()
    {
        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            if (_transformByVertexHandler == null) _transformByVertexHandler = GetComponent<TransformByVertexHandler>();
            if (_boundBox == null) _boundBox = GetComponent<BoundBox>();
            if (_centerPositionByVertex == null) _centerPositionByVertex = GetComponent<CenterPositionByVertex>();
            if (_presentationObject == null) _presentationObject = GetComponentInChildren<IPresentationObject>();
        }
        if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            if (_transformByVertexHandler == null) _transformByVertexHandler = GetComponent<TransformByVertexHandler>();
            if (_presentationObject == null) _presentationObject = GetComponentInChildren<IPresentationObject>();
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
            XRSelector.Instance.transformByVertexHandler = _transformByVertexHandler;
            XRSelector.Instance.centerPositionByVertex = _centerPositionByVertex;
            XRSelector.Instance.boundBox = _boundBox;
            XRSelector.Instance.presentationObject = _presentationObject;
        }
        else
        {
            XRSelector.Instance.transformByVertexHandler = _transformByVertexHandler;
            XRSelector.Instance.presentationObject = _presentationObject;
        }
        
        XRSelector.Instance.SetComponent(this, BoundObjectType);
        
        XRSelector.Instance.transform.rotation = transform.rotation;

        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            int lineLength = XRSelector.Instance.lineList.Length;
            int vertexLength = XRSelector.Instance.vertexList.Length;

            for (int i = 0; i < lineLength; i++)
            {
                XRSelector.Instance.lineList[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < vertexLength; i++)
            {
                XRSelector.Instance.vertexList[i].gameObject.SetActive(true);
            }
        }
        else if(BoundObjectType == BoundObjectType.TwoDimension)
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
