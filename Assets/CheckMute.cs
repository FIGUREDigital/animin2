using UnityEngine;
using System.Collections;

public class CheckMute : MonoBehaviour {

    void OnEnable(){
        if (ProfilesManagementScript.Instance.CurrentProfile.Settings.AudioEnabled)
        {
            this.GetComponent<ToggleableButtonScript>().SetOff();
        }
        else
        {

            this.GetComponent<ToggleableButtonScript>().SetOn();
        }
    }
}
