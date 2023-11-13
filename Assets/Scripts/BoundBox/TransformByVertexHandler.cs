using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DimBoxes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TransformByVertexHandler : MonoBehaviour
{
    [SerializeField]
    private BoundObjectType BoundObjectType = BoundObjectType.ThreeDimension;
    
    private BoundBox boundBox;
    
    public VertexHandler[] vertexList;

    private Vector3[,,] corners;

    private Vector3 initialCenter;

    [SerializeField]
    private Vector3 initialScale;

    [SerializeField]
    private float initialWidth;
    [SerializeField]
    private float initialHeight;
    [SerializeField]
    private float initialDepth;

    public BoundBoxLine[] lineList;

    private Canvas canvas;
    
    void Reset()
    {
        Init();
    }
    
    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (XRSelector.Instance.transformByVertexHandler != this) enabled = false;
        else enabled = true;
        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            SetCorner(XRSelector.Instance.boundBox.GetCorner());
            SetVertex();
            SetInitialVertexPosition();
            SetLine();
            CalculateInitialTransform();
        } else if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            SetVertex();
            SetInitialVertexPosition();
            SetLine();
            CalculateInitialTransform();
        }
    }

    private void SetCorner(Vector3[] vertex) // { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };
    {                         
        corners = new Vector3[2, 2, 2];      // corners[left, top, front]
        corners[0, 0, 0] = vertex[1];
        corners[0, 0, 1] = vertex[2];
        corners[0, 1, 0] = vertex[5];
        corners[0, 1, 1] = vertex[6];
        corners[1, 0, 0] = vertex[0];
        corners[1, 0, 1] = vertex[3];
        corners[1, 1, 0] = vertex[4];
        corners[1, 1, 1] = vertex[7];
    }
    
    public void CalculateInitialTransform()
    {
        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            Vector3 point = corners[0, 0, 0];
            Vector3 otherPoint = corners[1, 1, 1];
        
            initialCenter = (point + otherPoint) / 2f;
        
            initialWidth = Vector3.Distance(corners[0, 0, 0], corners[1, 0, 0]);
            initialHeight = Vector3.Distance(corners[0, 0, 0], corners[0, 0, 1]);
            initialDepth = Vector3.Distance(corners[0, 0, 0], corners[0, 1, 0]);

            initialScale = transform.localScale;
        } else if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            Vector3 point = vertexList[0].transform.position;
            Vector3 otherPoint = vertexList[5].transform.position;
        
            initialCenter = (point + otherPoint) / 2f;
        
            initialWidth = Vector3.Distance(vertexList[0].transform.position, vertexList[1].transform.position);
            initialDepth = Vector3.Distance(vertexList[0].transform.position, vertexList[4].transform.position);

            initialScale = transform.localScale;
        }
    }

    private void SetVertex() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        if (XRSelector.Instance.GetVertexList() != null)
            vertexList = XRSelector.Instance.GetVertexList();
    }

    private void SetLine()
    {
        if (XRSelector.Instance.boundBox == null) return;
        lineList = XRSelector.Instance.GetLineList();
    }
    
    private void SetInitialVertexPosition() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            int index;
            XRSelector.Instance.transform.position = transform.position;
        
            for (int top = 0; top < 2; top++)
            {
                for (int left = 0; left < 2; left++)
                {
                    for (int front = 0; front < 2; front++)
                    {
                        index = top * 4 + front * 2 + left;
                        vertexList[index].transform.localPosition = corners[left, top, front];
                    }
                }
            }
        } else if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            XRSelector.Instance.transform.position = transform.position;

            if (canvas)
            {
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();

                // Canvas의 각 꼭짓점 좌표 가져오기
                Vector3[] canvasCorners = new Vector3[4];
                canvasRect.GetWorldCorners(canvasCorners);

                vertexList[0].transform.position = canvasCorners[1];
                vertexList[1].transform.position = canvasCorners[2];
                vertexList[4].transform.position = canvasCorners[0];
                vertexList[5].transform.position = canvasCorners[3];
            }
        } 
    }

    public void ApplyCurrentTransform(float currentWidth, float currentDepth, float currentHeight, Vector3 center)
    {
        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            Vector3 newScale = new Vector3(initialScale.x * currentWidth / initialWidth, initialScale.y * currentDepth / initialDepth, initialScale.z * currentHeight / initialHeight);

            transform.position = center;
            transform.localScale = newScale;
        } else if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            Vector3 newScale = new Vector3(initialScale.x * currentWidth / initialWidth, initialScale.y * currentDepth / initialDepth, 1f);

            transform.position = center;
            transform.localScale = newScale;
        }
    }
    
    
    void OnEnable()
    {
        BoundObjectType = GetComponent<SelectObject>().GetBoundObjectType();
        if (BoundObjectType == BoundObjectType.TwoDimension) canvas = GetComponentInChildren<Canvas>();
        
        SetVertex();
        SetLine();
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlaying) return;
        if (XRSelector.Instance &&XRSelector.Instance.transformByVertexHandler && XRSelector.Instance.transformByVertexHandler == this) enabled = true;
        else
        {
            enabled = false;
            return;
        }
        BoundObjectType = GetComponent<SelectObject>().GetBoundObjectType();
        if (BoundObjectType == BoundObjectType.TwoDimension) canvas = GetComponentInChildren<Canvas>();
        SetVertex();
        SetLine();
    }
#endif
}
