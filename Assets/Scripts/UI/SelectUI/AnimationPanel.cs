using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Serialization;


public enum PanelType
{
    Previous = -1,
    Current = 0,
    Next = 1
}
public class AnimationPanel : MonoBehaviour,IUserInterfaceObserver
{
    public AnimationSliderButton[] buttons;

    public PanelType panelType = PanelType.Current;

    public GameObject cannotUsePanel;
    public IPresentationObject selectedObject;

    public TextMeshProUGUI slideLabel;
    public SlideToggle slideToggle;
    
    #region Observer Handler
    
    void Start()
    {
        XRSelector.Instance.RegisterObserver(this);
    }

    private void OnDestroy()
    {
        XRSelector.Instance.RemoveObserver(this);
    }

    private void OnEnable()
    {
        ObserverSlideUpdate(MainSystem.Instance.currentSlideNum);
    }

    public void ObserverObjectUpdate(IPresentationObject presentationObject)
    {
        selectedObject = presentationObject;
        if (presentationObject is PresentationObject)
        {
            List<SlideObjectData> slideObjectDatas = ((PresentationObject)presentationObject).slideData;

            int index = MainSystem.Instance.currentSlideNum + (int)panelType;
            
            if (index >= 0 && index < MainSystem.Instance.GetSlideCount())
            {
                slideToggle.gameObject.SetActive(true);
                slideToggle.presentationObject = (PresentationObject)presentationObject;
                slideToggle.currentSlideNumber = index;
            }
            else
            {
                slideToggle.gameObject.SetActive(false);
            }

            if (index < 0 || index >= MainSystem.Instance.GetSlideCount() || !slideObjectDatas[index].isVisible)
            {
                cannotUsePanel.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = false;
                }
                slideLabel.text = "";
            }
            else
            {
                slideToggle.toggle.isOn = slideObjectDatas[index].isVisible;
                cannotUsePanel.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = true;
                    buttons[i].SetSlideObjectDataWithIndex(slideObjectDatas[index], index);
                    buttons[i].InitProperty((PresentationObject)presentationObject);
                }
                slideLabel.text = "Slide " + index;
            }
        }
    }

    public void ObserverSlideUpdate(int currentSlideNumber)
    {
        if (selectedObject is PresentationObject)
        {
            List<SlideObjectData> slideObjectDatas = ((PresentationObject)selectedObject).slideData;

            int index = currentSlideNumber + (int)panelType;
            
            if (index >= 0 && index < MainSystem.Instance.GetSlideCount())
            {
                slideToggle.gameObject.SetActive(true);
                slideToggle.presentationObject = (PresentationObject)selectedObject;
                slideToggle.currentSlideNumber = index;
            }
            else
            {
                slideToggle.gameObject.SetActive(false);
            }
            if (index < 0 || index >= MainSystem.Instance.GetSlideCount() || !slideObjectDatas[index].isVisible)
            {
                cannotUsePanel.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = false;
                }
                slideLabel.text = "";
            }
            else
            {
                slideToggle.toggle.isOn = slideObjectDatas[index].isVisible;
                cannotUsePanel.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = true;
                    buttons[i].SetSlideObjectDataWithIndex(slideObjectDatas[index], index);
                    buttons[i].InitProperty((PresentationObject)selectedObject);
                }
                slideLabel.text = "Slide " + index;
            }
        }
    }
    
    public void ObserverSlideObjectDataUpdate()
    {
        if (selectedObject is PresentationObject)
        {
            List<SlideObjectData> slideObjectDatas = ((PresentationObject)selectedObject).slideData;

            int index = MainSystem.Instance.currentSlideNum + (int)panelType;
            
            if (index >= 0 && index < MainSystem.Instance.GetSlideCount())
            {
                slideToggle.gameObject.SetActive(true);
                slideToggle.presentationObject = (PresentationObject)selectedObject;
                slideToggle.currentSlideNumber = index;
            }
            else
            {
                slideToggle.gameObject.SetActive(false);
            }
            if (index < 0 || index >= MainSystem.Instance.GetSlideCount() || !slideObjectDatas[index].isVisible)
            {
                cannotUsePanel.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = false;
                }
                slideLabel.text = "";
            }
            else
            {
                slideToggle.toggle.isOn = slideObjectDatas[index].isVisible;
                cannotUsePanel.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = true;
                    buttons[i].SetSlideObjectData(slideObjectDatas[index]);
                    buttons[i].currentSlideNum = index;
                }
                slideLabel.text = "Slide " + index;
            }
        }
    }
    #endregion

}
