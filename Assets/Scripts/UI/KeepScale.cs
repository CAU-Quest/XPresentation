using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepScale : MonoBehaviour
{
    void Update()
    {
        Vector3 targetScale = transform.parent.localScale;

        transform.localScale = new Vector3(1 / targetScale.x, 1 / targetScale.y, 1 / targetScale.z);
    }
}
