using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlide
{
    public TransformData GetObjectData(int id);
    public XRIAnimation GetAnimation(int id);
    public void AddObjectData(int id, TransformData transform);
    public void AddAnimation(int id, XRIAnimation animation);
}
