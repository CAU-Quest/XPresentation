using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPresentationObject
{
    public void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale);
    public Vector3 GetPosition();
    public Quaternion GetRotation();
    public uint GetID();
}
