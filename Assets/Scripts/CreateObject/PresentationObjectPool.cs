using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PresentationObjectPool : MonoBehaviour
{
    public static PresentationObjectPool Instance = null;
    
    public GameObject[] prefabs; 
    List<GameObject>[] pools;
    public GameObject objectsPool;
    void Start()
    {
        if (null == Instance)
        {
            Instance = this;
            pools = new List<GameObject>[prefabs.Length];

            for (int index = 0; index < pools.Length; index++)
                pools[index] = new List<GameObject>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject Get(int index, Vector3 position)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.transform.position = position;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], position, new quaternion(0, 0, 0, 0));
            pools[index].Add(select);
        }

        return select;
    }

    public GameObject Get(int index, Vector3 position, Transform parent)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.transform.position = position;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], position, new quaternion(0, 0, 0, 0), parent);
            pools[index].Add(select);
        }

        return select;
    }

    public void Clear(int index)
    {
        foreach (GameObject item in pools[index])
            item.SetActive(false);
    }

    public void ClearAll()
    {
        SelectObject[] objects = objectsPool.GetComponentsInChildren<SelectObject>(true);
        for (int index = 0; index < pools.Length; index++)
        {
            pools[index].Clear();
        }

        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i].gameObject);
        }
    }
}
