
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
public class AnimationPanel : MonoBehaviour,IUserInterfaceObserver, ISelectedObjectModifierInitializer
{
    public AnimationSliderButton[] buttons;

    public PanelType panelType = PanelType.Current;

    public GameObject cannotUsePanel;
    public SelectEasingButton selectEasingButton;
    public GameObject animationPlayToggleButton;
    public IPresentationObject selectedObject;

    public TextMeshProUGUI slideLabel;
    public ToggleSlideButton toggleSlideButton;

    public XRAnimation xrAnimation;
    
    #region Observer Handler
    
    void Start()
    {
        XRSelector.Instance.RegisterObserver(this);
        toggleSlideButton.selectEasingButton = selectEasingButton;
    }

    private void OnDestroy()
    {
        if(XRSelector.Instance)
            XRSelector.Instance.RemoveObserver(this);
    }

    private void OnEnable()
    {
        if(MainSystem.Instance)
            ObserverSlideUpdate(MainSystem.Instance.currentSlideNum);
    }

    public void InitializeProperty(PresentationObject newSelectedObject)
    {
        ObserverObjectUpdate(newSelectedObject);
        if (selectEasingButton) selectEasingButton.CloseCanvas();
        if (panelType == PanelType.Previous)
        {
            if(MainSystem.Instance.currentSlideNum - 1 >= 0) xrAnimation = newSelectedObject.animationList[MainSystem.Instance.currentSlideNum - 1];
        } 
        else if (panelType == PanelType.Next)
        {
            if(MainSystem.Instance.currentSlideNum < MainSystem.Instance.GetSlideCount()) xrAnimation = newSelectedObject.animationList[MainSystem.Instance.currentSlideNum];
        }
    }

    public void FinalizeProperty() { }


