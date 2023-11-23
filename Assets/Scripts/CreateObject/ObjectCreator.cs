using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator Instance = null;
    private PresentationObjectPool objectPool;

    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void CreateObject(SelectableAction action)
    {
        Vector3 direction = XRUIManager.Instance.positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 position = XRUIManager.Instance.positionSource.position + direction * XRUIManager.Instance.distance;
        
        switch (action)
        {
            case SelectableAction.Close:
                break;
            case SelectableAction.Sphere:
                objectPool.Get(1, position);
                break;
            case SelectableAction.Cube:
                objectPool.Get(2, position);
                break;
            case SelectableAction.Cylinder:
                objectPool.Get(3, position);
                break;
            case SelectableAction.Plane:
                GameObject go = objectPool.Get(4, position);
                
                break;
            case SelectableAction.Text:
                objectPool.Get(5, position);
                break;
            case SelectableAction.ImportImage:
                objectPool.Get(6, position);
                break;
            case SelectableAction.ImportModel:
                objectPool.Get(7, position);
                break;
        }
    }
}
