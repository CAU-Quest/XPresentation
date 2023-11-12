using System;
using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using Oculus.Interaction;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using Object = UnityEngine.Object;

public class XRSelector : MonoBehaviour
{
    public static XRSelector Instance = null;

    public GameObject selectedObject;
    
    public BoundBoxLine[] lineList;
    public VertexHandler[] vertexList;

    public Object linePrefab;
    public GameObject vertexPrefab;


    public CenterPositionByVertex centerPositionByVertex;
    public BoundBox boundBox;
    public TransformByVertexHandler transformByVertexHandler;
    
    [Header("Line Properties")]
    
    public Material lineMaterial;
    
    [HideInInspector]
    [Range(0.005f, 0.25f)] public float lineWidth = 0.03f;
    [HideInInspector]
    public Color lineColor = Color.white;
    [HideInInspector]
    public int numCapVertices = 0;


    public GameObject Cursor;

    public bool edgeSelected = false;

    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }
        
        centerPositionByVertex = selectedObject.GetComponent<CenterPositionByVertex>();
        boundBox = selectedObject.GetComponent<BoundBox>();
        transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();
        if(boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null)
            Debug.Log("Selected Object doesn't have correct components");
        
        SetVertex();
        SetLines();
        SetLineVertex();

    }

    private void SetVertex() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        vertexList = GetComponentsInChildren<VertexHandler>();
        if (vertexList.Length == 0)
        {
            vertexList = new VertexHandler[8];
            for (int i = 0; i < 8; i++)
            {
#if UNITY_EDITOR
                GameObject go = PrefabUtility.InstantiatePrefab(vertexPrefab) as GameObject;
                go.transform.SetParent(transform);
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
#else
                        GameObject go = (GameObject)Instantiate(vertexPrefab, transform, false);
#endif
                vertexList[i] = go.GetComponent<VertexHandler>();
                vertexList[i].SetVertex(i / 4, (i % 4) / 2, (i % 4 % 2));
            }
        }
        else
        {
            for (int i = 0; i < vertexList.Length; i++)
            {

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(vertexList[i].gameObject) == vertexPrefab)
                    {
                        vertexList[i].enabled = true;
                    }
                    else
                    {
                        vertexList[i].gameObject.SetActive(false);
                        GameObject go = PrefabUtility.InstantiatePrefab(vertexPrefab) as GameObject;
                        go.transform.SetParent(transform);
                        go.transform.position = Vector3.zero;
                        go.transform.rotation = Quaternion.identity;
                        vertexList[i] = go.GetComponent<VertexHandler>();
                    }
                }