    public void ObserverObjectUpdate(IPresentationObject presentationObject)
    {
        selectedObject = presentationObject;
        if (presentationObject is PresentationObject)
        {
            List<SlideObjectData> slideObjectDatas = ((PresentationObject)presentationObject).slideData;

            int index = MainSystem.Instance.currentSlideNum + (int)panelType;
            
            if (index >= 0 && index < MainSystem.Instance.GetSlideCount())
            {
                toggleSlideButton.gameObject.SetActive(true);
                toggleSlideButton.presentationObject = (PresentationObject)presentationObject;
                toggleSlideButton.currentSlideNumber = index;
            }
            else
            {
                toggleSlideButton.SetActive(false);
                toggleSlideButton.gameObject.SetActive(false);
            }

            if (index < 0 || index >= MainSystem.Instance.GetSlideCount() || !slideObjectDatas[index].isVisible)
            {
                cannotUsePanel.SetActive(true);
                if(selectEasingButton) selectEasingButton.gameObject.SetActive(false);
                if(animationPlayToggleButton) animationPlayToggleButton.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = false;
                }
                toggleSlideButton.SetActive(false);
                slideLabel.text = (index < 0 || index >= MainSystem.Instance.GetSlideCount()) ? "(Slide not exists)" : "(No object in this Slide)";
            }
            else
            {
                toggleSlideButton.SetActive(slideObjectDatas[index].isVisible);
                cannotUsePanel.SetActive(false);
                if (selectEasingButton)
                {
                    selectEasingButton.gameObject.SetActive(true);
                    selectEasingButton.InitializeProperty((PresentationObject)selectedObject);
                }
                if(animationPlayToggleButton) animationPlayToggleButton.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = true;
                    buttons[i].SetSlideObjectDataWithIndex(slideObjectDatas[index], index);
                    buttons[i].InitializeProperty((PresentationObject)presentationObject);
                }
                slideLabel.text = "(Slide " + index + ")";
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
                toggleSlideButton.gameObject.SetActive(true);
                toggleSlideButton.presentationObject = (PresentationObject)selectedObject;
                toggleSlideButton.currentSlideNumber = index;
            }
            else
            {
                toggleSlideButton.gameObject.SetActive(false);
            }
            if (index < 0 || index >= MainSystem.Instance.GetSlideCount() || !slideObjectDatas[index].isVisible)
            {
                cannotUsePanel.SetActive(true);
                if(selectEasingButton) selectEasingButton.gameObject.SetActive(false);
                if(animationPlayToggleButton) animationPlayToggleButton.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = false;
                }
                toggleSlideButton.SetActive(false);
                slideLabel.text = (index < 0 || index >= MainSystem.Instance.GetSlideCount()) ? "(Slide not exists)" : "(No object in this Slide)";
            }
            else
            {
                toggleSlideButton.SetActive(slideObjectDatas[index].isVisible);
                cannotUsePanel.SetActive(false);
                if (selectEasingButton)
                {
                    selectEasingButton.gameObject.SetActive(true);
                    selectEasingButton.InitializeProperty((PresentationObject)selectedObject);
                }
                if(animationPlayToggleButton) animationPlayToggleButton.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = true;
                    buttons[i].SetSlideObjectDataWithIndex(slideObjectDatas[index], index);
                    buttons[i].InitializeProperty((PresentationObject)selectedObject);
                }
                slideLabel.text = "(Slide " + index + ")";
            }
        }
    }

    public void OnDisable()
    {
        if(XRSelector.Instance)
            XRSelector.Instance.DisableAnimationGhost();
    }

    public void ObserverSlideObjectDataUpdate()
    {
        if (selectedObject is PresentationObject)
        {
            List<SlideObjectData> slideObjectDatas = ((PresentationObject)selectedObject).slideData;

            int index = MainSystem.Instance.currentSlideNum + (int)panelType;
            
            
            if (index >= 0 && index < MainSystem.Instance.GetSlideCount())
            {
                if (panelType == PanelType.Previous)
                {
                    XRSelector.Instance.beforeAnimationGhost.SetVisible();
                    XRSelector.Instance.beforeAnimationGhost.ApplySlideObjectData(slideObjectDatas[index]);
                }

                if (panelType == PanelType.Next)
                {
                    XRSelector.Instance.afterAnimationGhost.SetVisible();
                    XRSelector.Instance.afterAnimationGhost.ApplySlideObjectData(slideObjectDatas[index]);
                }
                
                toggleSlideButton.gameObject.SetActive(true);
                toggleSlideButton.presentationObject = (PresentationObject)selectedObject;
                toggleSlideButton.currentSlideNumber = index;
            }
            else
            {
                if (panelType == PanelType.Previous)
                    XRSelector.Instance.beforeAnimationGhost.SetInvisible();
                if (panelType == PanelType.Next)
                    XRSelector.Instance.afterAnimationGhost.SetInvisible();
                
                toggleSlideButton.gameObject.SetActive(false);
            }
            if (index < 0 || index >= MainSystem.Instance.GetSlideCount() || !slideObjectDatas[index].isVisible)
            {
                cannotUsePanel.SetActive(true);
                if(selectEasingButton) selectEasingButton.gameObject.SetActive(false);
                if(animationPlayToggleButton) animationPlayToggleButton.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = false;
                }
                toggleSlideButton.SetActive(false);
                slideLabel.text = (index < 0 || index >= MainSystem.Instance.GetSlideCount()) ? "(Slide not exists)" : "(No object in this Slide)";
            }
            else
            {
                toggleSlideButton.SetActive(slideObjectDatas[index].isVisible);
                cannotUsePanel.SetActive(false);
                if (selectEasingButton)
                {
                    selectEasingButton.gameObject.SetActive(true);
                    selectEasingButton.InitializeProperty((PresentationObject)selectedObject);
                }
                if(animationPlayToggleButton) animationPlayToggleButton.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = true;
                    buttons[i].SetSlideObjectData(slideObjectDatas[index]);
                    buttons[i].currentSlideNum = index;
                }
                slideLabel.text = "(Slide " + index + ")";
            }
        }
    }

    #endregion

}