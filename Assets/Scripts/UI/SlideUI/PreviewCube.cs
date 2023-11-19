using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;

public class PreviewCube : MonoBehaviour
{
    public SnapListController snapListController;
    public SnapInteractor snapInteractor;

    public TextMeshProUGUI textNumber;

    public GameObject visual;

    public int previousNumber;
    public int currentNumber;

    [SerializeField] private Material material;
    [SerializeField] private GrabInteractable grabInteractable;

    public bool isVisible = true;


    void Awake()
    {
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
    public void SetInvisible()
    {
        isVisible = false;
        visual.SetActive(false);
        grabInteractable.MaxInteractors = 0;
    }
    
    public void Setvisible()
    {
        isVisible = true;
        visual.SetActive(true);
        grabInteractable.enabled = true;
        grabInteractable.MaxInteractors = -1;
    }

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        if (!isVisible)
        {
            Setvisible();
            material.SetTexture("_CubeMap", renderTexture);
            SetInvisible();
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
    }

    public void UnSelected()
    {
        snapListController.SortedList.Add(this);
        snapListController.SetNumberToPreviewCube();

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
