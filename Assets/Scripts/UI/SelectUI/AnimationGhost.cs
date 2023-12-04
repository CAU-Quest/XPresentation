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

    public RendererType rendererType;
    
    [Header("Mesh")]
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public LineRenderer lineRenderer;

    [Header("Image")]
    public RawImage image;

    [Header("Text")] 
    public TextMeshProUGUI textUI;

    
    public PresentationObject selectedObject;
    public GameObject canvas;
    public TextMeshProUGUI SlideNumberText;
    
    private Mesh mesh;

    public Vector3 initialScale = Vector3.one;

    public void Start()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    public void SetRenderer(RendererType rendererType)
    {
        this.rendererType = rendererType;
    }

    public void SetColor(Color color)
    {
        switch (rendererType)
        {
            case RendererType.Mesh:
                meshRenderer.material.color = color;
                break;
            case RendererType.Image:
                image.color = color;
                break;
            case RendererType.Text:
                textUI.color = color;
                break;
        }
    }

    public void SetText(string text)
    {
        this.textUI.text = text;
    }

    public void SetImage(Texture texture)
    {
        this.image.texture = texture;
    }
    
    public void SetMesh(Mesh mesh)
    {
        meshFilter.mesh = mesh;
        this.mesh = mesh;
    }

    public void SetVisible()
    {
        switch (rendererType)
        {
            case RendererType.Mesh:
                meshRenderer.enabled = true;
                image.enabled = false;
                textUI.enabled = false;
                break;
            case RendererType.Image:
                meshRenderer.enabled = false;
                image.enabled = true;
                textUI.enabled = false;
                break;
            case RendererType.Text:
                meshRenderer.enabled = false;
                image.enabled = false;
                textUI.enabled = true;
                break;
        }
        canvas.SetActive(true);
    }
    
    public void SetInvisible()
    {
        meshRenderer.enabled = false;
        image.enabled = false;
        textUI.enabled = false;
        canvas.SetActive(false);
    }
    
    public void ApplySlideObjectData(SlideObjectData slideObjectData)
    {
        transform.SetPositionAndRotation(slideObjectData.position, slideObjectData.rotation);
        transform.localScale = slideObjectData.scale;
        SetColor(slideObjectData.color);
        if (slideObjectData.isVisible)
        {
            SetVisible();
            canvas.SetActive(true);
        }
        else
        {
            SetInvisible();
            canvas.SetActive(false);
        }
    }

    private void Update()
    {
        int index = MainSystem.Instance.currentSlideNum + (int)ghostType;
        if (selectedObject)
        {
            bool isVisible = selectedObject.slideData[MainSystem.Instance.currentSlideNum].isVisible;
            bool checkRenderer = isVisible && (meshRenderer.enabled || textUI.enabled || image.enabled);
            lineRenderer.enabled = checkRenderer;
        }
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
