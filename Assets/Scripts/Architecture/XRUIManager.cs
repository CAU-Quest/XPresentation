using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRUIManager : MonoBehaviour
{
    public static XRUIManager Instance = null;

    [SerializeField]
    private ColorPickerCanvas colorPicker;

    public float distance = 0.5f;
    public float verticalOffset = -0.5f;
    
    
    public Transform positionSource;
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

    public void OpenColorPicker(Renderer renderer)
    {
        colorPicker.SetRenderer(renderer);
        
        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;

        colorPicker.transform.position = targetPosition;
        colorPicker.ShowColorPicker();
        
        colorPicker.gameObject.SetActive(true);
    }
}
