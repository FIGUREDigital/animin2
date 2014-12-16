/**
 * NativeToolboxMediaKitListener.cs
 * 
 * NativeToolboxMediaKitListener listens to the Native Toolbox MediaKit events.
 * File location: Assets/Scripts/NeatPlug/Native/Toolbox/NativeToolboxMediaKitListener.cs
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

public class NativeToolboxMediaKitListener : MonoBehaviour {
	
	// Turn this _debug switch on to see debug information.
	private bool _debug = false;	
	
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
		Debug.Log("Listeners Added!");
		NativeToolboxAgent.OnAudioItemInfoArrived += OnAudioItemInfoArrived;
		NativeToolboxAgent.OnVideoItemInfoArrived += OnVideoItemInfoArrived;
		NativeToolboxAgent.OnSingleNewMediaScanned += OnSingleNewMediaScanned;
		NativeToolboxAgent.OnScreenShotCaptured += OnScreenShotCaptured;
	}

	void OnDisable()
	{
		Debug.Log("Listeners Removed...");
		// Unregister the Native Toolbox events.
		// Do not modify the codes below.		
		NativeToolboxAgent.OnAudioItemInfoArrived -= OnAudioItemInfoArrived;
		NativeToolboxAgent.OnVideoItemInfoArrived -= OnVideoItemInfoArrived;
		NativeToolboxAgent.OnSingleNewMediaScanned -= OnSingleNewMediaScanned;
		NativeToolboxAgent.OnScreenShotCaptured -= OnScreenShotCaptured;
	}
	
	/**
	 * Fired when the audio item information arrived.
	 * 
	 * @param audioInfo
	 *         NativeToolbox.AudioFileInfo - the audio file info object.
	 * 	
	 * 
	 */
	void OnAudioItemInfoArrived(AudioFileInfo audioInfo)
	{
		Debug.Log ("OnAudioItemInfoArrived");

		if (_debug)
			Debug.Log (this.GetType().ToString() + "::OnAudioItemInfoArrived: title -> " 
				+ audioInfo.title + ", artist -> " 
				+ audioInfo.artist 
				+ ", duration -> " 
				+ audioInfo.duration 
				+ ", uri -> " 
				+ audioInfo.uri
			    + ", artFile -> "
			    + audioInfo.artFile);

		UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().StereoScript.AddAudio(audioInfo);

		/// Your code here...
	}
	
	/**
	 * Fired when the video item information arrived.
	 * 
	 * @param videoInfo
	 *         NativeToolbox.VideoFileInfo - the video file info object.
	 * 	
	 * 
	 */
	void OnVideoItemInfoArrived(VideoFileInfo videoInfo)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + "::OnVideoItemInfoArrived: title -> " 
				+ videoInfo.title + ", artist -> " 
				+ videoInfo.artist 
				+ ", duration -> " 
				+ videoInfo.duration 
				+ ", uri -> " 
				+ videoInfo.uri);
		/// Your code here...
	}	
	
	/**
	 * Fired when a newly added single media is successfully scanned.
	 * 
	 * @param string
	 *          filePath - The absolute file path of the media file.
	 */
	void OnSingleNewMediaScanned(string filePath)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + "::OnSingleNewMediaScanned: filePath -> " + filePath);
	}
	
	/**
	 * Fired when a screen shot is successfully captured.
	 * 
	 * @param string
	 *          filePath - The absolute file path of the screen shot media file.
	 */
	void OnScreenShotCaptured(string filePath)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + "::OnScreenShotCaptured: filePath -> " + filePath);
	}	
	
}
