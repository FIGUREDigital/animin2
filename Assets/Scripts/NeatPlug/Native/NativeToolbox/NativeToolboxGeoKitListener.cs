/**
 * NativeToolboxGeoKitListener.cs
 * 
 * NativeToolboxGeoKitListener listens to the Native Toolbox GeoKit events.
 * File location: Assets/Scripts/NeatPlug/Native/Toolbox/NativeToolboxGeoKitListener.cs
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-native-toolbox-plugin to find information 
 * about how to integrate and use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All Rights Reserved.
 * 
 * @version 1.4.3
 *
 */

using UnityEngine;
using System.Collections;

public class NativeToolboxGeoKitListener : MonoBehaviour {
	
	// Turn this _debug switch on to see debug information.
	private bool _debug = true;	
	
	private static bool _instanceFound = false;
	
	void Awake()
	{
		if (_instanceFound)
		{
			Destroy(gameObject);
			return;
		}
		_instanceFound = true;
		// The gameObject will retain...
		DontDestroyOnLoad(this);	
	}
	
	void OnEnable()
	{
		// Register the Native Toolbox events.
		// Do not modify the codes below.
		NativeToolboxAgent.OnGPSLocationChanged += OnGPSLocationChanged;
		NativeToolboxAgent.OnGPSProviderDisabled += OnGPSProviderDisabled;
		NativeToolboxAgent.OnGPSProviderEnabled += OnGPSProviderEnabled;
		NativeToolboxAgent.OnGPSStatusChanged += OnGPSStatusChanged;
	}

	void OnDisable()
	{
		// Unregister the Native Toolbox events.
		// Do not modify the codes below.		
		NativeToolboxAgent.OnGPSLocationChanged -= OnGPSLocationChanged;
		NativeToolboxAgent.OnGPSProviderDisabled -= OnGPSProviderDisabled;
		NativeToolboxAgent.OnGPSProviderEnabled -= OnGPSProviderEnabled;
		NativeToolboxAgent.OnGPSStatusChanged -= OnGPSStatusChanged;
	}
	
	/**
	 * Fired when the GPS location changed.	
	 * 
	 * @param latitude
	 *         double - the latitude of the location.
	 * 
	 * @param langitude
	 *         double - the longitude of the location.
	 * 
	 */
	void OnGPSLocationChanged(double latitude, double longitude)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnGPSLocationChanged(" + latitude + ", " + longitude + ") Fired.");	
		/// Your code here...
	}
	
	/**
	 * Fired when the GPS provider disabled.	
	 * 
	 * @param provider
	 *         string - the GPS provider.
	 */
	void OnGPSProviderDisabled(string provider)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnGPSProviderDisabled(\"" + provider + "\") Fired.");	
		/// Your code here...
	}	
	
	/**
	 * Fired when the GPS provider enabled.	
	 * 
	 * @param provider
	 *         string - the GPS provider.
	 */
	void OnGPSProviderEnabled(string provider)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnGPSProviderEnabled(\"" + provider + "\") Fired.");	
		/// Your code here...
	}	
	
	/**
	 * Fired when the GPS status changed.	
	 * 
	 * @param provider
	 *         string - the GPS provider.
	 */
	void OnGPSStatusChanged(string provider, string status)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnGPSStatusChanged(\"" + provider + "\", \"" + status + "\") Fired.");	
		/// Your code here...
	}	

}
