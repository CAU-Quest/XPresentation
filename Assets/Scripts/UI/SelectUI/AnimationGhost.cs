using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GhostType
{
    Previous = -1,
    Next = 1
}

public enum RendererType
{
    Mesh,
    Image,
    Text
}

public class AnimationGhost : MonoBehaviour
{
    public GhostType ghostType;
    
    [Header("Mesh")]
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public LineRenderer lineRenderer;

    [Header("Image")]
    public Image image;
    
    [Header("Text")]
    public InputField inputField;

    
    public PresentationObject selectedObject;
    public GameObject canvas;
    public TextMeshProUGUI SlideNumberText;
    
    private Mesh mesh;

    public Vector3 initialScale = Vector3.one;

    public void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    public void SetRendererType(RendererType rendererType)
    {
        switch (rendererType)
        {
            case RendererType.Mesh:
                break;
            case RendererType.Image:
                break;
            case RendererType.Text:
                break;
        }
    }
    
    public void SetMesh(Mesh mesh)
    {
        meshFilter.mesh = mesh;
        this.mesh = mesh;
    }

    public void SetVisible()
    {
        meshRenderer.enabled = true;
        canvas.SetActive(true);
    }
    
    public void SetInvisible()
    {
        meshRenderer.enabled = false;
        canvas.SetActive(false);
    }
    
    public void ApplySlideObjectData(SlideObjectData slideObjectData)
    {
        transform.SetPositionAndRotation(slideObjectData.position, slideObjectData.rotation);
        transform.localScale = Vector3.Scale(slideObjectData.scale, initialScale);
        if (meshRenderer != null) meshRenderer.material.color = slideObjectData.color;
        if (slideObjectData.isVisible)
        {
            meshRenderer.enabled = true;
            canvas.SetActive(true);
        }
        else
        {
            meshRenderer.enabled = false;
            canvas.SetActive(false);
        }
    }

    private void Update()
    {
        int index = MainSystem.Instance.currentSlideNum + (int)ghostType;
        if(selectedObject)
            lineRenderer.enabled = (selectedObject.slideData[MainSystem.Instance.currentSlideNum].isVisible && meshRenderer.enabled);
        SlideNumberText.text = index.ToString();
        if (selectedObject)
        {
            if (ghostType == GhostType.Previous)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, selectedObject.transform.position);
            }
            else
            {
                lineRenderer.SetPosition(0, selectedObject.transform.position);
                lineRenderer.SetPosition(1, transform.position);
            }
        }
    }
}
