using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideToggle : MonoBehaviour
{
    public Toggle toggle;
    public PresentationObject presentationObject;
    public int currentSlideNumber;

    public void Toggle()
    {
        toggle.isOn = !toggle.isOn;

        SlideObjectData so = presentationObject.GetSlideObjectDataByIndex(currentSlideNumber);
        so.isVisible = toggle.isOn;
        presentationObject.ApplyDataToSlideWithIndex(so, currentSlideNumber);
        XRSelector.Instance.NotifySlideObjectDataChangeToObservers();
    }
}
