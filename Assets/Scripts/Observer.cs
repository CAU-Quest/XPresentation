using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISystemObserver
{
    void ObserverUpdateMode(int mode);
    void ObserverUpdateSlide(int slide);
    void ObserverRemoveSlide(int index);
    void ObserverAddSlide();
}

public interface ISubject
{
    void RegisterObserver(ISystemObserver observer);
    void RemoveObserver(ISystemObserver observer);
    void NotifyObservers();
}
