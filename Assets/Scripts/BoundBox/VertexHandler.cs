using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VertexHandler : MonoBehaviour
{
    [SerializeField]
    private TransformByVertexHandler handler;
    [SerializeField]
    private int top;
    [SerializeField]
    private int front;
    [SerializeField]
    private int left;

    public void SetVertex(int top, int front, int left, TransformByVertexHandler handler)
    {
        this.top = top;
        this.front = front;
        this.left = left;
        this.handler = handler;
    }

    private void LateUpdate()
    {
        Vector3 parentScale = transform.parent.localScale;
        transform.localScale = new Vector3(0.05f / parentScale.x, 0.05f / parentScale.y, 0.05f / parentScale.z);
    }

    void Start()
    {
        handler = transform.parent.gameObject.GetComponentInChildren<TransformByVertexHandler>();
    }
    
    void Reset()
    {
        handler = transform.parent.gameObject.GetComponentInChildren<TransformByVertexHandler>();
    }
    public void UpdateVertex()
    {
        handler.MoveVertex(left, top, front, transform.localPosition);
    }
    
    void OnEnable()
    {
        handler = transform.parent.gameObject.GetComponentInChildren<TransformByVertexHandler>();
    }
    
    
#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlaying) return;
        handler = transform.parent.gameObject.GetComponentInChildren<TransformByVertexHandler>();
    }
#endif
}
