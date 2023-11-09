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

}

public interface ISubject
{
    void RegisterObserver(ISystemObserver observer);
    void RemoveObserver(ISystemObserver observer);
    void NotifyObservers();
}
