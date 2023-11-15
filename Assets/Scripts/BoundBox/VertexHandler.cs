using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VertexHandler : MonoBehaviour
{
    [SerializeField]
    private bool isSelected;
    
    [SerializeField]
    private int top;
    [SerializeField]
    private int front;
    [SerializeField]
    private int left;

    [SerializeField]
    private VertexHandler[] vertexList;

    private float currentWidth;
    private float currentHeight;
    private float currentDepth;

    private Vector3 beforePosition;
    void Start()
    {
        isSelected = false;
    }
    public void SetVertex(int top, int front, int left)
    {
        this.top = top;
        this.front = front;
        this.left = left;
    }

    public void SelectVertex()
    {
        isSelected = true;
    }

    public void UnselectVertex()
    {
        isSelected = false;
        XRSelector.Instance.selectObject.Select();
    }


    private int invertValue(int val)
    {
        return (val == 0) ? 1 : 0;
    }
    private void Update()
    {
        if (isSelected)
        {
            if (transform.localPosition != beforePosition)
            {
                beforePosition = transform.localPosition;
                if (XRSelector.Instance.GetBoundObjectType() == BoundObjectType.ThreeDimension)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            vertexList[i * 4 + j * 2 + left].transform.localPosition = new Vector3(
                                transform.localPosition.x, vertexList[i * 4 + j * 2 + left].transform.localPosition.y,
                                vertexList[i * 4 + j * 2 + left].transform.localPosition.z);
                            vertexList[top * 4 + i * 2 + j].transform.localPosition = new Vector3(
                                vertexList[top * 4 + i * 2 + j].transform.localPosition.x, transform.localPosition.y,
                                vertexList[top * 4 + i * 2 + j].transform.localPosition.z);
                            vertexList[i * 4 + front * 2 + j].transform.localPosition = new Vector3(
                                vertexList[i * 4 + front * 2 + j].transform.localPosition.x, vertexList[i * 4 + front * 2 + j].transform.localPosition.y,
                                transform.localPosition.z);
                        }
                    }

                    Vector3 point = transform.position;
                    Vector3 otherPoint = vertexList[invertValue(top) * 4 + invertValue(front) * 2 + invertValue(left)]
                        .transform.position;

                    currentWidth = Vector3.Distance(transform.localPosition,
                        vertexList[top * 4 + front * 2 + invertValue(left)].transform.localPosition);
                    currentDepth = Vector3.Distance(transform.localPosition,
                        vertexList[invertValue(top) * 4 + front * 2 + left].transform.localPosition);
                    currentHeight = Vector3.Distance(transform.localPosition,
                        vertexList[top * 4 + invertValue(front) * 2 + left].transform.localPosition);
                
                    XRSelector.Instance.transformByVertexHandler.ApplyCurrentTransform(currentWidth, currentDepth, currentHeight, (point + otherPoint) / 2f);
                } else if (XRSelector.Instance.GetBoundObjectType() == BoundObjectType.TwoDimension){
                
                    
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
                        vertexList[invertValue(top) * 4 + left].transform.localPosition.z);
                    for (int i = 0; i < 2; i++)
                    {
                            vertexList[i * 4 + left].transform.localPosition = new Vector3(
                                transform.localPosition.x, vertexList[i * 4 + + left].transform.localPosition.y,
                                vertexList[i * 4 + + left].transform.localPosition.z);
                            vertexList[top * 4 + i].transform.localPosition = new Vector3(
                                vertexList[top * 4 + i].transform.localPosition.x, transform.localPosition.y,
                                vertexList[top * 4 + i].transform.localPosition.z);
                    }

                    Vector3 point = transform.position;
                    Vector3 otherPoint = vertexList[invertValue(top) * 4 + invertValue(left)]
                        .transform.position;

                    currentWidth = Vector3.Distance(transform.localPosition,
                        vertexList[top * 4 + invertValue(left)].transform.localPosition);
                    currentDepth = Vector3.Distance(transform.localPosition,
                        vertexList[invertValue(top) * 4 + left].transform.localPosition);
                
                    XRSelector.Instance.transformByVertexHandler.ApplyCurrentTransform(currentWidth, currentDepth, 1f, (point + otherPoint) / 2f);
                }
            }
        }
    }

}
