using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeCurveEditor : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform leftBottom;
    public Transform rightTop;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, leftBottom.position);
        lineRenderer.SetPosition(1, rightTop.position);
    }
}
