using System.Collections;
using System.Collections.Generic;
using DimBoxesCustom;
using UnityEditor;
using UnityEngine;

public class BoundBoxCustom : MonoBehaviour
{
    public enum BoundSource
    {
        meshes,
        boxCollider,
    }
    
    [Header("Bound Box Property")]
    public BoundSource boundSource = BoundSource.meshes;
    
    
    [Header("Bound Box Property")]
    [HideInInspector]public bool isDrawingBoundBox = false;

    [HideInInspector] public Material lineMaterial;

    [HideInInspector]public Color lineColor = new Color(1f, 1f, 1f, 0.75f);

    [HideInInspector]public Object linePrefab;

    [HideInInspector]public float lineWidth = 0.03f;

    [HideInInspector]public int numCapVertices = 5;
    
    //Internal Variables

    protected Vector3[] corners = new Vector3[0];

    protected Vector3[,] lines = new Vector3[0, 0];

    protected LineRenderer[] lineList;
    
    
    protected Bounds bound;
    protected Vector3 boundOffset;
    [HideInInspector]
    public Bounds colliderBound;
    [HideInInspector]
    public Vector3 colliderBoundOffset;
    [HideInInspector]
    public Bounds meshBound;
    [HideInInspector]
    public Vector3 meshBoundOffset;
    
    
    private Vector3 topFrontLeft;
    private Vector3 topFrontRight;
    private Vector3 topBackLeft;
    private Vector3 topBackRight;
    private Vector3 bottomFrontLeft;
    private Vector3 bottomFrontRight;
    private Vector3 bottomBackLeft;
    private Vector3 bottomBackRight;

    
    [HideInInspector] public Vector3 startingScale;
    private Vector3 previousScale;
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    
    
    public virtual void Init()
    {
        SetPoints();
        SetLines();
        SetLineRenderers();
    }
    
    void SetPoints()
    {

        if (boundSource == BoundSource.boxCollider)
        {
            BoxCollider bc = GetComponent<BoxCollider>();
            if (!bc)
            {
                Debug.LogError("no BoxCollider - add BoxCollider to " + gameObject.name + " gameObject");
                return;

            }
            bound = new Bounds(bc.center, bc.size);
            boundOffset = bc.center;
        }

        else
        {
            bound = meshBound;
            boundOffset = meshBoundOffset;
        }
        //bound.size = new Vector3(bound.size.x * transform.localScale.x / startingScale.x, bound.size.y * transform.localScale.y / startingScale.y, bound.size.z * transform.localScale.z / startingScale.z);
        //boundOffset = new Vector3(boundOffset.x * transform.localScale.x / startingScale.x, boundOffset.y * transform.localScale.y / startingScale.y, boundOffset.z * transform.localScale.z / startingScale.z);

        topFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, 1, 1));
        topFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, 1, 1));
        topBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, 1, -1));
        topBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, 1, -1));
        bottomFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, -1, 1));
        bottomFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, -1, 1));
        bottomBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, -1, -1));
        bottomBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, -1, -1));

        corners = new Vector3[] { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };
    }
    
    protected virtual void SetLines()
    {
        Debug.Log("BB-lr");
        lineList = GetComponentsInChildren<LineRenderer>();
        if (lineList.Length == 0)
        {
            lineList = new LineRenderer[12];
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
                lineList[i] = go.GetComponent<LineRenderer>();
            }
        }
        else
        {
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
                        lineList[i] = go.GetComponent<LineRenderer>();
                    }
                }
#endif
                lineList[i].enabled = true;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            //width
            lineList[i].SetPositions(new Vector3[] { transform.TransformPoint(corners[2 * i]), transform.TransformPoint(corners[2 * i + 1]) });
            //height
            lineList[i + 4].SetPositions(new Vector3[] { transform.TransformPoint(corners[i]), transform.TransformPoint(corners[i + 4]) });
            //depth
            lineList[i + 8].SetPositions(new Vector3[] { transform.TransformPoint(corners[2 * i]), transform.TransformPoint(corners[2 * i + 3 - 4 * (i % 2)]) });
        }
    }
    
    
    public virtual void SetLineRenderers()
    {
        Gradient colorGradient = new Gradient();
        colorGradient.SetKeys(new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f)},  new GradientAlphaKey[] { new GradientAlphaKey(lineColor.a, 0.0f) });
        foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>(true))
        {
            lr.startWidth = lineWidth;
            lr.enabled = isDrawingBoundBox;
            lr.numCapVertices = numCapVertices;
            lr.colorGradient = colorGradient;
            lr.material = lineMaterial;
        }
    }
    
    void Reset()
    {
        AccurateBounds();
        Start();
    }
    void Start()
    {

        previousPosition = transform.position;
        previousRotation = transform.rotation;
        startingScale = transform.localScale;
        previousScale = startingScale;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void LateUpdate()
    {
        if (transform.localScale != previousScale)
        {
            SetPoints();
        }
        if (transform.position != previousPosition || transform.rotation != previousRotation || transform.localScale != previousScale)
        {
            SetLines();
            previousRotation = transform.rotation;
            previousPosition = transform.position;
            previousScale = transform.localScale;
        }
        //if(wire_renderer) cameralines.setOutlines(lines, wireColor, new Vector3[0][]);
    }
    

    void OnEnable()
    {
        //cameralines = FindObjectOfType(typeof(DimBoxes.DrawLines)) as DimBoxes.DrawLines;
        lineList = GetComponentsInChildren<LineRenderer>(true);
        LineRenderer[] lrs = GetComponentsInChildren<LineRenderer>(true);
        for (int i = 0; i < lrs.Length; i++)
        {
            if (!lrs[i].gameObject.activeSelf) DestroyImmediate(lrs[i].gameObject);
        }
        Init();
    }
    void OnDestroy()
    {
        LineRenderer[] lrs = GetComponentsInChildren<LineRenderer>(true);
        for (int i = 0; i < lrs.Length; i++)
        {
            DestroyImmediate(lrs[i].gameObject);
        }
    }

    void OnDisable()
    {
        LineRenderer[] lrs = GetComponentsInChildren<LineRenderer>();
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

    void OnRenderObject()
    {
        if (lines == null||!isDrawingBoundBox||!lineMaterial) return;
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);

        GL.Color(lineColor);
        for (int i = 0; i < lines.GetLength(0); i++)
        {
            GL.Vertex(lines[i, 0]);
            GL.Vertex(lines[i, 1]);
        }
 
        GL.End();
        GL.PopMatrix();
    }

    public void AccurateBounds()
    {

        MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
#if UNITY_EDITOR
        if (meshes.Length == 0)
        {
            EditorUtility.DisplayDialog("Dimbox message", "The object contains no meshes!\n- please reassign", "Continue");
        }
#endif
        VertexData[] vertexData = new VertexData[meshes.Length];
        for (int i = 0; i < meshes.Length; i++)
        {
            Mesh ms = meshes[i].sharedMesh;
            vertexData[i] = new VertexData(ms.vertices, meshes[i].transform.localToWorldMatrix);
        }
        Vector3 v1 = transform.right;
        Vector3 v2 = transform.up;
        Vector3 v3 = transform.forward;

        meshBound = OrientedBounds.OBB(vertexData, v1, v2, v3);
        meshBoundOffset = transform.InverseTransformPoint(meshBound.center);

    }
}
