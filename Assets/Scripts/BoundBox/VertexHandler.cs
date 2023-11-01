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


    private int invertValue(int val)
    {
        return (val == 0) ? 1 : 0;
    }
    private void LateUpdate()
    {
        if (isSelected)
        {
            if (transform.position != beforePosition)
            {
                beforePosition = transform.position;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        vertexList[i * 4 + j * 2 + left].transform.position = new Vector3(
                            transform.position.x, vertexList[i * 4 + j * 2 + left].transform.position.y,
                            vertexList[i * 4 + j * 2 + left].transform.position.z);
                        vertexList[top * 4 + i * 2 + j].transform.position = new Vector3(
                            vertexList[top * 4 + i * 2 + j].transform.position.x, transform.position.y,
                            vertexList[top * 4 + i * 2 + j].transform.position.z);
                        vertexList[i * 4 + front * 2 + j].transform.position = new Vector3(
                            vertexList[i * 4 + front * 2 + j].transform.position.x, vertexList[i * 4 + front * 2 + j].transform.position.y,
                            transform.position.z);
                    }
                }

                Vector3 point = transform.position;
                Vector3 otherPoint = vertexList[invertValue(top) * 4 + invertValue(front) * 2 + invertValue(left)]
                    .transform.position;

                float currentWidth = Mathf.Abs(point.x - otherPoint.x);
                float currentDepth = Mathf.Abs(point.y - otherPoint.y);
                float currentHeight = Mathf.Abs(point.z - otherPoint.z);
                
                XRSelector.Instance.transformByVertexHandler.ApplyCurrentTransform(currentWidth, currentDepth, currentHeight, (point + otherPoint) / 2f);
            }
        }
    }

}
