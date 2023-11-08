using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.XR.Interaction.Toolkit;


[System.Serializable]
public struct TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    
}

public class PresentationObject : MonoBehaviour, IPresentationObject, ISystemObserver
{
    private static uint idCount = 1;
    public uint id = 1;

    public List<XRAnimation> animationList = new List<XRAnimation>();
    public List<TransformData> slideData = new List<TransformData>();

    private int currentSlide;
    private int mode;

    public Material beforeSlideMaterial;
    public Material afterSlideMaterial;
    public Material normalModeMaterial;

    private GameObject dottedLine;
    public GameObject dottedLinePrefab;

    private GameObject ghostObject;
    private PresentationGhostObject ghost;

    private MeshRenderer meshRenderer;

    public void ObserverUpdateMode(int mode)
    {
        this.mode = mode;
        if (mode == 3)
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

        SetTransform(slideData[slide].position, slideData[slide].rotation, slideData[slide].scale);
        ghost.applyTransform();
        if (mode == 3)
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
        SetTransform(slideData[currentSlide].position, slideData[currentSlide].rotation, slideData[currentSlide].scale);
    }
    
    public void addSlide()
    {
        List<Slide> slides = MainSystem.Instance.slideList;
        TransformData beforeTransform = slideData[slideData.Count - 1];
        for (int i = slideData.Count; i < MainSystem.Instance.GetSlideCount(); i++)
        {
            XRAnimation anim = new XRAnimation();
            
            anim.SetParentObject(this);
            //anim.SetSlide(slides[i], slides[i + 1]);
            anim.SetPreviousTransform(beforeTransform);
            anim.SetNextTransform(beforeTransform);
            
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
        
        
        TransformData transformData = new TransformData();
        transformData.position = transform.position;
        transformData.rotation = transform.rotation;
        transformData.scale = transform.localScale;
        
        
        TransformData nextTransformData = new TransformData();
        nextTransformData.position = transform.position;
        nextTransformData.rotation = transform.rotation;
        nextTransformData.scale = transform.localScale;
        for (int i = 0; i < MainSystem.Instance.GetSlideCount(); i++)
        {
            XRAnimation anim = new XRAnimation();
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = transform.localScale;

            transformData.position = nextTransformData.position;
            transformData.rotation = nextTransformData.rotation;
            transformData.scale = nextTransformData.scale;
            
            nextTransformData = new TransformData();
            nextTransformData.position = transformData.position;
            nextTransformData.rotation = transformData.rotation;
            nextTransformData.scale = transformData.scale;

            nextTransformData.position.y += Random.Range(0, 1f);
            
            anim.SetParentObject(this);
            //anim.SetSlide(slides[i], slides[i + 1]);
            anim.SetPreviousTransform(transformData);
            anim.SetNextTransform(nextTransformData);
            
            slides[i].AddAnimation(id, anim);
            slides[i].AddObjectData(id, transformData);
            animationList.Add(anim);
            slideData.Add(transformData);
        }
        
        
        ghostObject = Instantiate(gameObject, transform);
        if (meshRenderer == null)
        {
            Debug.LogError("meshRenderer is null.");
        }

        if (ghostObject == null)
        {
            Debug.LogError("ghostObject is null.");
        }
        ghostObject.GetComponent<MeshRenderer>().material = afterSlideMaterial;
        Destroy(ghostObject.GetComponent<PresentationObject>());
        ghost = ghostObject.AddComponent<PresentationGhostObject>();

        if (ghost == null)
        {
            Debug.LogError("ghost is null.");
        }
        ghost.parentObject = this;
        ghost.SetID(id);

        dottedLine = Instantiate(dottedLinePrefab, transform);
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
            element.SetPreviousTransform(slideData[index]);
            element.SetNextTransform(slideData[index + 1]);
        }
        
        return element;
    }
    
    public void ObserverMoveSlides(int moved, int count, int into)
    {
        Debug.Log("Move Slides(" + moved + ", " + count + ", " + into + ")");
        if (count == 1)
        {
            TransformData transformElement = slideData[moved];
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
            List<TransformData> transformDatas = slideData.GetRange(moved, count);
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
        TransformData transformData = new TransformData();
        transformData.position = transform.position;
        transformData.rotation = transform.rotation;
        transformData.scale = transform.localScale;
        MainSystem.Instance.slideList[this.currentSlide].AddObjectData(this.id, transformData);
        this.slideData[this.currentSlide] = transformData;
        this.animationList[this.currentSlide].SetPreviousTransform(transformData);
        if(this.currentSlide - 1 >= 0)
            this.animationList[this.currentSlide - 1].SetNextTransform(transformData);
    }

    public uint GetID()
    {
        return id;
    }
    
    public void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.SetPositionAndRotation(position, rotation);
        transform.localScale = scale;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public Quaternion GetRotation()
    {
        return this.transform.rotation;
    }
    
}
