using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Color Default = ColorManager.Default;
    public Color Hover = ColorManager.Hover;
    public Color Select = ColorManager.Select;

    public Color SliderDefault = ColorManager.SliderDefault;
    public Color SliderHover = ColorManager.SliderHover;
    public Color SliderSelect = ColorManager.SliderSelect;
    
    public Color ToggleUnselected = ColorManager.ToggleUnselected;
    public Color ToggleSelected = ColorManager.ToggleSelected;
    public Color ToggleUnselectedHover = ColorManager.ToggleUnselectedHover;
    public Color ToggleSelectedHover = ColorManager.ToggleSelectedHover;
    public Color ToggleSelect = ColorManager.ToggleSelect;


    public Camera previewRenderCamera;
    public RenderTexture renderTexture;
    
    private void Update()
    {
        previewRenderCamera.RenderToCubemap(renderTexture);
    }


}
