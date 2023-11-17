using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEditor;
using UnityEngine;

public class SnapListController : MonoBehaviour
{
    [SerializeField]
    private SlideListSnapPoseDelegate listSnapPoseDelegate;

    private SnapInteractable snapInteractable;

    public GameObject previewCube;

    public List<PreviewCube> previewCubeList = new List<PreviewCube>();
    public List<RenderTexture> renderTextureList = new List<RenderTexture>();

    public int currentSlideNumber = 0;
    private int beforeSlideNumber;

    public Camera previewRenderCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        //listSnapPoseDelegate = GetComponentInChildren<SlideListSnapPoseDelegate>();
        snapInteractable = GetComponentInChildren<SnapInteractable>();
        beforeSlideNumber = currentSlideNumber;
        previewCubeList.Sort((PreviewCube p1, PreviewCube p2) => p1.transform.localPosition.x.CompareTo(p2.transform.localPosition.x));
        for (int i = 0; i < 7; i++)
        {
            previewCubeList[i].SetNumber(i + 1);
            previewCubeList[i].SetPreviousNumber(i + 1);
            if (currentSlideNumber - 3 + i < 0 || currentSlideNumber - 3 + i >= MainSystem.Instance.GetSlideCount())
            {
                previewCubeList[i].SetInvisible();
            }
            else
            {
                previewCubeList[i].Setvisible();
            }
        }
        RenderAllTexture();
        SetTextureToPreviewCube();
    }

    void Update()
    {
        
        int count = 0;
        if (currentSlideNumber < 2) count += 2 - currentSlideNumber;
        previewCubeList.Sort((PreviewCube p1, PreviewCube p2) => p1.transform.localPosition.x.CompareTo(p2.transform.localPosition.x));
        for (int i = 0; i < 7; i++)
        {
            if (currentSlideNumber - 3 + i < 0 || currentSlideNumber - 3 + i >= MainSystem.Instance.GetSlideCount())
            {
                previewCubeList[i].SetInvisible();
            }
            else
            {
                previewCubeList[i].Setvisible();
            }

            if(previewCubeList[i].isVisible)
                previewCubeList[i].SetNumber(currentSlideNumber + count++ - 2);
        }
        if (currentSlideNumber != beforeSlideNumber)
        {
            beforeSlideNumber = currentSlideNumber;
            RenderAllTexture();
        }
        SetTextureToPreviewCube();
    }

    public void SetTextureToPreviewCube()
    {
        for (int i = 0; i < 7; i++)
        {
            if (currentSlideNumber - 3 + i < 0 || currentSlideNumber - 3 + i >= MainSystem.Instance.GetSlideCount()) continue;
            if(previewCubeList[i].isVisible) previewCubeList[i].SetRenderTexture(renderTextureList[i]);
        }
    }

    public void RenderAllTexture()
    {
        
        for (int i = 0; i < renderTextureList.Count; i++)
        {
            if (currentSlideNumber - 3 + i < 0 || currentSlideNumber - 3 + i >= MainSystem.Instance.GetSlideCount()) continue;
            
            previewRenderCamera.enabled = true;
            MainSystem.Instance.GoToSlideByIndex(currentSlideNumber - 3 + i);
            previewRenderCamera.RenderToCubemap(renderTextureList[i]);
            previewRenderCamera.enabled = false;
        }
        MainSystem.Instance.GoToSlideByIndex(currentSlideNumber);

    }

    public void CheckMessage()
    {
        Debug.Log("hello");
    }
    
}
