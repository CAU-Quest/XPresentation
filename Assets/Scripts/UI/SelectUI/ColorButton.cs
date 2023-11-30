using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour, ISelectedObjectModifierInitializer
{
    public Image image;
    public PanelType panelType;

    private PresentationObject selectedObject;

    public void InitProperty(PresentationObject selectedObject)
    {
        int index = MainSystem.Instance.currentSlideNum + (int)panelType;
        if(index >= 0 && index < MainSystem.Instance.GetSlideCount())
            image.color = selectedObject.slideData[index].color;
        this.selectedObject = selectedObject;
    }

    public void OpenColorPicker()
    {
        ColorPicker.Create(image.color, "", SetColor, SetColor, true);
    }

    public void SetColor(Color color)
    {
        int index = MainSystem.Instance.currentSlideNum + (int)panelType;
        SlideObjectData so = selectedObject.slideData[index];
        so.color = color;
        
        selectedObject.ApplyDataToSlideWithIndex(so, index);
        XRSelector.Instance.NotifySlideObjectDataChangeToObservers();
    }
}
