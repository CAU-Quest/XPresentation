using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PreviewCube : MonoBehaviour
{
    public SnapListController snapListController;
    public SnapInteractor snapInteractor;

    public TextMeshProUGUI textNumber;

    public GameObject visual;
    public Transform cube;

    public int previousNumber;
    public int currentNumber;

    [SerializeField] private Material material;
    [SerializeField] private GrabInteractable grabInteractable;

    public bool isVisible = true;
    
    public Transform indexSupport;
    [HideInInspector] public MeshRenderer indexRenderer;
    private WaitForSeconds _waitForSeconds;

    void Awake()
    {
        _waitForSeconds = new WaitForSeconds(0.2f);
        indexRenderer = indexSupport.GetComponent<MeshRenderer>();
        material = GetComponentInChildren<MeshRenderer>(true).material;
        grabInteractable = GetComponentInChildren<GrabInteractable>(true);
    }
    public void SetNumber(int number)
    {
        currentNumber = number;
        textNumber.SetText(number.ToString());
    }

    public void GoToCurrentNumberSlide()
    {
        snapListController.GoToSlideByIndex(currentNumber);
    }

    public void SetCurrentNumberToPreviousNumber()
    {
        previousNumber = currentNumber;
    }
    public IEnumerator SetInvisible(float time = 0.2f)
    {
        isVisible = false;
        if (time > 0f)
        {
            cube.transform.localScale = Vector3.one;
            cube.DOScale(0f, 0.2f).SetEase(Ease.OutCirc);
            yield return _waitForSeconds;
        }
        visual.SetActive(false);
        grabInteractable.MaxInteractors = 0;
    }
    
    public IEnumerator SetVisible(float time = 0.2f)
    {
        isVisible = true;
        visual.SetActive(true);
        if (time > 0f)
        {
            cube.transform.localScale = new Vector3();
            cube.DOScale(1f, 0.2f).SetEase(Ease.InCirc);
            yield return _waitForSeconds;
        }
        grabInteractable.enabled = true;
        grabInteractable.MaxInteractors = -1;
    }

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        if (!isVisible)
        {
            StartCoroutine(SetVisible(0f));
            material.SetTexture("_CubeMap", renderTexture);
            StartCoroutine(SetInvisible(0f));
        }
        else
        {
            material.SetTexture("_CubeMap", renderTexture);
        }
    }

    public void Selected()
    {
        snapListController.SortedList.Remove(this);
        snapListController.SetSelectingPreviewCube(this);
        snapListController.HighlightCenterIndexSupport(false);
        indexSupport.gameObject.SetActive(false);
    }

    public void UnSelected()
    {
        snapListController.SortedList.Add(this);
        snapListController.SetSelectingPreviewCube(null);
        snapListController.SetNumberToPreviewCube();
        snapListController.HighlightCenterIndexSupport(true);
        indexSupport.gameObject.SetActive(true);
        
        Debug.Log("Previous Number : " + previousNumber + " -> Current Number : " + currentNumber);
        if (previousNumber != currentNumber)
        {
            if (previousNumber >= 0 && previousNumber < MainSystem.Instance.GetSlideCount() && currentNumber >= 0 &&
                currentNumber < MainSystem.Instance.GetSlideCount())
            {
                MainSystem.Instance.MoveSlideTo(previousNumber, currentNumber);
            }
            snapListController.SetPreviousNumberToPreviewCube();
        }
        snapListController.RenderAllTexture();
        //if (currentNumber >= 0 && currentNumber < MainSystem.Instance.GetSlideCount())
         //   MainSystem.Instance.GoToSlideByIndex(currentNumber);
    }
}
