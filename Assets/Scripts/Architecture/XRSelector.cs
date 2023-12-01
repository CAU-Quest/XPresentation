using System;
using System.Collections;
using System.Collections.Generic;
using DimBoxes;
using Oculus.Interaction;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class XRSelector : MonoBehaviour, IUserInterfaceSubject
{
    #region Member Variables

    public static XRSelector Instance = null;

    public List<IUserInterfaceObserver> observers = new List<IUserInterfaceObserver>();
    
    public GameObject selectedObject;
    
    public BoundBoxLine[] lineList;
    public VertexHandler[] vertexList;

    public Object linePrefab;
    public GameObject vertexPrefab;

    private BoundObjectType BoundObjectType = BoundObjectType.ThreeDimension;

    public CenterPositionByVertex centerPositionByVertex;
    public BoundBox boundBox;
    public TransformByVertexHandler transformByVertexHandler;
    public SelectObject selectObject;
    public IPresentationObject presentationObject;
    
    [Header("Line Properties")]
    
    public Material lineMaterial;
    
    [Range(0.005f, 0.25f)] public float lineWidth = 0.03f;
    public Color lineColor = Color.white;
    public int numCapVertices = 0;

    [HideInInspector]
    public bool edgeSelected = false;

    public AnimationGhost beforeAnimationGhost;
    public AnimationGhost afterAnimationGhost;

    #endregion

    #region UI Observer Handling
    public void RegisterObserver(IUserInterfaceObserver observer)
    {
        this.observers.Add(observer);
    }
    
    public void RemoveObserver(IUserInterfaceObserver observer)
    {
        this.observers.Remove(observer);
    }

    
    public void NotifyObjectChangeToObservers()
    {
        if (selectedObject && presentationObject != null)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].ObserverObjectUpdate(presentationObject);
            }
        }
    }
    
    public void NotifySlideChangeToObservers()
    {
        for (int i = 0; i < this.observers.Count; i++)
        {
            this.observers[i].ObserverSlideUpdate(MainSystem.Instance.currentSlideNum);
        }
    }
    
    public void NotifySlideObjectDataChangeToObservers()
    {
        for (int i = 0; i < this.observers.Count; i++)
        {
            this.observers[i].ObserverSlideObjectDataUpdate();
        }
    }
    

    #endregion

    #region Unity Life Cycle
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }
        
        centerPositionByVertex = selectedObject.GetComponent<CenterPositionByVertex>();
        boundBox = selectedObject.GetComponent<BoundBox>();
        transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();
        presentationObject = selectedObject.GetComponentInChildren<PresentationObject>();
        if(boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null || presentationObject == null)
            Debug.Log("Selected Object doesn't have correct components");
        
        SetVertex();
        SetLines();
        SetLineVertex();

    }
    
    public void Start()
    {
        vertexList = GetComponentsInChildren<VertexHandler>(true);
        lineList = GetComponentsInChildren<BoundBoxLine>(true);
        
        for(int i = 0; i < vertexList.Length; i++)
            vertexList[i].gameObject.SetActive(true);
        for(int i = 0; i < lineList.Length; i++)
            lineList[i].gameObject.SetActive(true);
    }


    private void Update()
    {
        if(!selectedObject) return; 
        if(edgeSelected) selectedObject.transform.rotation = transform.rotation;
        else transform.rotation = selectedObject.transform.rotation;
    }

    
    public void Reset()
    {
        if (null == Instance)
        {
            Instance = this;
        }
    }
    
    
#if UNITY_EDITOR
    public void OnValidate()
    {
        /*
        if (EditorApplication.isPlaying) return;
        if (Instance == null)
        {
            Instance = this;
        }

        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }

        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            centerPositionByVertex = selectedObject.GetComponent<CenterPositionByVertex>();
            boundBox = selectedObject.GetComponent<BoundBox>();
            transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();
            presentationObject = selectedObject.GetComponentInChildren<PresentationObject>();
            if(boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null || presentationObject == null)
                Debug.Log("Selected Object doesn't have correct components");
        
        
            SetVertex();
            SetLines();
            SetLineRenderers();
            SetLineVertex();
        } else if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            transformByVertexHandler = selectedObject.GetComponent<TransformByVertexHandler>();
            if(transformByVertexHandler == null)
                Debug.Log("Selected Object doesn't have correct components");
            
            SetVertex();
            SetLines();
            SetLineRenderers();
            SetLineVertex();
        }*/
    }
