/**
 * NativeToolboxDialogKitListener.cs
 * 
 * NativeToolboxDialogKitListener listens to the Native Toolbox DialogKit events.
 * File location: Assets/Scripts/NeatPlug/Native/Toolbox/NativeToolboxDialogKitListener.cs
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-native-toolbox-plugin to find information 
 * about how to use this program.
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

public class NativeToolboxDialogKitListener : MonoBehaviour {
	
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
		NativeToolboxAgent.OnMessageBoxPositiveButtonClicked += OnMessageBoxPositiveButtonClicked;
		NativeToolboxAgent.OnMessageBoxNeutralButtonClicked += OnMessageBoxNeutralButtonClicked;
		NativeToolboxAgent.OnMessageBoxNegativeButtonClicked += OnMessageBoxNegativeButtonClicked;	
		NativeToolboxAgent.OnTextInputDialogOKButtonClicked += OnTextInputDialogOKButtonClicked;
		NativeToolboxAgent.OnTextInputDialogCancelButtonClicked += OnTextInputDialogCancelButtonClicked;
		NativeToolboxAgent.OnLoginDialogOKButtonClicked += OnLoginDialogOKButtonClicked;
		NativeToolboxAgent.OnLoginDialogCancelButtonClicked += OnLoginDialogCancelButtonClicked;
		NativeToolboxAgent.OnOpenWebConfirmDialogOKButtonClicked += OnOpenWebConfirmDialogOKButtonClicked;
		NativeToolboxAgent.OnOpenWebConfirmDialogCancelButtonClicked += OnOpenWebConfirmDialogCancelButtonClicked;
	}

	void OnDisable()
	{
		// Unregister the Native Toolbox events.
		// Do not modify the codes below.		
		NativeToolboxAgent.OnMessageBoxPositiveButtonClicked -= OnMessageBoxPositiveButtonClicked;
		NativeToolboxAgent.OnMessageBoxNeutralButtonClicked -= OnMessageBoxNeutralButtonClicked;
		NativeToolboxAgent.OnMessageBoxNegativeButtonClicked -= OnMessageBoxNegativeButtonClicked;
		NativeToolboxAgent.OnTextInputDialogOKButtonClicked -= OnTextInputDialogOKButtonClicked;
		NativeToolboxAgent.OnTextInputDialogCancelButtonClicked -= OnTextInputDialogCancelButtonClicked;
		NativeToolboxAgent.OnLoginDialogOKButtonClicked -= OnLoginDialogOKButtonClicked;
		NativeToolboxAgent.OnLoginDialogCancelButtonClicked -= OnLoginDialogCancelButtonClicked;
		NativeToolboxAgent.OnOpenWebConfirmDialogOKButtonClicked -= OnOpenWebConfirmDialogOKButtonClicked;
		NativeToolboxAgent.OnOpenWebConfirmDialogCancelButtonClicked -= OnOpenWebConfirmDialogCancelButtonClicked;
	}
	
	/**
	 * Fired when the positive button on the message box is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created message box, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateMessageBox(id, ...)
	 *                  method to create a message box. 
	 */
	void OnMessageBoxPositiveButtonClicked(string id)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnMessageBoxPositiveButtonClicked(\"" + id + "\") Fired.");	
		/// Your code here...
	}
	
	/**
	 * Fired when the neutral button on the message box is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created message box, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateMessageBox(id, ...)
	 *                  method to create a message box. 
	 */
	void OnMessageBoxNeutralButtonClicked(string id)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnMessageBoxNeutralButtonClicked(\"" + id + "\") Fired.");	
		/// Your code here...
	}	
	
	/**
	 * Fired when the negative button on the message box is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created message box, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateMessageBox(id, ...)
	 *                  method to create a message box. 
	 */
	void OnMessageBoxNegativeButtonClicked(string id)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnMessageBoxNegativeButtonClicked(\"" + id + "\") Fired.");	
		/// Your code here...
	}		
	
	/**
	 * Fired when the Ok button on the Text Input Dialog is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created dialog, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateTextInputDialog(id, ...)
	 *                  method to create a text input dialog. 
	 * 
	 * @param input
	 *         string - the user input.
	 */
	void OnTextInputDialogOKButtonClicked(string id, string input)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnTextInputDialogOKButtonClicked(\"" + id + "\", \"" + input + "\") Fired.");	
		/// Your code here...
	}
	
	/**
	 * Fired when the Cancel button on the Text Input Dialog is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created dialog, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateTextInputDialog(id, ...)
	 *                  method to create a text input dialog. 
	 */
	void OnTextInputDialogCancelButtonClicked(string id)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnTextInputDialogCancelButtonClicked(\"" + id + "\") Fired.");	
		/// Your code here...
	}
	
	/**
	 * Fired when the Ok button on the Login Dialog is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created dialog, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateLoginDialog(id, ...)
	 *                  method to create a user login dialog. 
	 * 
	 * @param username
	 *         string - user-entered username
	 * 
	 * @param password
	 *         string - user-entered password
	 */
	void OnLoginDialogOKButtonClicked(string id, string username, string password)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnLoginDialogOKButtonClicked(\"" + id + "\", \"" + username + "\", \"" + password + "\") Fired.");	
		/// Your code here...
	}
	
	/**
	 * Fired when the Cancel button on the Login Dialog is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created dialog, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateLoginDialog(id, ...)
	 *                  method to create a user login dialog. 
	 */
	void OnLoginDialogCancelButtonClicked(string id)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnLoginDialogCancelButtonClicked(\"" + id + "\") Fired.");	
		/// Your code here...
	}	
	
	/**
	 * Fired when the Ok button on the Open Web Confirm Dialog is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created dialog, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateOpenWebConfirmDialog(id, ...)
	 *                  method to create a dialog to let the user confirm before opening
	 *                  a web view. 
	 */
	void OnOpenWebConfirmDialogOKButtonClicked(string id)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnOpenWebConfirmDialogOKButtonClicked(\"" + id + "\") Fired.");	
		/// Your code here...
	}	
	
	/**
	 * Fired when the Cancel button on the Open Web Confirm Dialog is clicked.
	 *
	 * @param id
	 *         string - unique identifier of created dialog, 
	 *                  the id is set by yourself when you call 
	 *                  NativeToolbox.DialogKit.CreateOpenWebConfirmDialog(id, ...)
	 *                  method to create a dialog to let the user confirm before opening
	 *                  a web view. 
	 */
	void OnOpenWebConfirmDialogCancelButtonClicked(string id)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnOpenWebConfirmDialogCancelButtonClicked(\"" + id + "\") Fired.");	
		/// Your code here...
	}		
	

}
