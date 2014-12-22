using UnityEngine;
using System.Collections;

public class CheckMute : MonoBehaviour {

    void OnEnable(){
        if (ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled)
        {
            this.GetComponent<ToggleableButtonScript>().SetOff();
        }
        else
        {

            this.GetComponent<ToggleableButtonScript>().SetOn();
        }
    }
}
