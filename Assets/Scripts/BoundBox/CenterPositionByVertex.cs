using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CenterPositionByVertex : MonoBehaviour
{
    
    public VertexHandler[] vertexList;
    private Vector3[] positions;
    
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

}
