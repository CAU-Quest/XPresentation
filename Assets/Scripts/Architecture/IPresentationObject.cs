using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPresentationObject
{
    public void UpdateCurrentObjectDataInSlide(); //last name : SaveTransformToSlide
    public void ApplyDataToSlide(SlideObjectData data); //new : SaveTransformToSlide -> UpdateCurrentObjectDataInSlide, ApplyDataToSlide
    public void ApplyDataToObject(SlideObjectData data); //last name : SetSlideObjectData
}
