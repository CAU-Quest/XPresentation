using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    public void Select()
    {
        XRSelector.Instance.transformByVertexHandler.enabled = false;
        XRSelector.Instance.centerPositionByVertex.enabled = false;
        XRSelector.Instance.boundBox.enabled = false;
        XRSelector.Instance.selectedObject = gameObject;
        XRSelector.Instance.SetComponent();
    }
}
