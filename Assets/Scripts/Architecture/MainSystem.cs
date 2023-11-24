using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 0 - Edit Mode, 1 - Deploy Mode, 2 - Slide Mode, 3 - Animation Mode
public class MainSystem : MonoBehaviour, ISubject
{
    public static MainSystem Instance = null;

    private List<ISystemObserver> observers = new List<ISystemObserver>();

    [HideInInspector]
    public List<Slide> slideList = new List<Slide>();

    public enum Mode
    {
        None, Edit, Animation, Preview
    }

    [Header("System")]

    public Mode mode; 
    
    [SerializeField]
    private int slideCount = 1;
    public int currentSlideNum;
    [SerializeField]
    public float slideInterval;
    public bool isPlayingAnimation = true;
    
    [Header("Import Option")]

    public Material beforeSlideMaterial;
    public Material afterSlideMaterial;
    public GameObject dottedLinePrefab;
    
    
    [Header("Functions")]



    public int moved;
    public int count;
    public int into;

    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            for(int i = 0; i < slideCount; i++)
                slideList.Add(new Slide());
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        mode = Mode.Edit;
    }

    public void MoveSlide()
    {
        MoveSlidesTo(moved, count, into);
    }
    
    public void RegisterObserver(ISystemObserver observer)
    {
        this.observers.Add(observer);
    }
    
    public void RemoveObserver(ISystemObserver observer)
    {
        this.observers.Remove(observer);
    }

    public int GetSlideCount()
    {
        return slideCount;
    }

    public void NotifyObservers()
    {
        for (int i = 0; i < this.observers.Count; i++)
        {
            this.observers[i].ObserverUpdateMode(mode);
            this.observers[i].ObserverUpdateSlide(currentSlideNum);
        }
        
        int lineLength = XRSelector.Instance.lineList.Length;
        int vertexLength = XRSelector.Instance.vertexList.Length;

        for (int i = 0; i < lineLength; i++)
        {
            XRSelector.Instance.lineList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < vertexLength; i++)
        {
            XRSelector.Instance.vertexList[i].gameObject.SetActive(false);
        }
    }

    public void NotifyObserversMoveSlides(int moved, int count, int into)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].ObserverMoveSlides(moved, count, into);
        }
        
        int lineLength = XRSelector.Instance.lineList.Length;
        int vertexLength = XRSelector.Instance.vertexList.Length;

        for (int i = 0; i < lineLength; i++)
        {
            XRSelector.Instance.lineList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < vertexLength; i++)
        {
            XRSelector.Instance.vertexList[i].gameObject.SetActive(false);
        }
    }
    
    
    
    public void AddSlide()
    {
        slideCount += 1;
        slideList.Add(new Slide());
        for (int i = 0; i < this.observers.Count; i++)
        {
            observers[i].ObserverAddSlide();
        }
    }
    
    public void RemoveSlide()
    {
        slideCount -= 1;
        for (int i = 0; i < this.observers.Count; i++)
        {
            observers[i].ObserverRemoveSlide(currentSlideNum);
        }
    }

    public void MoveSlideTo(int moved, int into) // moved 위치에 있는 slide를 into 위치로 옮김
    {
        Slide element = slideList[moved];
        slideList.RemoveAt(moved);
        
        slideList.Insert(into, element);
        NotifyObserversMoveSlides(moved, 1, into);
    }

    public void MoveSlidesTo(int moved, int count, int into) // moved 위치에 있는 count 갯수 만큼의 slide를 into 위치로 옮김
    {
        List<Slide> elements = slideList.GetRange(moved, count);
        slideList.RemoveRange(moved, count);

        if(into < moved) slideList.InsertRange(into, elements);
        else slideList.InsertRange(into - count + 1, elements);
        
        NotifyObserversMoveSlides(moved, count, into);
    }

    public ISlide GetSlideByIndex(int index)
    {
        return slideList[index];
    }

    public void GoToPreviousSlide()
    {
        if (currentSlideNum > 0)
        {
            currentSlideNum -= 1;
            slideInterval = 0;
            isPlayingAnimation = false;
            NotifyObservers();
        }
    }

    public void GoToNextSlide()
    {
        if (currentSlideNum < slideCount - 1)
        {
            currentSlideNum += 1;
            slideInterval = 0;
            isPlayingAnimation = false;
            NotifyObservers();
        }
    }

    public void GoToSlideByIndex(int index)
    {
        if (index >= 0 && index < slideCount)
        {
            currentSlideNum = index;
            slideInterval = 0;
            isPlayingAnimation = false;
            NotifyObservers();
        }
    }

    public void ChangeMode(Mode mode)
    {
        if (this.mode != mode)
        {
            this.mode = mode;
            NotifyObservers();
        }
    }

    public void ToggleMode()
    {
        this.mode++;
        //this.mode %= 4;
        NotifyObservers();
    }

    public void AnimationToggle()
    {
        isPlayingAnimation = true;
        int lineLength = XRSelector.Instance.lineList.Length;
        int vertexLength = XRSelector.Instance.vertexList.Length;

        for (int i = 0; i < lineLength; i++)
        {
            XRSelector.Instance.lineList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < vertexLength; i++)
        {
            XRSelector.Instance.vertexList[i].gameObject.SetActive(false);
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if (isPlayingAnimation && slideInterval < 1.0f)
        {
            if (currentSlideNum == slideCount - 1) isPlayingAnimation = false;
            else
            {
                slideInterval += Time.deltaTime;
                if (slideInterval >= 1.0f)
                {
                    slideInterval = 1.0f;
                    if (currentSlideNum + 1 < slideCount)
                    {
                        slideInterval = 0;
                        currentSlideNum += 1;
                        isPlayingAnimation = false;
                        NotifyObservers();
                    }
                }
            }
        }
    }
}
