using UnityEngine;
using System.Collections;

public class LightBulbSwitch : MonoBehaviour {

	// Use this for initialization
	void OnEnable() 
	{
		if (CaringPageControls.TargetItem == null) return;
		ToggleableButtonScript toggle = CaringPageControls.CurrentUI.GetComponentInChildren<ToggleableButtonScript>();
		LighbulbSwitchOnOffScript onOff = CaringPageControls.TargetItem.GetComponentInChildren<LighbulbSwitchOnOffScript>();
		if (toggle != null && onOff != null)
		{
			toggle.Set(onOff.IsOn);
		}		
	}
}
