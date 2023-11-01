using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    public void Select()
    {
        XRSelector.Instance.selectedObject = gameObject;
        XRSelector.Instance.SetComponent();
    }
}
