using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TransformByVertexHandler : MonoBehaviour
{
    private BoundBox boundBox;
    
    private VertexHandler[] vertexList;

    public GameObject vertexPrefab;

    public Vector3[,,] corners;

    public Vector3 center;
    
    public float width;
    public float height;
    public float depth;
    
    
    void Reset()
    {
        Start();
        Init();
    }
    
    void Start()
    {
        corners = new Vector3[2, 2, 2];
        setVertex();
    }

    private void Init()
    {
        if(boundBox == null) boundBox = GetComponent<BoundBox>();
        SetCorner(boundBox.GetCorner());
        setVertex();
        UpdateVertex();
    }

    private void SetCorner(Vector3[] vertex) // { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };
    {                                        // corners[left, top, front]
        corners[0, 0, 0] = vertex[1];
        corners[0, 0, 1] = vertex[2];
        corners[0, 1, 0] = vertex[5];
        corners[0, 1, 1] = vertex[6];
        corners[1, 0, 0] = vertex[0];
        corners[1, 0, 1] = vertex[3];
        corners[1, 1, 0] = vertex[4];
        corners[1, 1, 1] = vertex[7];
        CalculateSizeFromVertex(vertex);
    }

    public void CalculateSizeFromVertex(Vector3[] vertex)
    {
        Vector3 center = Vector3.zero;
        for(int i = 0; i < 8; i++)
            center += vertex[i];

        this.center = center / 8;

        width = Vector3.Distance(vertex[1], vertex[0]);
        height = Vector3.Distance(vertex[1], vertex[2]);
        depth = Vector3.Distance(vertex[1], vertex[5]);
    }

    private void setVertex() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        vertexList = transform.parent.gameObject.GetComponentsInChildren<VertexHandler>();
        if (vertexList.Length == 0)
        {
            vertexList = new VertexHandler[8];
            for (int i = 0; i < 8; i++)
            {
#if UNITY_EDITOR
                GameObject go = PrefabUtility.InstantiatePrefab(vertexPrefab) as GameObject;
                go.transform.SetParent(transform.parent);
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
#else
                        GameObject go = (GameObject)Instantiate(vertexPrefab, transform, false);
#endif
                vertexList[i] = go.GetComponent<VertexHandler>();
                vertexList[i].SetVertex(i / 4, (i % 4) / 2, (i % 4 % 2), this);
            }
        }
    }

    public void MoveVertex(int left, int top, int front, Vector3 pos)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                corners[left, i, j].x = pos.x;
                corners[i, top, j].y = pos.y;
                corners[i, j, front].z = pos.z;
            }
        }
        UpdateVertex();
    }
    
    

    private void UpdateVertex() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        int index;
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
        
        CalculateScale();
    }

    public void CalculateScale()
    {
        float currentWidth = Vector3.Distance(corners[0, 0, 0], corners[1, 0, 0]);
        float currentDepth = Vector3.Distance(corners[0, 0, 0], corners[0, 1, 0]);
        float currentHeight = Vector3.Distance(corners[0, 0, 0], corners[0, 0, 1]);

        Vector3 center = Vector3.zero;
        for (int top = 0; top < 2; top++)
        {
            for (int left = 0; left < 2; left++)
            {
                for (int front = 0; front < 2; front++)
                {
                    center += corners[left, top, front];
                }
            }
        }
        this.center = center / 8;

        transform.position = center;
        transform.localScale = new Vector3(currentWidth / width, currentDepth / depth, currentHeight / height);
    }
    
    void OnEnable()
    {
        vertexList = transform.parent.gameObject.GetComponentsInChildren<VertexHandler>(true);
        VertexHandler[] lrs = transform.parent.gameObject.GetComponentsInChildren<VertexHandler>(true);
        for (int i = 0; i < lrs.Length; i++)
        {
            if (!lrs[i].gameObject.activeSelf) DestroyImmediate(lrs[i].gameObject);
        }
        Init();
        
        
    }
    void OnDestroy()
    {
        VertexHandler[] lrs = transform.parent.gameObject.GetComponentsInChildren<VertexHandler>(true);
        for (int i = 0; i < lrs.Length; i++)
        {
            DestroyImmediate(lrs[i].gameObject);
        }
    }

    void OnDisable()
    {
        VertexHandler[] lrs = transform.parent.gameObject.GetComponentsInChildren<VertexHandler>();
        for (int i = 0; i < lrs.Length; i++)
        {
            lrs[i].enabled = false;
        }
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlaying) return;
        Init();
    }


#endif
}
