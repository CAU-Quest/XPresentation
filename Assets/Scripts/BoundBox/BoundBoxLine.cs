using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEditor;
using UnityEngine;

public class BoundBoxLine : MonoBehaviour
{
    public VertexHandler vertex1;
    public VertexHandler vertex2;

    private LineRenderer lineRenderer;

    public EdgeHandler edgeHandler;
    // Start is called before the first frame update
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeHandler = GetComponentInChildren<EdgeHandler>();
    }


    public void SetAxisToEdgeHandler(EdgeHandler.RotationAxis rotationAxis)
    {
        edgeHandler.ChangeRotationAxis(rotationAxis);
    }
    

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateLine();
    }

    public void UpdateLine()
    {
        if (vertex1 != null && vertex2 != null && lineRenderer != null)
        {
            Vector3 pos1 = vertex1.transform.position;
            Vector3 pos2 = vertex2.transform.position;
        
            lineRenderer.SetPosition(0, pos1);
            lineRenderer.SetPosition(1, pos2);

            transform.position = (pos1 + pos2) / 2f;
        
            Vector3 directionAB = pos2 - pos1;
            // 두 벡터 사이의 각도를 계산하고 해당 각도로 Quaternion을 만듭니다.
            Quaternion rotationToLookAtA = Quaternion.LookRotation(directionAB);

            transform.rotation = rotationToLookAtA;
        }
    }

    public void SetVertex(VertexHandler vertex1, VertexHandler vertex2)
    {
        this.vertex1 = vertex1;
        this.vertex2 = vertex2;
    }
    
#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlaying) return;
        lineRenderer = GetComponent<LineRenderer>();
    }
#endif
}
