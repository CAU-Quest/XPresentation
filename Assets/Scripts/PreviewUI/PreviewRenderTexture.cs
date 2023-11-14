using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewRenderTexture : MonoBehaviour
{
    public RenderTexture[] renderTextures; // 7개의 Render Texture 배열
    public Camera previewCamera;

    public int currentSlide = 3;
    public int slideCount;

    public int firstRenderTextureNum = 0;
    public int lastRenderTextureNum = 7;

    void Start()
    {
        previewCamera = GetComponent<Camera>();
        slideCount = MainSystem.Instance.GetSlideCount();
        for (int i = 0; i < 7; i++)
        {
            renderTextures[i].Release();
        }
        RenderInitialTexture();
    }

    public void RenderInitialTexture()
    {
        if (renderTextures.Length != 7) return;

        for (int i = 0; i < renderTextures.Length; i++)
        {
            if (currentSlide - 3 + i < 0 || currentSlide - 3 + i >= slideCount) continue;
            
            Debug.Log("Render Texture : " + i);
            previewCamera.enabled = true;
            MainSystem.Instance.GoToSlideByIndex(currentSlide - 3 + i);
            previewCamera.RenderToCubemap(renderTextures[i]);
            previewCamera.enabled = false;
        }

        firstRenderTextureNum = 0;
        lastRenderTextureNum = 7;
        
        previewCamera.enabled = false;
        MainSystem.Instance.GoToSlideByIndex(0);
        
    }


    public void RenderRequiredTexture()
    {
        if (renderTextures.Length != 7) return;

        for (int i = 0; i < renderTextures.Length; i++)
        {
            if (currentSlide - 3 + i < 0 || currentSlide - 3 + i >= slideCount) continue;
            
            Debug.Log("Render Texture : " + i);
            previewCamera.enabled = true;
            previewCamera.targetTexture = renderTextures[i];
            MainSystem.Instance.GoToSlideByIndex(currentSlide - 3 + i);
            previewCamera.Render();
            previewCamera.enabled = false;
        }

        firstRenderTextureNum = currentSlide - 3;
        lastRenderTextureNum = currentSlide + 3;
        
        previewCamera.enabled = false;
        MainSystem.Instance.GoToSlideByIndex(currentSlide);
        
        
    }
}
