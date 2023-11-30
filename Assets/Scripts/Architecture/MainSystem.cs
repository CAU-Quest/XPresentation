using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainSystem : MonoBehaviour, ISubject
{

    #region Member Variables

    public static MainSystem Instance = null;

    public List<ISystemObserver> observers = new List<ISystemObserver>();
    public List<IUserInterfaceObserver> UIObservers = new List<IUserInterfaceObserver>();


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

    

    #endregion

    #region Object Observer

    public void RegisterObserver(ISystemObserver observer)
    {
        this.observers.Add(observer);
    }
    
    public void RemoveObserver(ISystemObserver observer)
    {
        this.observers.Remove(observer);
    }

    
    public void NotifyObservers()
    {
        for (int i = 0; i < this.observers.Count; i++)
        {
            this.observers[i].ObserverUpdateMode(mode);
            this.observers[i].ObserverUpdateSlide(currentSlideNum);
        }
        
        XRSelector.Instance.DeactivateBoundBox();
        
        XRSelector.Instance.NotifySlideChangeToObservers();
    }
    
    public void NotifyObserverSaveData()
    {
        SaveData.Instance.objects.Clear();
        for (int i = 0; i < this.observers.Count; i++)
        {
            observers[i].ObserverUpdateSave();
        }
    }

    public void NotifyObserversMoveSlides(int moved, int count, int into)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].ObserverMoveSlides(moved, count, into);
        }
        
        XRSelector.Instance.DeactivateBoundBox();
    }


    #endregion
    
    #region Slide Control

    public int GetSlideCount()
    {
        return slideCount;
    }

    public void AddSlide()
    {
        slideCount += 1;
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


    public void MoveSlide()
    {
        MoveSlidesTo(moved, count, into);
    }

    
    
    public void MoveSlideTo(int moved, int into) // moved 위치에 있는 slide를 into 위치로 옮김
    {
        NotifyObserversMoveSlides(moved, 1, into);
    }

    public void MoveSlidesTo(int moved, int count, int into) // moved 위치에 있는 count 갯수 만큼의 slide를 into 위치로 옮김
    {
        NotifyObserversMoveSlides(moved, count, into);
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

    

    #endregion

    #region Mode Control

    public void ChangeMode(Mode mode)
    {
        if (this.mode != mode)
        {
            this.mode = mode;
            NotifyObservers();
        }
    }
    public void AnimationToggle()
    {
        isPlayingAnimation = true;
        XRSelector.Instance.DeactivateBoundBox();
    }


    

    #endregion

    #region Unity LifeCycle

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

        mode = Mode.Edit;
    }
    
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

    #endregion
}
