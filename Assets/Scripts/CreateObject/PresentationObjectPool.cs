using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PresentationObjectPool : MonoBehaviour
{
    public static PresentationObjectPool Instance = null;

    public SaveData saveData;
    
    public GameObject[] prefabs; 
    List<GameObject>[] pools;

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
        for (int index = 0; index < pools.Length; index++)
            foreach (GameObject item in pools[index])
                item.SetActive(false);
    }
}
