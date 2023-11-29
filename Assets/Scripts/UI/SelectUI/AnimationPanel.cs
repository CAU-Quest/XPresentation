using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;


public enum PanelType
{
    Previous = -1,
    Current = 0,
    Next = 1
}
public class AnimationPanel : MonoBehaviour,IUserInterfaceObserver
{
    public AnimationSliderButton[] buttons;

    [SerializeField] private PanelType _panelType = PanelType.Current;

    public GameObject cannotUsePanel;
    public IPresentationObject selectedObject;
    
    #region Observer Handler
    
    void Start()
    {
        XRSelector.Instance.RegisterObserver(this);
    }

    private void OnDestroy()
    {
        XRSelector.Instance.RemoveObserver(this);
        
    }
    
    public void ObserverObjectUpdate(IPresentationObject presentationObject)
    {
        selectedObject = presentationObject;
        if (presentationObject is PresentationObject)
        {
            List<SlideObjectData> slideObjectDatas = ((PresentationObject)presentationObject).slideData;

            int index = MainSystem.Instance.currentSlideNum;
            index += (int)_panelType;
            if (index < 0 || index >= MainSystem.Instance.GetSlideCount())
            {
                cannotUsePanel.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = false;
                }
            }
            else
            {
                cannotUsePanel.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = true;
                    buttons[i].SetSlideObjectData(slideObjectDatas[index], index);
                    buttons[i].InitProperty((PresentationObject)presentationObject);
                }
            }
        }
    }

    public void ObserverSlideUpdate(int currentSlideNumber)
    {
        if (selectedObject is PresentationObject)
        {
            List<SlideObjectData> slideObjectDatas = ((PresentationObject)selectedObject).slideData;

            int index = currentSlideNumber;
            index += (int)_panelType;
            if (index < 0 || index >= MainSystem.Instance.GetSlideCount())
            {
                cannotUsePanel.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = false;
                }
            }
            else
            {
                cannotUsePanel.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].canUse = true;
                    buttons[i].SetSlideObjectData(slideObjectDatas[index], index);
                    buttons[i].InitProperty((PresentationObject)selectedObject);
                }
            }
        }
    }
    #endregion

}
