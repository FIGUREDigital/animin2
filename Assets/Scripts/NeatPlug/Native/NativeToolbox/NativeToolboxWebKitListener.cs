/**
 * NativeToolboxWebKitListener.cs
 * 
 * NativeToolboxWebKitListener listens to the Native Toolbox MediaKit events.
 * File location: Assets/Scripts/NeatPlug/Native/Toolbox/NativeToolboxWebKitListener.cs
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

public class NativeToolboxWebKitListener : MonoBehaviour {
	
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
		NativeToolboxAgent.OnLinkTappedInWebView += OnLinkTappedInWebView;
		NativeToolboxAgent.OnFunctionCalledFromJSInWebView += OnFunctionCalledFromJSInWebView;
	}

	void OnDisable()
	{
		// Unregister the Native Toolbox events.
		// Do not modify the codes below.		
		NativeToolboxAgent.OnLinkTappedInWebView -= OnLinkTappedInWebView;
		NativeToolboxAgent.OnFunctionCalledFromJSInWebView -= OnFunctionCalledFromJSInWebView;
	}
	
	/**
	 * Fired when a link is tapped through in web view.
	 * 
	 * @param string
	 *          link - The link that is tapped by the user.
	 */
	void OnLinkTappedInWebView(string link)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + "::OnLinkTappedInWebView: link -> " + link);
	}
	
	/**
	 * Fired when a function call from JS in web view received.
	 * 
	 * @param string
	 *          method - The method to be called.
	 * 
	 * @param string
	 *          param - The parameter attached to the function call.
	 */
	void OnFunctionCalledFromJSInWebView(string method, string param)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + "::OnFunctionCalledFromJSInWebView: method -> " + method + ", param -> " + param);
	}
	
}
