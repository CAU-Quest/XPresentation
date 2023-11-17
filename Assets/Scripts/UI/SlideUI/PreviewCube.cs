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

    private int previousNumber;

    private Material material;

    public bool isVisible = true;


    void Start()
    {
        material = GetComponentInChildren<MeshRenderer>().material;
    }
    public void SetNumber(int number)
    {
        textNumber.SetText(number.ToString());
    }

    public void SetPreviousNumber(int number)
    {
        previousNumber = number;
    }
    
    public int GetPreviousNumber()
    {
        return previousNumber;
    }

    public void SetInvisible()
    {
        isVisible = false;
        visual.SetActive(false);
    }
    
    public void Setvisible()
    {
        isVisible = true;
        visual.SetActive(true);
    }

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        material.SetTexture("_CubeMap", renderTexture);
    }

    public void Selected()
    {
        
    }

    public void UnSelected()
    {
        
    }
}
