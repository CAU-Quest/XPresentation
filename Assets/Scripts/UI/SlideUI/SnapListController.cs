using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SnapListController : MonoBehaviour, ISlideObserver
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

    public TextMeshProUGUI totalCount;
    private bool isInitialized = false;
    private PreviewCube _lastHighlightedCube;
    
    // Start is called before the first frame update
    
    void Start()
    {
        //listSnapPoseDelegate = GetComponentInChildren<SlideListSnapPoseDelegate>();
        snapInteractable = GetComponentInChildren<SnapInteractable>();
        beforeSlideNumber = currentSlideNumber;
        MainSystem.Instance.RegisterObserver(this);

        _lastHighlightedCube = SortedList[3];
        totalCount.text = "/" + MainSystem.Instance.GetSlideCount().ToString("0");
        HighlightCenterIndexSupport(true);
        StartCoroutine("InitNumber");
    }

    IEnumerator InitNumber()
    {
        yield return new WaitForSeconds(0.5f);
        SetInitialNumber();
        RenderAllTexture();
        HighlightCenterIndexSupport(true);
    }

    public void ObserverUpdateSlide()
    {
        SetInitialNumber();
        RenderAllTexture();
        HighlightCenterIndexSupport(true);
    }

    public void AddSlide()
    {
        MainSystem.Instance.AddSlide();
    }
    
    public void RemoveSlide()
    {
        MainSystem.Instance.RemoveSlide();
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
                StartCoroutine(SortedList[i].SetInvisible());
            }
            else if (currentSlideNumber - 3 + i >= MainSystem.Instance.GetSlideCount())
            {
                SortedList[i].SetNumber(MainSystem.Instance.GetSlideCount() - 1);
                SortedList[i].previousNumber = MainSystem.Instance.GetSlideCount() - 1;
                StartCoroutine(SortedList[i].SetInvisible());
            }
            else
            {
                StartCoroutine(SortedList[i].SetVisible());
            }

            if (SortedList[i].isVisible)
            {
                SortedList[i].SetNumber(currentSlideNumber - 3 + i);
                SortedList[i].previousNumber = currentSlideNumber - 3 + i;
            }
        }
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
                StartCoroutine(SortedList[i].SetInvisible(0f));
            }
            else if (currentSlideNumber - 3 + i >= MainSystem.Instance.GetSlideCount())
            {
                SortedList[i].SetNumber(MainSystem.Instance.GetSlideCount() - 1);
                SortedList[i].previousNumber = MainSystem.Instance.GetSlideCount() - 1;
                StartCoroutine(SortedList[i].SetInvisible(0f));
            }
            else
            {
                StartCoroutine(SortedList[i].SetVisible(0f));
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
    
    /*
    public void GoToSlideByIndex(int index)
    {
        StartCoroutine(Swipe(index));
    }
*/
    private IEnumerator Swipe(int index)
    {
        var waitForSeconds = new WaitForSeconds(0.1f);

        Debug.Log("START");
        while (currentSlideNumber + 1 < index || currentSlideNumber - 1 > index)
        {
            Debug.Log(currentSlideNumber +","+index);
            if (currentSlideNumber > index)
            {
                SwipeToLeft();
            }
            else
            {
                SwipeToRight();
            }
            SetNumberToPreviewCube();
            RenderAllTexture();
            yield return waitForSeconds;
        }
        Debug.Log(currentSlideNumber +","+index+"FIN");
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

            if (selectingPreviewCube)
            {
                if (newIndex >= 0 /*&& newIndex != selectingPreviewCube.currentNumber*/)
                {
                    StartCoroutine(element.SetVisible());
                }
                else
                {
                    StartCoroutine(element.SetInvisible(0f));
                }
            }
            else
            {
                if (newIndex >= 0)
                {
                    StartCoroutine(element.SetVisible());
                }
                else
                {
                    StartCoroutine(element.SetInvisible(0f));
                }
            }
        
            SortedList.Insert(0, element);
            currentSlideNumber--;
            RenderAllTexture();
        }

        if(!selectingPreviewCube) HighlightCenterIndexSupport(true);
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

            if (selectingPreviewCube)
            {
                if (newIndex < MainSystem.Instance.GetSlideCount()/* && newIndex != selectingPreviewCube.currentNumber*/)
                {
                    StartCoroutine(element.SetVisible());
                }
                else
                {
                    StartCoroutine(element.SetInvisible(0f));
                }
            }
            else
            {
                if (newIndex < MainSystem.Instance.GetSlideCount())
                {
                    StartCoroutine(element.SetVisible());
                }
                else
                {
                    StartCoroutine(element.SetInvisible(0f));
                }
            }

            SortedList.Add(element);
            currentSlideNumber++;
            RenderAllTexture();
        }
        
        if(!selectingPreviewCube) HighlightCenterIndexSupport(true);
    }

    public void HighlightCenterIndexSupport(bool isTrue)
    {
        totalCount.DOFade(0f, 0.2f);
        _lastHighlightedCube.indexSupport.DOScale( 0.02f, 0.2f);
        _lastHighlightedCube.indexRenderer.material.DOColor(ColorManager.Default, 0.2f);
        
        if (!isTrue) return;
        totalCount.DOKill();
        totalCount.DOFade(1f, 0.2f);
        SortedList[3].indexSupport.DOScale(0.023f, 0.2f);
        SortedList[3].indexRenderer.material.DOColor(ColorManager.Select, 0.2f);
            
        _lastHighlightedCube = SortedList[3];
    }
}
