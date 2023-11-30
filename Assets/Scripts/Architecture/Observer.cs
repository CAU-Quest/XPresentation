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
}

public interface ISubject
{
    void RegisterObserver(ISystemObserver observer);
    void RemoveObserver(ISystemObserver observer);
    void NotifyObservers();
    void NotifyObserverSaveData();
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