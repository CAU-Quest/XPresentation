using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SnapListController : MonoBehaviour
{
    [SerializeField]
    private SlideListSnapPoseDelegate listSnapPoseDelegate;

    private SnapInteractable snapInteractable;

    public GameObject previewCube;

    public List<PreviewCube> previewCubeList = new List<PreviewCube>();
    public List<PreviewCube> SortedList = new List<PreviewCube>();

    public PreviewCube selectingPreviewCube;
    
    public List<RenderTexture> renderTextureList = new List<RenderTexture>();
    [SerializeField] public int currentSlideNumber = 0;
    [SerializeField] private int beforeSlideNumber;

    public Camera previewRenderCamera;

    private bool isInitialized = false;
    
    // Start is called before the first frame update
    
    void Start()
    {
        //listSnapPoseDelegate = GetComponentInChildren<SlideListSnapPoseDelegate>();
        snapInteractable = GetComponentInChildren<SnapInteractable>();
        beforeSlideNumber = currentSlideNumber;
        //SetTextureToPreviewCube();
        SetNumberToPreviewCube();
        RenderAllTexture();
    }
    
    public void SetInitialNumber()
    {
        int count = 0;

        
        if (currentSlideNumber < 2) count += 2 - currentSlideNumber;
        SortedList.Sort((PreviewCube p1, PreviewCube p2) => p1.transform.localPosition.x.CompareTo(p2.transform.localPosition.x));
        for (int i = 0; i < SortedList.Count; i++)
        {
            if (currentSlideNumber - 3 + i < 0)
            {
                SortedList[i].SetNumber(0);
                SortedList[i].previousNumber = 0;
                SortedList[i].SetInvisible();
            }
            else if (currentSlideNumber - 3 + i >= MainSystem.Instance.GetSlideCount())
            {
                SortedList[i].SetNumber(MainSystem.Instance.GetSlideCount() - 1);
                SortedList[i].previousNumber = MainSystem.Instance.GetSlideCount() - 1;
                SortedList[i].SetInvisible();
            }
            else
            {
                SortedList[i].Setvisible();
            }

            if (SortedList[i].isVisible)
            {
                SortedList[i].SetNumber(currentSlideNumber - 3 + i);
                SortedList[i].previousNumber = currentSlideNumber - 3 + i;
            }
        }
    }

    void LateUpdate()
    {
        if (!isInitialized)
        {
            Debug.Log("Init");
            SetInitialNumber();
            RenderAllTexture();
            isInitialized = true;
        }/*
        if (currentSlideNumber != beforeSlideNumber)
        {
            beforeSlideNumber = currentSlideNumber;
            RenderAllTexture();
        }
        SetTextureToPreviewCube();*/
    }

    public void SetPreviousNumberToPreviewCube()
    {
        for (int i = 0; i < previewCubeList.Count; i++)
        {
            previewCubeList[i].SetCurrentNumberToPreviousNumber();
        }
    }

    public void SetNumberToPreviewCube()
    {
        int count = 0;
        if (currentSlideNumber < 2) count += 2 - currentSlideNumber;
        SortedList.Sort((PreviewCube p1, PreviewCube p2) => p1.transform.localPosition.x.CompareTo(p2.transform.localPosition.x));
        for (int i = 0; i < 7; i++)
        {
            if (currentSlideNumber - 3 + i < 0)
            {
                SortedList[i].SetNumber(0);
                SortedList[i].previousNumber = 0;
                SortedList[i].SetInvisible();
            }
            else if (currentSlideNumber - 3 + i >= MainSystem.Instance.GetSlideCount())
            {
                SortedList[i].SetNumber(MainSystem.Instance.GetSlideCount() - 1);
                SortedList[i].previousNumber = MainSystem.Instance.GetSlideCount() - 1;
                SortedList[i].SetInvisible();
            }
            else
            {
                SortedList[i].Setvisible();
            }

            if(SortedList[i].isVisible)
                SortedList[i].SetNumber(currentSlideNumber - 3 + i);
        }
    }

    public void SetTextureToPreviewCube()
    {
        for (int i = 0; i < previewCubeList.Count; i++)
        {
            previewCubeList[i].SetRenderTexture(renderTextureList[i]);
        }
    }
    
    public void RenderAllTexture()
    {
        for (int i = 0; i < renderTextureList.Count; i++)
        {
            previewRenderCamera.enabled = true;
            MainSystem.Instance.GoToSlideByIndex(previewCubeList[i].currentNumber);
            previewRenderCamera.RenderToCubemap(renderTextureList[i]);
            previewRenderCamera.enabled = false;
        }
        MainSystem.Instance.GoToSlideByIndex(currentSlideNumber);
    }

    public void GoToSlideByIndex(int index)
    {
        currentSlideNumber = index;
        SetNumberToPreviewCube();
        RenderAllTexture();
    }

    public void SwipeToLeft()
    {
        if (currentSlideNumber > 0)
        {
            PreviewCube element = SortedList[SortedList.Count - 1];
            SortedList.Remove(element);
            listSnapPoseDelegate.UntrackElement(element.snapInteractor.Identifier);
            listSnapPoseDelegate.TrackElement(element.snapInteractor.Identifier, 
                new Pose(transform.position + new Vector3(-0.24f, 0, 0), Quaternion.identity));

            int newIndex = SortedList[0].currentNumber - 1;
            int temp = element.previousNumber;
            element.SetNumber(newIndex);
            SetPreviousNumberToPreviewCube();
            element.previousNumber = temp;
            if (selectingPreviewCube && newIndex != selectingPreviewCube.currentNumber)
            {
                if (newIndex >= 0)
                {
                    element.Setvisible();
                }
                else
                {
                    element.SetInvisible();
                }
            }
            else
            {
                if (newIndex >= 0)
                {
                    element.Setvisible();
                }
                else
                {
                    element.SetInvisible();
                }
            }
        
        
            SortedList.Insert(0, element);
            currentSlideNumber--;
            RenderAllTexture();
        }
    }

    public void SetSelectingPreviewCube(PreviewCube cube)
    {
        selectingPreviewCube = cube;
    }

    public void SwipeToRight()
    {
        if (currentSlideNumber < MainSystem.Instance.GetSlideCount() - 1)
        {
            PreviewCube element = SortedList[0];
            SortedList.Remove(element);
            listSnapPoseDelegate.UntrackElement(element.snapInteractor.Identifier);
            listSnapPoseDelegate.TrackElement(element.snapInteractor.Identifier,
                new Pose(transform.position + new Vector3(0.24f, 0, 0), Quaternion.identity));
        
            int newIndex = SortedList[SortedList.Count - 1].currentNumber + 1;
            int temp = element.previousNumber;
            element.SetNumber(newIndex);
            SetPreviousNumberToPreviewCube();
            element.previousNumber = temp;
            if (selectingPreviewCube && newIndex != selectingPreviewCube.currentNumber)
            {
                if (newIndex < MainSystem.Instance.GetSlideCount())
                {
                    element.Setvisible();
                }
                else
                {
                    element.SetInvisible();
                }
            }
            else
            {
                if (newIndex < MainSystem.Instance.GetSlideCount())
                {
                    element.Setvisible();
                }
                else
                {
                    element.SetInvisible();
                }
            }

            SortedList.Add(element);
            currentSlideNumber++;
            RenderAllTexture();
        }
    }
}
