using System.Collections;
using System.Collections.Generic;
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

    public void UpdateVertex()
    {
        handler.MoveVertex(left, top, front, transform.localPosition);
    }
}
