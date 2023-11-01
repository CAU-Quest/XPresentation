using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using UnityEditor;
using UnityEngine;

public class XRSelector : MonoBehaviour
{
    public static XRSelector Instance = null;

    public GameObject selectedObject;
    
    private BoundBoxLine[] lineList;
    private VertexHandler[] vertexList;


    public CenterPositionByVertex centerPositionByVertex;
    public BoundBox boundBox;
    public TransformByVertexHandler transformByVertexHandler;
    
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
        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }
        
        centerPositionByVertex = selectedObject.GetComponent<CenterPositionByVertex>();
        boundBox = selectedObject.GetComponent<BoundBox>();
        transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();
        if(boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null)
            Debug.Log("Selected Object doesn't have correct components");
    }

    public void SetComponent()
    {
        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }
        
        centerPositionByVertex = selectedObject.GetComponent<CenterPositionByVertex>();
        boundBox = selectedObject.GetComponent<BoundBox>();
        transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();
        if (boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null)
        {
            Debug.Log("Selected Object doesn't have correct components");
            return;
        }
        
        boundBox.UpdateBounds();
        centerPositionByVertex.CenterPosition();
        boundBox.UpdateBounds();
    }
    
#if UNITY_EDITOR
    public void OnValidate()
    {
        if (EditorApplication.isPlaying) return;
        if (null == Instance)
        {
            Instance = this;
        }

        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }
        centerPositionByVertex = selectedObject.GetComponent<CenterPositionByVertex>();
        boundBox = selectedObject.GetComponent<BoundBox>();
        transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();
        if(boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null)
            Debug.Log("Selected Object doesn't have correct components");
    }
#endif
    
}
