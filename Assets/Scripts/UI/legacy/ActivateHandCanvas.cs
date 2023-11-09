using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.XR.Interaction.Toolkit;

public class ActivateHandCanvas : MonoBehaviour
{
    public GameObject leftUI;
    public GameObject rightUI;

    //public InputActionProperty leftXButton;
    //public InputActionProperty rightBButton;

    private bool leftUiOpen = false;
    private bool rightUiOpen = false;


    protected void OnEnable()
    {
        //leftXButton.action.performed += ctx => leftUiOpen = !leftUiOpen;
        //rightBButton.action.performed += ctx => rightUiOpen = !rightUiOpen;
    }
    void Update()
    {
        leftUI.SetActive(leftUiOpen);
        rightUI.SetActive(rightUiOpen);
    }
}
