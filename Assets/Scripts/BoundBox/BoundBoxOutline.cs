using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BoundBoxOutline : MonoBehaviour
{
    protected Vector3[,] lines = new Vector3[0, 0];
    
    protected Bounds bound;
    protected Vector3 boundOffset;
    
    [HideInInspector]
    public Bounds meshBound;
    [HideInInspector]
    public Vector3 meshBoundOffset;

    protected Vector3[] corners = new Vector3[0];
    
    public Material lineMaterial;
    public Color wireColor = new Color(0f, 1f, 0.4f, 0.74f);
    
    private Vector3 topFrontLeft;
    private Vector3 topFrontRight;
    private Vector3 topBackLeft;
    private Vector3 topBackRight;
    private Vector3 bottomFrontLeft;
    private Vector3 bottomFrontRight;
    private Vector3 bottomBackLeft;
    private Vector3 bottomBackRight;

    
    
    public Vector3 startingScale;
    private Vector3 previousScale;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    void Reset()
    {
        AccurateBounds();
        Start();
    }
    public virtual void Init()
    {
        SetPoints();
        SetLines();
    }
    
    void SetPoints()
    {
        bound = meshBound;
        boundOffset = meshBoundOffset;
        
        bound.size = new Vector3(bound.size.x * transform.localScale.x / startingScale.x, bound.size.y * transform.localScale.y / startingScale.y, bound.size.z * transform.localScale.z / startingScale.z);
        boundOffset = new Vector3(boundOffset.x * transform.localScale.x / startingScale.x, boundOffset.y * transform.localScale.y / startingScale.y, boundOffset.z * transform.localScale.z / startingScale.z);

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

        //Quaternion rot = transform.rotation;
        //Vector3 pos = transform.position;
        List<Vector3[]> _lines = new List<Vector3[]>();
        //int linesCount = 12;

        Vector3[] _line;
        for (int i = 0; i < 4; i++)
        {
            //width
            _line = new Vector3[] { corners[2 * i], corners[2 * i + 1] };
            _lines.Add(_line);
            //height
            _line = new Vector3[] { corners[i], corners[i + 4] };
            _lines.Add(_line);
            //depth
            _line = new Vector3[] { corners[2 * i], corners[2 * i + 3 - 4 * (i % 2)] };
            _lines.Add(_line);

        }

        lines = new Vector3[_lines.Count, 2];
        for (int j = 0; j < _lines.Count; j++)
        {
            lines[j, 0] = _lines[j][0];
            lines[j, 1] = _lines[j][1];
        }
    }

    void OnEnable()
    {
        Init();
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
        if (lines == null||!lineMaterial) return;
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);

        GL.Color(wireColor);
        for (int i = 0; i < lines.GetLength(0); i++)
        {
            GL.Vertex(lines[i, 0]);
            GL.Vertex(lines[i, 1]);
        }
 
        GL.End();
        GL.PopMatrix();
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        previousRotation = transform.rotation;
        startingScale = transform.localScale;
        previousScale = startingScale;
        Init();
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
