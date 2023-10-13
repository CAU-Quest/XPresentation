using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour, ISubject
{
    public static MainSystem Instance = null;

    private List<ISystemObserver> observers = new List<ISystemObserver>();
    
    private int mode; // 0 - Edit Mode, 1 - Deploy Mode, 2 - Slide Mode, 3 - Animation Mode

    public List<Slide> slideList = new List<Slide>();

    [SerializeField]
    private int slideCount = 1;
    [SerializeField]
    private int currentSlideNum;
    [SerializeField]
    public float slideInterval;
    public bool isPlayingAnimation = true;
    
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

    public void ChangeMode(int mode)
    {
        if (this.mode != mode)
        {
            this.mode = mode;
            NotifyObservers();
        }
    }

    public void AnimationToggle()
    {
        isPlayingAnimation = !isPlayingAnimation;
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
