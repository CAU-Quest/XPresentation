using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DimBoxes;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

//using UnityEngine.XR.Interaction.Toolkit;


[System.Serializable]
public struct SlideObjectData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Color color;
    public bool isGrabbable;
    public bool isVisible;

    public SlideObjectData(Vector3 pos, Quaternion rot, Vector3 scale, Color col, bool grabbable = false,
        bool visible = false)
    {
        position = pos;
        rotation = rot;
        this.scale = scale;
        color = col;
        isGrabbable = grabbable;
        isVisible = visible;
    }
    
    public SlideObjectData(SlideObjectData currentData, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        position = pos;
        rotation = rot;
        this.scale = scale;
        color = currentData.color;
        isGrabbable = currentData.isGrabbable;
        isVisible = currentData.isVisible;
    }
    
    public SlideObjectData(SlideObjectData currentData, Color col)
    {
        position = currentData.position;
        rotation = currentData.rotation;
        scale = currentData.scale;
        color = col;
        isGrabbable = currentData.isGrabbable;
        isVisible = currentData.isVisible;
    }
}

public class PresentationObject : MonoBehaviour, IPresentationObject, ISystemObserver
{
    private static uint idCount = 1;
    public uint id = 1;

    public List<XRAnimation> animationList = new List<XRAnimation>();
    public List<SlideObjectData> slideData = new List<SlideObjectData>();

    private int currentSlide;
    private MainSystem.Mode mode;

    private Canvas canvas;

    private Material normalModeMaterial;

    private GameObject dottedLine;

    private GameObject ghostObject;
    private PresentationGhostObject ghost;

    private MeshRenderer meshRenderer;
    private Grabbable grabbable;


    public Transform Transform => transform.parent;
    public Material Material => (meshRenderer) ? meshRenderer.material : null;
    public bool isGrabbableInPresentation = true;
    public bool isVisible = true;

