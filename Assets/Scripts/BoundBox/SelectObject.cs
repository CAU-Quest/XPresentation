using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{

    public void Unselect()
    {
        int lineLength = XRSelector.Instance.lineList.Length;
        int vertexLength = XRSelector.Instance.vertexList.Length;

        for (int i = 0; i < lineLength; i++)
        {
            XRSelector.Instance.lineList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < vertexLength; i++)
        {
            XRSelector.Instance.vertexList[i].gameObject.SetActive(false);
        }
    }
    public void Select()
    {
        XRSelector.Instance.transformByVertexHandler.enabled = false;
        XRSelector.Instance.centerPositionByVertex.enabled = false;
        XRSelector.Instance.boundBox.enabled = false;
        XRSelector.Instance.selectedObject = gameObject;
        XRSelector.Instance.SetComponent();
        
        XRSelector.Instance.transform.rotation = transform.rotation;
        
        int lineLength = XRSelector.Instance.lineList.Length;
        int vertexLength = XRSelector.Instance.vertexList.Length;

        for (int i = 0; i < lineLength; i++)
        {
            XRSelector.Instance.lineList[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < vertexLength; i++)
        {
            XRSelector.Instance.vertexList[i].gameObject.SetActive(true);
        }
    }
}
