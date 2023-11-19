using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRAnimationLine : MonoBehaviour    
{
    public LineRenderer lineRenderer;

    public PresentationObject object1;
    public PresentationGhostObject object2;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, object1.transform.position);
        lineRenderer.SetPosition(1, object2.transform.position);
    }
}
