using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPresentationObject
{
    public void SetSlideObjectData(SlideObjectData slideObjectData);
    public SlideObjectData GetSlideObjectData();
    public uint GetID();
}
