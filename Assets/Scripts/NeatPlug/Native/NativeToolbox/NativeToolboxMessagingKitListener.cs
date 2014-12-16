/**
 * NativeToolboxMessagingKitListener.cs
 * 
 * NativeToolboxMessagingKitListener listens to the Native Toolbox MessagingKit events.
 * File location: Assets/Scripts/NeatPlug/Native/Toolbox/NativeToolboxMessagingKitListener.cs
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

public class NativeToolboxMessagingKitListener : MonoBehaviour {
	
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
		NativeToolboxAgent.OnSmsSent += OnSmsSent;
		NativeToolboxAgent.OnFailedToSendSms += OnFailedToSendSms;
		NativeToolboxAgent.OnSmsDelivered += OnSmsDelivered;
		NativeToolboxAgent.OnFailedToDeliverSms += OnFailedToDeliverSms;
	}

	void OnDisable()
	{
		// Unregister the Native Toolbox events.
		// Do not modify the codes below.		
		NativeToolboxAgent.OnSmsSent -= OnSmsSent;
		NativeToolboxAgent.OnFailedToSendSms -= OnFailedToSendSms;
		NativeToolboxAgent.OnSmsDelivered -= OnSmsDelivered;
		NativeToolboxAgent.OnFailedToDeliverSms -= OnFailedToDeliverSms;
	}
	
	/**
	 * Fired when the sms is sent.	
	 */
	void OnSmsSent()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnSmsSent() Fired.");	
		/// Your code here...
	}
	
	/**
	 * Fired when failed to send the sms.	
	 * 
	 * @param err
	 *         string - the error message.
	 */
	void OnFailedToSendSms(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToSendSms() Fired. Error: " + err);	
		/// Your code here...
	}	
	
	/**
	 * Fired when the sms is successfully delivered.		 
	 */
	void OnSmsDelivered()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnSmsDelivered() Fired.");	
		/// Your code here...
	}	
	
	/**
	 * Fired when failed to deliver the sms.
	 */
	void OnFailedToDeliverSms()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToDeliverSms() Fired.");	
		/// Your code here...
	}	

}
