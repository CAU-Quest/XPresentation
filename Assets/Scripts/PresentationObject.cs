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
    private int id = 1;

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
            
            slides[i].AddAnimation(this.GetInstanceID(), anim);
            slides[i].AddObjectData(this.GetInstanceID(), beforeTransform);
            animationList.Add(anim);
            slideData.Add(beforeTransform);
        }
    }

    public void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        MainSystem.Instance.RegisterObserver(this);
        List<Slide> slides = MainSystem.Instance.slideList;
        for (int i = 0; i < MainSystem.Instance.GetSlideCount(); i++)
        {
            XRAnimation anim = new XRAnimation();
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = transform.localScale;
            
            TransformData transformData = new TransformData();
            transformData.position = transform.position;
            transformData.rotation = transform.rotation;
            transformData.scale = transform.localScale;
            
            anim.SetParentObject(this);
            //anim.SetSlide(slides[i], slides[i + 1]);
            anim.SetPreviousTransform(transformData);
            anim.SetNextTransform(transformData);
            
            slides[i].AddAnimation(this.GetInstanceID(), anim);
            slides[i].AddObjectData(this.GetInstanceID(), transformData);
            animationList.Add(anim);
            slideData.Add(transformData);
        }
        
        
        ghostObject = Instantiate(gameObject);
        ghostObject.GetComponent<MeshRenderer>().material = afterSlideMaterial;
        Destroy(ghostObject.GetComponent<PresentationObject>());
        ghost = ghostObject.AddComponent<PresentationGhostObject>();

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

    public int GetID()
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