#endif
                vertexList[i].enabled = true;
            }
        }
    }


    private void Update()
    {
        if(edgeSelected) selectedObject.transform.rotation = transform.rotation;
        else transform.rotation = selectedObject.transform.rotation;
    }

    private void SetLines()
    {
        
        Debug.Log("Start Set Lines");
        lineList = GetComponentsInChildren<BoundBoxLine>();
        if (lineList.Length == 0)
        {
            Debug.Log("Create Line List");
            lineList = new BoundBoxLine[12];
            for (int i = 0; i < 12; i++)
            {
#if UNITY_EDITOR
                GameObject go = PrefabUtility.InstantiatePrefab(linePrefab) as GameObject;
                go.transform.SetParent(transform);
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
#else
                GameObject go = (GameObject)Instantiate(linePrefab, transform, false);
#endif
                lineList[i] = go.GetComponent<BoundBoxLine>();
            }
        }
        else
        {
            Debug.Log("LineList.Length != 0");
            for (int i = 0; i < lineList.Length; i++)
            {

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(lineList[i].gameObject) == linePrefab)
                    {
                        lineList[i].enabled = true;
                    }
                    else
                    {
                        lineList[i].gameObject.SetActive(false);
                        GameObject go = PrefabUtility.InstantiatePrefab(linePrefab) as GameObject;
                        go.transform.SetParent(transform);
                        go.transform.position = Vector3.zero;
                        go.transform.rotation = Quaternion.identity;
                        lineList[i] = go.GetComponent<BoundBoxLine>();
                    }
                }
#endif
                lineList[i].enabled = true;
            }
        }
    }


    public void SetLineVertex()
    {
        lineList[0].SetVertex(vertexList[0], vertexList[1]);
        lineList[1].SetVertex(vertexList[2], vertexList[3]);
        lineList[2].SetVertex(vertexList[4], vertexList[5]);
        lineList[3].SetVertex(vertexList[6], vertexList[7]);

        lineList[0].edgeHandler.Init(OneGrabRotateTransformer.Axis.Right, transform);
        lineList[1].edgeHandler.Init(OneGrabRotateTransformer.Axis.Right, transform);
        lineList[2].edgeHandler.Init(OneGrabRotateTransformer.Axis.Right, transform);
        lineList[3].edgeHandler.Init(OneGrabRotateTransformer.Axis.Right, transform);
        
        lineList[4].SetVertex(vertexList[1], vertexList[5]);
        lineList[5].SetVertex(vertexList[0], vertexList[4]);
        lineList[6].SetVertex(vertexList[2], vertexList[6]);
        lineList[7].SetVertex(vertexList[3], vertexList[7]);
        
        lineList[4].edgeHandler.Init(OneGrabRotateTransformer.Axis.Up, transform);
        lineList[5].edgeHandler.Init(OneGrabRotateTransformer.Axis.Up, transform);
        lineList[6].edgeHandler.Init(OneGrabRotateTransformer.Axis.Up, transform);
        lineList[7].edgeHandler.Init(OneGrabRotateTransformer.Axis.Up, transform);
        
        lineList[8].SetVertex(vertexList[1], vertexList[3]);
        lineList[9].SetVertex(vertexList[0], vertexList[2]);
        lineList[10].SetVertex(vertexList[5], vertexList[7]);
        lineList[11].SetVertex(vertexList[4], vertexList[6]);
        
        
        lineList[8].edgeHandler.Init(OneGrabRotateTransformer.Axis.Forward, transform);
        lineList[9].edgeHandler.Init(OneGrabRotateTransformer.Axis.Forward, transform);
        lineList[10].edgeHandler.Init(OneGrabRotateTransformer.Axis.Forward, transform);
        lineList[11].edgeHandler.Init(OneGrabRotateTransformer.Axis.Forward, transform);
        
    }

    private void SetLineRenderers()
    {
        Gradient colorGradient = new Gradient();
        colorGradient.SetKeys(new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f)},  new GradientAlphaKey[] { new GradientAlphaKey(lineColor.a, 0.0f) });
        foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>(true))
        {
            lr.startWidth = lineWidth;
            lr.enabled = true;
            lr.numCapVertices = numCapVertices;
            lr.colorGradient = colorGradient;
            lr.material = lineMaterial;
        }
    }


    public VertexHandler[] GetVertexList()
    {
        if (vertexList != null && vertexList.Length == 8)
            return vertexList;
        return null;
    }
    public BoundBoxLine[] GetLineList()
    {
        if (lineList != null && lineList.Length == 12)
            return lineList;
        return null;
    }
        
    public void SetComponent()
    {
        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }
        
        transform.rotation = selectedObject.transform.rotation;
        
        centerPositionByVertex = selectedObject.GetComponent<CenterPositionByVertex>();
        boundBox = selectedObject.GetComponent<BoundBox>();
        transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();

        centerPositionByVertex.enabled = true;
        boundBox.enabled = true;
        transformByVertexHandler.enabled = true;
        
        if (boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null)
        {
            Debug.Log("Selected Object doesn't have correct components");
            return;
        }
        
        boundBox.UpdateBounds();
        centerPositionByVertex.CenterPosition();
        boundBox.UpdateBounds();
        
#if UNITY_EDITOR
        for (int i = 0; i < 12; i++)
        {
            transformByVertexHandler.lineList[i].UpdateLine();
        }
#endif
    }
    
#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlaying) return;
        if (null == Instance)
        {
            Instance = this;
        }

        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }
        centerPositionByVertex = selectedObject.GetComponent<CenterPositionByVertex>();
        boundBox = selectedObject.GetComponent<BoundBox>();
        transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();
        if(boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null)
            Debug.Log("Selected Object doesn't have correct components");
        
        
        SetVertex();
        SetLines();
        SetLineRenderers();
        SetLineVertex();
    }
#endif
    
    
    void OnEnable()
    {
        vertexList = GetComponentsInChildren<VertexHandler>(true);
        lineList = GetComponentsInChildren<BoundBoxLine>(true);
        
        VertexHandler[] lrs = GetComponentsInChildren<VertexHandler>(true);
        BoundBoxLine[] lls = GetComponentsInChildren<BoundBoxLine>(true);
        
        for (int i = 0; i < lrs.Length; i++)
        {
            if (!lrs[i].gameObject.activeSelf) DestroyImmediate(lrs[i].gameObject);
        }
        for (int i = 0; i < lls.Length; i++)
        {
            if (!lls[i].gameObject.activeSelf) DestroyImmediate(lls[i].gameObject);
        }
        
        SetVertex();
        SetLines();
        SetLineRenderers();
        SetLineVertex();
    }
    void OnDestroy()
    {
        VertexHandler[] lrs = GetComponentsInChildren<VertexHandler>(true);
        BoundBoxLine[] lls = GetComponentsInChildren<BoundBoxLine>(true);
        
        for (int i = 0; i < lrs.Length; i++)
        {
            DestroyImmediate(lrs[i].gameObject);
        }
        for (int i = 0; i < lls.Length; i++)
        {
            DestroyImmediate(lls[i].gameObject);
        }
    }

    void OnDisable()
    {
        VertexHandler[] lrs = GetComponentsInChildren<VertexHandler>(true);
        BoundBoxLine[] lls = GetComponentsInChildren<BoundBoxLine>(true);

        for (int i = 0; i < lrs.Length; i++)
        {
            lrs[i].enabled = false;
        }

        for (int i = 0; i < lls.Length; i++)
        {
            lls[i].enabled = false;
        }
    }
}
