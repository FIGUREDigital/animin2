using UnityEngine;
using System.Collections;

// This class allows you to hide objects until a given tutorial has been completed or a 'fire' event within it has been fired 
public class TutorialShowHide : MonoBehaviour {

    public string fireEvent;
    public GameObject[] itemsToShowOnceUnlocked;  // These objects will be hidden on start if the given tutorial has not been completed
    public GameObject[] additionalShowOnEvent;    // These objects along with itemsToShowOnceUnlocked are shown when the event occurs
    public bool showOnRepeatedEvents = true;      // If the event occurs again (ie is reused) then should we show the items again?
    public bool hideAdditionalOnProgress = false;  // If true then additionalShowOnEvent objects will be hidden once the tutorial system moves past the event that fires fireEvent
    public string hideAdditionalOnSpecificEvent = "NextLesson";   // eg "NextLesson"

    bool tutorialCompleted = false;
    bool eventFired = false;

	void Start () 
    {
        tutorialCompleted = TutorialHandler.CheckTutorialContainingEventCompleted(fireEvent);
        ShowHide(itemsToShowOnceUnlocked, tutorialCompleted);
        ShowHide(additionalShowOnEvent, false);
	    TutorialHandler.FireEvents += EventFired;
	}

    void OnDestroy()
    {
        TutorialHandler.FireEvents -= EventFired;
    }
	
    void EventFired(string fired)
    {
        if (fireEvent == fired && (!tutorialCompleted || showOnRepeatedEvents))
        {
            if (!tutorialCompleted)
            {
                ShowHide(itemsToShowOnceUnlocked, true);
            }
            ShowHide(additionalShowOnEvent, true);
            eventFired = true;
            tutorialCompleted = true;
        }
        else if (eventFired)
        {
            // Remove items on progression to next event
            if (hideAdditionalOnProgress || ((hideAdditionalOnSpecificEvent.Length > 0) && (fired == hideAdditionalOnSpecificEvent)))
            {
                eventFired = false;
                ShowHide(additionalShowOnEvent, false);
            }
        }
	}

    static void ShowHide(GameObject[] objects, bool show)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                objects[i].SetActive(show);
            }
        }
    }
}
