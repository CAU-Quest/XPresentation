using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlide
{
    public SlideObjectData GetObjectData(uint id);
    public XRIAnimation GetAnimation(uint id);
    public void AddObjectData(uint id, SlideObjectData transform);
    public void AddAnimation(uint id, XRIAnimation animation);
}
