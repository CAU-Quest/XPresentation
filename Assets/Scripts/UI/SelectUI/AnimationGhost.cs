using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType
{
    Previous,
    Next
}

public class AnimationGhost : MonoBehaviour
{

    public GhostType ghostType;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private Mesh mesh;

    public Vector3 initialScale = Vector3.one;

    public void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public void SetMesh(Mesh mesh)
    {
        meshFilter.mesh = mesh;
        this.mesh = mesh;
    }

    public void SetVisible()
    {
        meshRenderer.enabled = true;
    }
    
    public void SetInvisible()
    {
        meshRenderer.enabled = false;
    }
    
    public void ApplySlideObjectData(SlideObjectData slideObjectData)
    {
        transform.SetPositionAndRotation(slideObjectData.position, slideObjectData.rotation);
        transform.localScale = Vector3.Scale(slideObjectData.scale, initialScale);
        if (meshRenderer != null) meshRenderer.material.color = slideObjectData.color;
        if (slideObjectData.isVisible)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
