using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISystemObserver
{
    void ObserverUpdateMode(MainSystem.Mode mode);
    void ObserverUpdateSlide(int slide);
    void ObserverRemoveSlide(int index);
    void ObserverAddSlide();
    void ObserverMoveSlides(int moved, int count, int into);
    void ObserverUpdateSave();
    void ObserverAddSlideNextTo(int index);
    void ObserverCreateVideo(int index);
}

public interface ISubject
{
    void RegisterObserver(ISystemObserver observer);
    void RemoveObserver(ISystemObserver observer);
    void NotifyObservers();
    void NotifyObserverSaveData();
}

public interface ISlideObserver
{
    void ObserverUpdateSlide();
}

public interface ISlideSubject
{
    void RegisterObserver(ISlideObserver observer);
    void RemoveObserver(ISlideObserver observer);
    void NotifySlideChangeToObservers();
}

public interface IUserInterfaceObserver
{
    public void ObserverObjectUpdate(IPresentationObject presentationObject);
    public void ObserverSlideUpdate(int currentSlideNumber);
    public void ObserverSlideObjectDataUpdate();
}

public interface IUserInterfaceSubject
{
    void RegisterObserver(IUserInterfaceObserver observer);
    void RemoveObserver(IUserInterfaceObserver observer);
    void NotifyObjectChangeToObservers();
    void NotifySlideChangeToObservers();
    void NotifySlideObjectDataChangeToObservers();
}