using UnityEngine;
using System.Collections;

public class ResetTutorials : MonoBehaviour {

    public void Press(){
        TutorialHandler.Instance.ResetTutorials();
    }
}
