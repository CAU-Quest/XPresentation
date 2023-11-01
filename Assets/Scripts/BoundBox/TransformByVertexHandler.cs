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
    private BoundBox boundBox;
    
    public VertexHandler[] vertexList;

    public GameObject vertexPrefab;

    public Vector3[,,] corners;

    public Vector3 initialCenter;

    public Vector3 initialScale;

    public float initalWidth;
    public float initalHeight;
    public float initalDepth;

    public BoundBoxLine[] lineList;
    
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
        //XRSelector.Instance.centerPositionByVertex.CenterPosition();
        //XRSelector.Instance.boundBox.CalculateBounds();
        SetCorner(XRSelector.Instance.boundBox.GetCorner());
        SetVertex();
        SetInitialVertexPosition();
        SetLine();
        CalculateInitialTransform();
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
        Vector3 point = corners[0, 0, 0];
        Vector3 otherPoint = corners[1, 1, 1];
        
        initialCenter = (point + otherPoint) / 2f;
        
        Debug.Log("Initial Transform Pos : " + point + " / other Pos : " + otherPoint);
        initalWidth = Mathf.Abs(point.x - otherPoint.x);
        initalHeight = Mathf.Abs(point.z - otherPoint.z);
        initalDepth = Mathf.Abs(point.y - otherPoint.y);

        initialScale = transform.localScale;
    }

    private void SetVertex() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        vertexList = XRSelector.Instance.GetComponentsInChildren<VertexHandler>();
        if (vertexList.Length == 0)
        {
            vertexList = new VertexHandler[8];
            for (int i = 0; i < 8; i++)
            {
#if UNITY_EDITOR
                GameObject go = PrefabUtility.InstantiatePrefab(vertexPrefab) as GameObject;
                go.transform.SetParent(XRSelector.Instance.transform);
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
#else
                        GameObject go = (GameObject)Instantiate(vertexPrefab, transform, false);
#endif
                vertexList[i] = go.GetComponent<VertexHandler>();
                vertexList[i].SetVertex(i / 4, (i % 4) / 2, (i % 4 % 2));
            }
        }
    }

    private void SetLine()
    {
        if (XRSelector.Instance.boundBox == null) return;
        lineList = XRSelector.Instance.boundBox.GetLineList();
        
        lineList[0].SetVertex(vertexList[0], vertexList[1]);
        lineList[1].SetVertex(vertexList[2], vertexList[3]);
        lineList[2].SetVertex(vertexList[4], vertexList[5]);
        lineList[3].SetVertex(vertexList[6], vertexList[7]);

        lineList[0].GetComponent<CapsuleCollider>().direction = 0;
        lineList[1].GetComponent<CapsuleCollider>().direction = 0;
        lineList[2].GetComponent<CapsuleCollider>().direction = 0;
        lineList[3].GetComponent<CapsuleCollider>().direction = 0;
        
        lineList[4].SetVertex(vertexList[1], vertexList[5]);
        lineList[5].SetVertex(vertexList[0], vertexList[4]);
        lineList[6].SetVertex(vertexList[2], vertexList[6]);
        lineList[7].SetVertex(vertexList[3], vertexList[7]);
        
        lineList[4].GetComponent<CapsuleCollider>().direction = 1;
        lineList[5].GetComponent<CapsuleCollider>().direction = 1;
        lineList[6].GetComponent<CapsuleCollider>().direction = 1;
        lineList[7].GetComponent<CapsuleCollider>().direction = 1;
        
        lineList[8].SetVertex(vertexList[1], vertexList[3]);
        lineList[9].SetVertex(vertexList[0], vertexList[2]);
        lineList[10].SetVertex(vertexList[5], vertexList[7]);
        lineList[11].SetVertex(vertexList[4], vertexList[6]);
        
        lineList[8].GetComponent<CapsuleCollider>().direction = 2;
        lineList[9].GetComponent<CapsuleCollider>().direction = 2;
        lineList[10].GetComponent<CapsuleCollider>().direction = 2;
        lineList[11].GetComponent<CapsuleCollider>().direction = 2;
    }
    
    private void SetInitialVertexPosition() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        int index;
        
        for (int top = 0; top < 2; top++)
        {
            for (int left = 0; left < 2; left++)
            {
                for (int front = 0; front < 2; front++)
                {
                    index = top * 4 + front * 2 + left;
                    vertexList[index].transform.position = corners[left, top, front] + transform.position;
                    Debug.Log("vertexList[" + index +"] : " + vertexList[index].transform.position);
                }
            }
        }
        
    }

    public void ApplyCurrentTransform(float currentWidth, float currentDepth, float currentHeight, Vector3 center)
    {
        Vector3 newScale = new Vector3(initialScale.x * currentWidth / initalWidth, initialScale.y * currentDepth / initalDepth, initialScale.z * currentHeight / initalHeight);

        Vector3 centerDiff = Vector3.Scale(new Vector3(currentWidth, currentDepth, currentHeight) / 2f + initialCenter,
            newScale - initialScale);
        
        Debug.Log("New Scale : " + newScale + " / Center Diff : " + centerDiff);
        
        transform.position = center;
        transform.localScale = newScale;
    }
    
    
    void OnEnable()
    {
        vertexList = XRSelector.Instance.GetComponentsInChildren<VertexHandler>(true);
        VertexHandler[] lrs = XRSelector.Instance.GetComponentsInChildren<VertexHandler>(true);
        for (int i = 0; i < lrs.Length; i++)
        {
            if (!lrs[i].gameObject.activeSelf) DestroyImmediate(lrs[i].gameObject);
        }
        Init();
    }
    void OnDestroy()
    {
        VertexHandler[] lrs = XRSelector.Instance.GetComponentsInChildren<VertexHandler>(true);
        for (int i = 0; i < lrs.Length; i++)
        {
            DestroyImmediate(lrs[i].gameObject);
        }
    }

    void OnDisable()
    {
        VertexHandler[] lrs = XRSelector.Instance.GetComponentsInChildren<VertexHandler>();
        for (int i = 0; i < lrs.Length; i++)
        {
            lrs[i].enabled = false;
        }
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlaying) return;
        if (XRSelector.Instance.transformByVertexHandler != this) enabled = false;
        else enabled = true;
        Init();
    }


#endif
}
