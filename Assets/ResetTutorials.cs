using UnityEngine;
using System.Collections;

public class ResetTutorials : MonoBehaviour {

    public void Press(){
        UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.ResetTutorials();
    }
}