    public void ObserverUpdateMode(MainSystem.Mode mode)
    {
        this.mode = mode;
        if (mode == MainSystem.Mode.Animation)
        {
            if(meshRenderer != null)
                meshRenderer.material = MainSystem.Instance.beforeSlideMaterial;
            if (currentSlide + 1 < MainSystem.Instance.GetSlideCount())
            {
                if (slideData[currentSlide + 1].isVisible)
                {
                    ghostObject.SetActive(true);
                    if (isVisible)
                    {
                        dottedLine.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if(meshRenderer != null)
                meshRenderer.material = normalModeMaterial;
            ghostObject.SetActive(false);
            dottedLine.SetActive(false);
        }
    }

    public int GetCurrentSlide()
    {
        return currentSlide;
    }

    public void ObserverUpdateSlide(int slide)
    {
        this.currentSlide = slide;

        ApplySlideObjectData(slideData[slide]);
        ghost.applyTransform();
        if (mode == MainSystem.Mode.Animation)
        {
            if (currentSlide + 1 < MainSystem.Instance.GetSlideCount())
            {
                if (slideData[slide + 1].isVisible)
                {
                    ghostObject.SetActive(true);
                    if (isVisible)
                    {
                        dottedLine.SetActive(true);
                    }
                }
                
            }
            else
            {
                ghostObject.SetActive(false);
                dottedLine.SetActive(false);
            }
        }
    }

    public void ObserverRemoveSlide(int index)
    {
        removeSlide(index);
    }

    public void ObserverAddSlide()
    {
        addSlide();
    }
    
    public void removeSlide(int index)
    {
        animationList.RemoveAt(index);
        slideData.RemoveAt(index);
        ApplySlideObjectData(slideData[currentSlide]);
    }
    
    public void addSlide()
    {
        SlideObjectData beforeTransform = slideData[slideData.Count - 1];
        for (int i = slideData.Count; i < MainSystem.Instance.GetSlideCount(); i++)
        {
            XRAnimation anim = new XRAnimation();
            
            anim.SetParentObject(this);
            anim.SetPreviousSlideObjectData(beforeTransform);
            anim.SetNextSlideObjectData(beforeTransform);
            
            animationList.Add(anim);
            slideData.Add(beforeTransform);
        }
    }

    public void SetGrabbable(bool boolean)
    {
        isGrabbableInPresentation = boolean;
        
        if(isGrabbableInPresentation)
            grabbable.MaxGrabPoints = -1;
        else
            grabbable.MaxGrabPoints = 0;
    }

    public void SetVisible()
    {
        isVisible = true;
        if (isGrabbableInPresentation && grabbable)
        {
            grabbable.MaxGrabPoints = -1;
        }
        if (meshRenderer)
        {
            meshRenderer.enabled = true;
        }

        if (canvas)
        {
            canvas.enabled = true;
        }
    }

    public void SetInvisible()
    {
        isVisible = false;
        if (grabbable)
        {
            grabbable.MaxGrabPoints = 0;
        }
        if (meshRenderer)
        {
            meshRenderer.enabled = false;
        }

        if (canvas)
        {
            canvas.enabled = false;
        }
    }
    

    public void Start()
    {
        Init(MainSystem.Instance.currentSlideNum);
    }

    public void Init(int currentSlideNum)
    {
        this.id = idCount++;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        canvas = GetComponent<Canvas>();
        grabbable = GetComponent<Grabbable>();
        if (grabbable == null) grabbable = GetComponentInParent<Grabbable>();
        if(meshRenderer != null)
            normalModeMaterial = meshRenderer.material;
        MainSystem.Instance.RegisterObserver(this);
        
        
        SlideObjectData slideObjectData = new SlideObjectData();
        
        SlideObjectData nextSlideData = new SlideObjectData();
        nextSlideData.position = Vector3.zero;
        nextSlideData.rotation = Quaternion.identity;
        nextSlideData.scale = Vector3.one;
        nextSlideData.color = Color.white;
        nextSlideData.isVisible = false;
        nextSlideData.isGrabbable = false;
        
        for (int i = 0; i < MainSystem.Instance.GetSlideCount(); i++)
        {
            XRAnimation anim = new XRAnimation();
            
            slideObjectData.position = nextSlideData.position;
            slideObjectData.rotation = nextSlideData.rotation;
            slideObjectData.scale = nextSlideData.scale;
            slideObjectData.isVisible = nextSlideData.isVisible;
            slideObjectData.isGrabbable = nextSlideData.isGrabbable;
            if (meshRenderer != null) slideObjectData.color = meshRenderer.material.color;
            
            nextSlideData = new SlideObjectData();
            nextSlideData.position = Vector3.zero;
            nextSlideData.rotation = Quaternion.identity;
            nextSlideData.scale = Vector3.one;
            nextSlideData.color = Color.white;
            nextSlideData.isVisible = false;
            nextSlideData.isGrabbable = false;
            
            if (currentSlideNum == i)
            {
                slideObjectData.position = transform.parent.position;
                slideObjectData.rotation = transform.parent.rotation;
                slideObjectData.scale = transform.parent.localScale;
                if (meshRenderer != null) slideObjectData.color = meshRenderer.material.color;
                slideObjectData.isVisible = true;
                slideObjectData.isGrabbable = true;
            }
            
            anim.SetParentObject(this);
            anim.SetPreviousSlideObjectData(slideObjectData);
            anim.SetNextSlideObjectData(nextSlideData);
            
            animationList.Add(anim);
            slideData.Add(slideObjectData);
        }
        
        
        ghostObject = Instantiate(transform.parent.gameObject);
        TransformByVertexHandler tvh = ghostObject.GetComponent<TransformByVertexHandler>();
        CenterPositionByVertex cpv = ghostObject.GetComponent<CenterPositionByVertex>();
        BoundBox bb = ghostObject.GetComponent<BoundBox>();

        if (tvh != null)
        {
            tvh.enabled = false;
        }

        if (cpv != null)
        {
            cpv.enabled = false;
        }

        if (bb != null)
        {
            bb.enabled = false;
        }
        

        if (ghostObject == null)
        {
            Debug.LogError("ghostObject is null.");
        }

        GameObject go = ghostObject.GetComponentInChildren<PresentationObject>().gameObject;
        if(meshRenderer != null)
            go.GetComponentInChildren<MeshRenderer>().material = MainSystem.Instance.afterSlideMaterial;
        Destroy(ghostObject.GetComponentInChildren<PresentationObject>());
        ghost = go.AddComponent<PresentationGhostObject>();

        if (ghost == null)
        {
            Debug.LogError("ghost is null.");
        }
        ghost.parentObject = this;
        ghost.SetID(id);

        dottedLine = Instantiate(MainSystem.Instance.dottedLinePrefab);
        dottedLine.GetComponent<XRAnimationLine>().object1 = this;
        dottedLine.GetComponent<XRAnimationLine>().object2 = ghost;

        dottedLine.SetActive(false);
        ghostObject.SetActive(false);
    }

    public void Update()
    {
        if (MainSystem.Instance.isPlayingAnimation)
        {
            animationList[currentSlide].Play();
        }
    }

    public XRAnimation CreateDefaultAnimation(int index)
    {
        XRAnimation element = new XRAnimation();
        if (index + 1 < slideData.Count)
        {
            element.SetParentObject(this);
            element.SetPreviousSlideObjectData(slideData[index]);
            element.SetNextSlideObjectData(slideData[index + 1]);
        }
        
        return element;
    }
    
    public void ObserverMoveSlides(int moved, int count, int into)
    {
        Debug.Log("Move Slides(" + moved + ", " + count + ", " + into + ")");
        if (count == 1)
        {
            SlideObjectData transformElement = slideData[moved];
            slideData.RemoveAt(moved);

            slideData.Insert(into, transformElement);

            if (into < moved)
            {
                animationList.RemoveAt(moved);
                if(moved - 1 >= 0) animationList.RemoveAt(moved - 1);
                if (into - 1 >= 0)
                {
                    animationList.RemoveAt(into - 1);
                    animationList.Insert(into - 1, CreateDefaultAnimation(into - 1));
                }
                animationList.Insert(into, CreateDefaultAnimation(into));
                if(moved - 1 >= 0) animationList.Insert(moved, CreateDefaultAnimation(moved));
            } 
            else
            {
                if (into - 1 >= 0) animationList.RemoveAt(into);
                animationList.RemoveAt(moved);
                if(moved - 1 >= 0) animationList.RemoveAt(moved - 1);
                if(moved - 1 >= 0) animationList.Insert(moved - 1, CreateDefaultAnimation(moved - 1));
                if (into - 1 >= 0) animationList.Insert(into - 1, CreateDefaultAnimation(into - 1));
                animationList.Insert(into, CreateDefaultAnimation(into));   
            }
        }
        else
        {
            List<SlideObjectData> transformDatas = slideData.GetRange(moved, count);
            slideData.RemoveRange(moved, count);
            
            if (into < moved) slideData.InsertRange(into, transformDatas);
            else slideData.InsertRange(into - count + 1, transformDatas);

            List<XRAnimation> animationElements = animationList.GetRange(moved, count);
                

            if (into < moved)
            {
                animationList.RemoveRange(moved, count);
                if(moved - 1 >= 0) animationList.RemoveAt(moved - 1);
                if (into - 1 >= 0) animationList.RemoveAt(into - 1);
                if (into - 1 >= 0) animationList.Insert(into - 1, CreateDefaultAnimation(into - 1));
                animationList.InsertRange(into, animationElements);
                animationList.RemoveAt(into + count - 1);
                animationList.Insert(into + count - 1, CreateDefaultAnimation(into + count - 1));
                if(moved - 1 >= 0) animationList.Insert(moved + count - 1, CreateDefaultAnimation(moved + count - 1));
            }
            else
            {
                animationList.RemoveAt(into);
                animationList.RemoveRange(moved, count);
                if(moved - 1 >= 0) animationList.RemoveAt(moved - 1);
                if(moved - 1 >= 0) animationList.Insert(moved - 1, CreateDefaultAnimation(moved - 1));
                animationList.Insert(into - count, CreateDefaultAnimation(into - count));
                animationList.InsertRange(into - count + 1, animationElements);
                animationList.RemoveAt(into);
                animationList.Insert(into, CreateDefaultAnimation(into));
            }
        }
    }

    public void SaveTransformToSlide()
    {
        var data = new SlideObjectData();
        data.position = transform.parent.position;
        data.rotation = transform.parent.rotation;
        data.scale = transform.parent.localScale;
        if (meshRenderer != null) data.color = meshRenderer.material.color;
        
        this.slideData[this.currentSlide] = data;
        this.animationList[this.currentSlide].SetPreviousSlideObjectData(data);
        if(this.currentSlide - 1 >= 0)
            this.animationList[this.currentSlide - 1].SetNextSlideObjectData(data);
    }

    public SlideObjectData GetCurrentSlideObjectData()
    {
        var currentSlideIndex = MainSystem.Instance.currentSlideNum;
        return slideData[currentSlideIndex];
    }

    public uint GetID()
    {
        return id;
    }
    
    public void ApplySlideObjectData(SlideObjectData data) //SetSlideObjectData -> ApplySlideObjectData
    {
        transform.parent.SetPositionAndRotation(data.position, data.rotation);
        transform.parent.localScale = data.scale;
        if (meshRenderer != null) meshRenderer.material.color = data.color;
        
        if (data.isVisible)
        {
            SetVisible();
        }
        else
        {
            SetInvisible();
        }
        SetGrabbable(data.isGrabbable);
    }

    public void UpdateSlideObjectData(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        
    }

    public void UpdateSlideObjectData(Color col)
    {
        
    }
    
    /*
    public SlideObjectData GetSlideObjectData()
    {
        var data = new SlideObjectData();
        data.position = transform.parent.position;
        data.rotation = transform.parent.rotation;
        data.scale = transform.parent.localScale;
        if (meshRenderer != null)
        {
            data.material = meshRenderer.material;
            data.color = data.material.color;
        }

        return data;
    }
    */
}
