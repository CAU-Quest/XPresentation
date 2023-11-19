using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoundObjectType
{
    TwoDimension = 0,
    ThreeDimension = 1
}

public class SelectObject : MonoBehaviour
{
    [SerializeField] 
    private BoundObjectType BoundObjectType = BoundObjectType.ThreeDimension;

    public BoundObjectType GetBoundObjectType()
    {
        return BoundObjectType;
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
