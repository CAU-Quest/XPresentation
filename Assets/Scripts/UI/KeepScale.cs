using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepScale : MonoBehaviour
{
    void Update()
    {
        var targetScale = transform.parent.localScale;
        transform.localScale = new Vector3(1 / (targetScale.x + 0.001f), 1 / (targetScale.y + 0.001f), 1 / (targetScale.z + 0.001f));
    }
}