#endif
    
    
    
    void OnEnable()
    {
        vertexList = GetComponentsInChildren<VertexHandler>(true);
        lineList = GetComponentsInChildren<BoundBoxLine>(true);
        
        VertexHandler[] lrs = GetComponentsInChildren<VertexHandler>(true);
        BoundBoxLine[] lls = GetComponentsInChildren<BoundBoxLine>(true);
        
        for (int i = 0; i < lrs.Length; i++)
        {
            if (!lrs[i].gameObject.activeSelf) DestroyImmediate(lrs[i].gameObject);
        }
        for (int i = 0; i < lls.Length; i++)
        {
            if (!lls[i].gameObject.activeSelf) DestroyImmediate(lls[i].gameObject);
        }
        
        SetVertex();
        SetLines();
        SetLineRenderers();
        SetLineVertex();
    }
    void OnDestroy()
    {
        VertexHandler[] lrs = GetComponentsInChildren<VertexHandler>(true);
        BoundBoxLine[] lls = GetComponentsInChildren<BoundBoxLine>(true);
        
        for (int i = 0; i < lrs.Length; i++)
        {
            DestroyImmediate(lrs[i].gameObject);
        }
        for (int i = 0; i < lls.Length; i++)
        {
            DestroyImmediate(lls[i].gameObject);
        }
    }

    void OnDisable()
    {
        VertexHandler[] lrs = GetComponentsInChildren<VertexHandler>(true);
        BoundBoxLine[] lls = GetComponentsInChildren<BoundBoxLine>(true);

        for (int i = 0; i < lrs.Length; i++)
        {
            lrs[i].enabled = false;
        }

        for (int i = 0; i < lls.Length; i++)
        {
            lls[i].enabled = false;
        }
    }

    #endregion

    #region Vertex & Line Handling

    public void DeactivateBoundBox()
    {
        int lineLength = lineList.Length;
        int vertexLength = vertexList.Length;

        for (int i = 0; i < lineLength; i++)
        {
            lineList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < vertexLength; i++)
        {
            vertexList[i].gameObject.SetActive(false);
        }
    }
    
    public void ActivateBoundBox()
    {
        int lineLength = lineList.Length;
        int vertexLength = vertexList.Length;

        for (int i = 0; i < lineLength; i++)
        {
            lineList[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < vertexLength; i++)
        {
            vertexList[i].gameObject.SetActive(true);
        }
    }

    public void Reselect()
    {
        if(selectedObject)
            selectObject.Select();
    }
    
    private void SetVertex() // { topFrontLeft, topFrontRight, topBackLeft, topBackRight, bottomFrontLeft, bottomFrontRight, bottomBackLeft, bottomBackRight };
    {
        vertexList = GetComponentsInChildren<VertexHandler>(true);
        if (vertexList.Length == 0)
        {
            vertexList = new VertexHandler[8];
            for (int i = 0; i < 8; i++)
            {
#if UNITY_EDITOR
                GameObject go = PrefabUtility.InstantiatePrefab(vertexPrefab) as GameObject;
                go.transform.SetParent(transform);
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
#else
                        GameObject go = (GameObject)Instantiate(vertexPrefab, transform, false);
#endif
                vertexList[i] = go.GetComponent<VertexHandler>();
                vertexList[i].SetVertex(i / 4, (i % 4) / 2, (i % 4 % 2));
            }
        }
        else
        {
            for (int i = 0; i < vertexList.Length; i++)
            {

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(vertexList[i].gameObject) == vertexPrefab)
                    {
                        vertexList[i].enabled = true;
                    }
                    else
                    {
                        vertexList[i].gameObject.SetActive(false);
                        GameObject go = PrefabUtility.InstantiatePrefab(vertexPrefab) as GameObject;
                        go.transform.SetParent(transform);
                        go.transform.position = Vector3.zero;
                        go.transform.rotation = Quaternion.identity;
                        vertexList[i] = go.GetComponent<VertexHandler>();
                    }
                }
#endif
                vertexList[i].enabled = true;
            }
        }
        if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            vertexList[2].gameObject.SetActive(false);
            vertexList[3].gameObject.SetActive(false);
            vertexList[6].gameObject.SetActive(false);
            vertexList[7].gameObject.SetActive(false);
        }
    }

    private void SetLines()
    {
        
        Debug.Log("Start Set Lines");
        lineList = GetComponentsInChildren<BoundBoxLine>(true);
        if (lineList.Length == 0)
        {
            Debug.Log("Create Line List");
            lineList = new BoundBoxLine[12];
            for (int i = 0; i < 12; i++)
            {
#if UNITY_EDITOR
                GameObject go = PrefabUtility.InstantiatePrefab(linePrefab) as GameObject;
                go.transform.SetParent(transform);
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
#else
                GameObject go = (GameObject)Instantiate(linePrefab, transform, false);
#endif
                lineList[i] = go.GetComponent<BoundBoxLine>();
            }
        }
        else
        {
            Debug.Log("LineList.Length != 0");
            for (int i = 0; i < lineList.Length; i++)
            {

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(lineList[i].gameObject) == linePrefab)
                    {
                        lineList[i].enabled = true;
                    }
                    else
                    {
                        lineList[i].gameObject.SetActive(false);
                        GameObject go = PrefabUtility.InstantiatePrefab(linePrefab) as GameObject;
                        go.transform.SetParent(transform);
                        go.transform.position = Vector3.zero;
                        go.transform.rotation = Quaternion.identity;
                        lineList[i] = go.GetComponent<BoundBoxLine>();
                    }
                }
#endif
                lineList[i].enabled = true;
            }
        }
    }


    public void SetLineVertex()
    {
        if (BoundObjectType == BoundObjectType.ThreeDimension)
        {
            lineList[0].SetVertex(vertexList[0], vertexList[1]);
            lineList[1].SetVertex(vertexList[2], vertexList[3]);
            lineList[2].SetVertex(vertexList[4], vertexList[5]);
            lineList[3].SetVertex(vertexList[6], vertexList[7]);

            lineList[0].edgeHandler.Init(OneGrabRotateTransformer.Axis.Right, transform);
            lineList[1].edgeHandler.Init(OneGrabRotateTransformer.Axis.Right, transform);
            lineList[2].edgeHandler.Init(OneGrabRotateTransformer.Axis.Right, transform);
            lineList[3].edgeHandler.Init(OneGrabRotateTransformer.Axis.Right, transform);
        
            lineList[4].SetVertex(vertexList[1], vertexList[5]);
            lineList[5].SetVertex(vertexList[0], vertexList[4]);
            lineList[6].SetVertex(vertexList[2], vertexList[6]);
            lineList[7].SetVertex(vertexList[3], vertexList[7]);
        
            lineList[4].edgeHandler.Init(OneGrabRotateTransformer.Axis.Up, transform);
            lineList[5].edgeHandler.Init(OneGrabRotateTransformer.Axis.Up, transform);
            lineList[6].edgeHandler.Init(OneGrabRotateTransformer.Axis.Up, transform);
            lineList[7].edgeHandler.Init(OneGrabRotateTransformer.Axis.Up, transform);
        
            lineList[8].SetVertex(vertexList[1], vertexList[3]);
            lineList[9].SetVertex(vertexList[0], vertexList[2]);
            lineList[10].SetVertex(vertexList[5], vertexList[7]);
            lineList[11].SetVertex(vertexList[4], vertexList[6]);
        
        
            lineList[8].edgeHandler.Init(OneGrabRotateTransformer.Axis.Forward, transform);
            lineList[9].edgeHandler.Init(OneGrabRotateTransformer.Axis.Forward, transform);
            lineList[10].edgeHandler.Init(OneGrabRotateTransformer.Axis.Forward, transform);
            lineList[11].edgeHandler.Init(OneGrabRotateTransformer.Axis.Forward, transform);
        }


        if (BoundObjectType == BoundObjectType.TwoDimension)
        {
            for (int i = 0; i < 12; i++)
            {
                lineList[i].gameObject.SetActive(false);
            }
            lineList[0].gameObject.SetActive(true);
            lineList[2].gameObject.SetActive(true);
            lineList[4].gameObject.SetActive(true);
            lineList[5].gameObject.SetActive(true);
        }
    }

    private void SetLineRenderers()
    {
        Gradient colorGradient = new Gradient();
        colorGradient.SetKeys(new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f)},  new GradientAlphaKey[] { new GradientAlphaKey(lineColor.a, 0.0f) });
        foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>(true))
        {
            lr.startWidth = lineWidth;
            lr.enabled = true;
            lr.numCapVertices = numCapVertices;
            lr.colorGradient = colorGradient;
            lr.material = lineMaterial;
        }
    }

    public VertexHandler[] GetVertexList()
    {
        if (vertexList != null && vertexList.Length == 8)
            return vertexList;
        return null;
    }
    public BoundBoxLine[] GetLineList()
    {
        if (lineList != null && lineList.Length == 12)
            return lineList;
        return null;
    }
    
    public BoundObjectType GetBoundObjectType()
    {
        return BoundObjectType;
    }


    #endregion

    #region Select

    
    public void SetComponent(SelectObject selectObject, BoundObjectType boundObjectType)
    {
        if (selectedObject == null)
        {
            Debug.Log("There is no selected Object");
            return;
        }

        if (boundObjectType == BoundObjectType.ThreeDimension)
        {
            transform.rotation = selectedObject.transform.rotation;
        
            centerPositionByVertex.SetVertex();
            boundBox.SetLines();
            transformByVertexHandler.SetVertex();
            transformByVertexHandler.SetLine();

            centerPositionByVertex.enabled = true;
            boundBox.enabled = true;
            transformByVertexHandler.enabled = true;
        
            if (boundBox == null || centerPositionByVertex == null || transformByVertexHandler == null || presentationObject == null)
            {
                Debug.Log("Selected Object doesn't have correct components");
                return;
            }
            
            boundBox.UpdateBounds();
            centerPositionByVertex.CenterPosition();
            boundBox.UpdateBounds();
        
#if UNITY_EDITOR
            for (int i = 0; i < 12; i++)
            {
                lineList[i].UpdateLine();
            }
#endif
        }
        else if(boundObjectType == BoundObjectType.TwoDimension)
        {
            transform.rotation = selectedObject.transform.rotation;
            
            transformByVertexHandler.enabled = true;
            
            if (transformByVertexHandler == null || presentationObject == null)
            {
                Debug.Log("Selected Object doesn't have correct components");
                return;
            }
            XRSelector.Instance.transformByVertexHandler.Init();
            
#if UNITY_EDITOR
            lineList[0].UpdateLine();
            lineList[2].UpdateLine();
            lineList[4].UpdateLine();
            lineList[5].UpdateLine();
#endif
        }
        BoundObjectType = boundObjectType;
        
        NotifyObjectChangeToObservers();
    }


    public void EnableAnimationGhost()
    {
        beforeAnimationGhost.gameObject.SetActive(true);
        afterAnimationGhost.gameObject.SetActive(true);

        if (selectObject.deployType == DeployType.Cube || selectObject.deployType == DeployType.Sphere
                                                       || selectObject.deployType == DeployType.Cylinder ||
                                                       selectObject.deployType == DeployType.ImportModel)
        {
            beforeAnimationGhost.SetRenderer(RendererType.Mesh);
            MeshFilter meshFilter = selectedObject.GetComponentInChildren<MeshFilter>();
            Mesh mesh = meshFilter.mesh;
            beforeAnimationGhost.initialScale = meshFilter.transform.localScale;
            afterAnimationGhost.initialScale = meshFilter.transform.localScale;
            beforeAnimationGhost.SetMesh(mesh);
            afterAnimationGhost.SetMesh(mesh);
            
        } else if (selectObject.deployType == DeployType.ImportImage || selectObject.deployType == DeployType.Plane)
        {
            RawImage image = selectedObject.GetComponentInChildren<RawImage>(true);
            beforeAnimationGhost.SetRenderer(RendererType.Image);
            afterAnimationGhost.SetRenderer(RendererType.Image);
            
            beforeAnimationGhost.initialScale = image.transform.localScale;
            afterAnimationGhost.initialScale = image.transform.localScale;
            
            beforeAnimationGhost.SetImage(image.texture);
            afterAnimationGhost.SetImage(image.texture);

        } else if (selectObject.deployType == DeployType.Text)
        {
            beforeAnimationGhost.SetRenderer(RendererType.Text);
            
        }

        if (presentationObject is PresentationObject)
        {
            beforeAnimationGhost.selectedObject = presentationObject as PresentationObject;
            afterAnimationGhost.selectedObject = presentationObject as PresentationObject;;
        }
        
    }
    
    public void DisableAnimationGhost()
    {
        beforeAnimationGhost.gameObject.SetActive(false);
        afterAnimationGhost.gameObject.SetActive(false);
    }
    #endregion
    
}
