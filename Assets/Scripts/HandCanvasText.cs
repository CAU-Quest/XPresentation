using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandCanvasText : MonoBehaviour, ISystemObserver
{
    public TextMeshProUGUI tmpSlideNum;
    public TextMeshProUGUI tmpMode;

    public void ObserverUpdateMode(int mode)// 0 - Edit Mode, 1 - Deploy Mode, 2 - Slide Mode, 3 - Animation Mode
    {
        tmpMode.text = "Mode : ";
        switch (mode)
        {
            case 0:
                tmpMode.text += "Edit Mode";
                break;
            case 1:
                tmpMode.text += "Deploy Mode";
                break;
            case 2:
                tmpMode.text += "Slide Mode";
                break;
            case 3:
                tmpMode.text += "Animation Mode";
                break;
        }
    }
    
    public void ObserverUpdateSlide(int slide)
    {
        tmpSlideNum.text = "Current Slide : " + slide;
    }

    public void ObserverRemoveSlide(int index)
    {
    }

    public void ObserverAddSlide()
    {
        
    }

    public void Start()
    {
        MainSystem.Instance.RegisterObserver(this);
    }

}
