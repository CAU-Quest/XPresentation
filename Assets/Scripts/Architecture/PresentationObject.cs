using System.Collections;
using System.Collections.Generic;
using System.Text;
using DimBoxes;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.XR.Interaction.Toolkit;


[System.Serializable]
public struct SlideObjectData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Color color;
    public bool isGrabbable;
    public bool isExist;
}

public class PresentationObject : MonoBehaviour, IPresentationObject, ISystemObserver
{
    private static uint idCount = 1;
    public uint id = 1;

    public List<XRAnimation> animationList = new List<XRAnimation>();
    public List<SlideObjectData> slideData = new List<SlideObjectData>();

    private int currentSlide;
    private MainSystem.Mode mode;

    public Material beforeSlideMaterial;
    public Material afterSlideMaterial;
    public Material normalModeMaterial;

    private GameObject dottedLine;
    public GameObject dottedLinePrefab;

    private GameObject ghostObject;
    private PresentationGhostObject ghost;

    private MeshRenderer meshRenderer;

    public void ObserverUpdateMode(MainSystem.Mode mode)
    {
        this.mode = mode;
        if (mode == MainSystem.Mode.Animation)
        {
            meshRenderer.material = beforeSlideMaterial;
            if (currentSlide + 1 < MainSystem.Instance.GetSlideCount())
            {
                ghostObject.SetActive(true);
                dottedLine.SetActive(true);
            }
        }
        else
        {
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

        SetSlideObjectData(slideData[slide]);
        ghost.applyTransform();
        if (mode == MainSystem.Mode.Animation)
        {
            if (currentSlide + 1 < MainSystem.Instance.GetSlideCount())
            {
                ghostObject.SetActive(true);
                dottedLine.SetActive(true);
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
        SetSlideObjectData(slideData[currentSlide]);
    }
    
    public void addSlide()
    {
        List<Slide> slides = MainSystem.Instance.slideList;
        SlideObjectData beforeTransform = slideData[slideData.Count - 1];
        for (int i = slideData.Count; i < MainSystem.Instance.GetSlideCount(); i++)
        {
            XRAnimation anim = new XRAnimation();
            
            anim.SetParentObject(this);
            //anim.SetSlide(slides[i], slides[i + 1]);
            anim.SetPreviousSlideObjectData(beforeTransform);
            anim.SetNextSlideObjectData(beforeTransform);
            
            slides[i].AddAnimation(id, anim);
            slides[i].AddObjectData(id, beforeTransform);
            animationList.Add(anim);
            slideData.Add(beforeTransform);
        }
    }

    public void GetTransformDataFromSlide(int index)
    {
        slideData[index] = MainSystem.Instance.slideList[index].GetObjectData(id);
    }
    

    public void Start()
    {
        this.id = idCount++;
        meshRenderer = GetComponent<MeshRenderer>();
        MainSystem.Instance.RegisterObserver(this);
        List<Slide> slides = MainSystem.Instance.slideList;
        
        
        SlideObjectData transformData = new SlideObjectData();
        transformData.position = transform.parent.position;
        transformData.rotation = transform.parent.rotation;
        transformData.scale = transform.parent.localScale;
        transformData.color = meshRenderer.material.color;
        
        
        SlideObjectData nextTransformData = new SlideObjectData();
        nextTransformData.position = transform.parent.position;
        nextTransformData.rotation = transform.parent.rotation;
        nextTransformData.scale = transform.parent.localScale;
        nextTransformData.color = meshRenderer.material.color;
        for (int i = 0; i < MainSystem.Instance.GetSlideCount(); i++)
        {
            XRAnimation anim = new XRAnimation();

            transformData.position = nextTransformData.position;
            transformData.rotation = nextTransformData.rotation;
            transformData.scale = nextTransformData.scale;
            transformData.color = meshRenderer.material.color;
            
            nextTransformData = new SlideObjectData();
            nextTransformData.position = transformData.position;
            nextTransformData.rotation = transform.parent.rotation;
            nextTransformData.scale = transformData.scale;
            nextTransformData.color = meshRenderer.material.color;
            
            anim.SetParentObject(this);
            anim.SetPreviousSlideObjectData(transformData);
            anim.SetNextSlideObjectData(nextTransformData);
            
            slides[i].AddAnimation(id, anim);
            slides[i].AddObjectData(id, transformData);
            animationList.Add(anim);
            slideData.Add(transformData);
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
        
        if (meshRenderer == null)
        {
            Debug.LogError("meshRenderer is null.");
        }

        if (ghostObject == null)
        {
            Debug.LogError("ghostObject is null.");
        }
        ghostObject.GetComponentInChildren<MeshRenderer>().material = afterSlideMaterial;

        GameObject go = ghostObject.GetComponentInChildren<PresentationObject>().gameObject;
        Destroy(ghostObject.GetComponentInChildren<PresentationObject>());
        ghost = go.AddComponent<PresentationGhostObject>();

        if (ghost == null)
        {
            Debug.LogError("ghost is null.");
        }
        ghost.parentObject = this;
        ghost.SetID(id);

        dottedLine = Instantiate(dottedLinePrefab);
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
        SlideObjectData transformData = new SlideObjectData();
        transformData.position = transform.parent.position;
        transformData.rotation = transform.parent.rotation;
        transformData.scale = transform.parent.localScale;
        
        transformData.color = meshRenderer.material.color;
        MainSystem.Instance.slideList[this.currentSlide].AddObjectData(this.id, transformData);
        this.slideData[this.currentSlide] = transformData;
        this.animationList[this.currentSlide].SetPreviousSlideObjectData(transformData);
        if(this.currentSlide - 1 >= 0)
            this.animationList[this.currentSlide - 1].SetNextSlideObjectData(transformData);
    }

    public uint GetID()
    {
        return id;
    }
    
    public void SetSlideObjectData(SlideObjectData slideObjectData)
    {
        transform.parent.SetPositionAndRotation(slideObjectData.position, slideObjectData.rotation);
        transform.parent.localScale = slideObjectData.scale;
        meshRenderer.material.color = slideObjectData.color;
    }

    public SlideObjectData GetSlideObjectData()
    {
        SlideObjectData data = new SlideObjectData();
        data.position = transform.parent.position;
        data.rotation = transform.parent.rotation;
        data.scale = transform.parent.localScale;
        data.color = meshRenderer.material.color;
        
        return data;
    }
    
}
