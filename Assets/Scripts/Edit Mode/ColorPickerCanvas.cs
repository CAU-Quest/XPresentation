using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerCanvas : MonoBehaviour
{
    //private Canvas canvas;
    [SerializeField]
    private Renderer renderer;

    void Start()
    {
        //canvas = GetComponent<Canvas>();
    }
    
    public void SetRenderer(Renderer renderer)
    {
        this.renderer = renderer;
        renderer.sharedMaterial = renderer.material;
    }
    
    public void ShowColorPicker()
    {
        ColorPicker.Create(renderer.sharedMaterial.color, "Choose the cube's color!", SetColor, ColorFinished, true);
    }
    private void SetColor(Color currentColor)
    {
        renderer.sharedMaterial.color = currentColor;
    }

    private void ColorFinished(Color finishedColor)
    {
        Debug.Log("You chose the color " + ColorUtility.ToHtmlStringRGBA(finishedColor));
        gameObject.SetActive(false);
    }
}
