using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CenterPositionByVertex : MonoBehaviour
{
    
    public VertexHandler[] vertexList;
    private Vector3[] positions;
    
    
    private void SetVertex() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        if (XRSelector.Instance.GetVertexList() != null)
            vertexList = XRSelector.Instance.GetVertexList();
    }
    
    public void CenterPosition()
    {
        int count = transform.childCount;
        positions = new Vector3[count];
        
        for (int i = 0; i < count; i++)
        {
            positions[i] = transform.GetChild(i).position;
        }

        Vector3 center = Vector3.zero;

        for (int i = 0; i < 8; i++)
        {
            center += vertexList[i].transform.position;
        }

        transform.position = center / 8f;
        
        for (int i = 0; i < count; i++)
        {
            transform.GetChild(i).position = positions[i];
        }
    }

    void OnEnable()
    {
        SetVertex();
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlaying) return;
        if (XRSelector.Instance && XRSelector.Instance.centerPositionByVertex && XRSelector.Instance.centerPositionByVertex == this) enabled = true;
        else
        {
            enabled = false;
            return;
        }
        SetVertex();
    }
#endif
}
