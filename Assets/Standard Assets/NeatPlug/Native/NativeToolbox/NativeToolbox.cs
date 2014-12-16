/**
 * NativeToolbox.cs
 * 
 * A Singleton class encapsulating public access methods of native tool box. 
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-native-toolbox-plugin to find information how 
 * to use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All rights reserved.
 * 
 * @version 1.4.3
 *
 */

using UnityEngine;
using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;


public class NativeToolbox
{
	private class NativeToolboxNativeHelper : INativeToolboxNativeHelper {
		
#if UNITY_ANDROID	
		private AndroidJavaObject _plugin = null;
#endif		
		public NativeToolboxNativeHelper()
		{
			
		}
		
		public void CreateInstance(string className, string instanceMethod)
		{	
#if UNITY_ANDROID			
			AndroidJavaClass jClass = new AndroidJavaClass(className);
			_plugin = jClass.CallStatic<AndroidJavaObject>(instanceMethod);	
#endif			
		}
		
		public void Call(string methodName, params object[] args)
		{
#if UNITY_ANDROID			
			_plugin.Call(methodName, args);	
#endif
		}
		
		public void Call(string methodName, string signature, object arg)
		{
#if UNITY_ANDROID			
			var method = AndroidJNI.GetMethodID(_plugin.GetRawClass(), methodName, signature);			
			AndroidJNI.CallObjectMethod(_plugin.GetRawObject(), method, AndroidJNIHelper.CreateJNIArgArray(new object[] {arg}));
#endif			
		}
		
		public ReturnType Call<ReturnType> (string methodName, params object[] args)
		{
#if UNITY_ANDROID			
			return _plugin.Call<ReturnType>(methodName, args);
#else
			return default(ReturnType);			
#endif			
		}
	
	};		
	
		
	static NativeToolbox ()
	{
	}	
	
	public static class DialogKit {
	
		public enum DialogIconType
		{
			Alert = 0,
			Info,
			Email,
			Dialer,
			Map,
			Star
		};
		
		public enum UILayout
		{
			Top_Left = 0,
			Top_Centered,		
			Top_Right,
			Middle_Left,
			Middle_Centered,		
			Middle_Right,
			Bottom_Left,
			Bottom_Centered,		
			Bottom_Right		
		};
		
		public enum BalloonDuration
		{
			Short = 0,
			Long	
		};
		
		public static readonly string BTN_CAP_SYS_OK = "SYS_OK";
		public static readonly string BTN_CAP_SYS_CANCEL = "SYS_CANCEL";
		public static readonly string BTN_CAP_SYS_YES = "SYS_YES";
		public static readonly string BTN_CAP_SYS_NO = "SYS_NO";
		
		static DialogKit()
		{
#if UNITY_ANDROID	
			if (!NativeToolboxAndroid.IsNativeHelperSet())
			{
				NativeToolboxAndroid.SetNativeHelper(new NativeToolboxNativeHelper());
			}
#endif			
		}
			
		/**
		 * Create a message balloon.
		 * 
		 * @param text
		 *           string - text to be shown on the balloon.
		 * 
		 * @param duration
		 *           BalloonDuration - the balloon duration.
		 * 
		 * @param layout
		 *           UILayout - layout for placing the balloon.
		 * 
		 * @param xMargin
		 *           int - horizontal margin
		 * 
		 * @param yMargin 
		 *           int - vertical margin
		 * 	
		 */
		public static void CreateMessageBalloon (string text,	                                         
										BalloonDuration duration,
										UILayout layout,		                                     
										int xMargin, int yMargin)
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.DialogKit.CreateMessageBalloon(text, (int)duration, (int)layout, xMargin, yMargin);			
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.DialogKit.CreateMessageBalloon(text, (int)duration, (int)layout, xMargin, yMargin);
#endif			
		}
		
		/**
		 * Create a message box.
		 * 
		 * This is a flexible message box which you can use to
		 * present an alert dialog, "Rate It" dialog, confirmation dialog, and more...
		 * 
		 * There are 3 buttons can be defined and shown up on the dialog :
		 * (1) Positive Button, like "OK", "Yes", "Agree", "Like" ...
		 * (2) Neutral Button, like "Just so so", "No comment" ...
		 * (3) Negative Button, like "Cancel", "No", "Disagree", "Dislike" ...
		 * You can customize the button captions, the dialog icon, the title, and 
		 * the message.	 
		 * 
		 * Below button captions can be set to system-defined button captions: 
		 * (1) BTN_CAP_SYS_OK
		 * (2) BTN_CAP_SYS_CANCEL
		 * (3) BTN_CAP_SYS_YES
		 * (4) BTN_CAP_SYS_NO
		 * NOTE: On Android systems, there is an issue may mislead users: 
		 *       SYS_YES and SYS_NO still show the strings of SYS_OK and SYS_CANCEL.
		 *       See: http://code.google.com/p/android/issues/detail?id=3713 
		 *       It is suggested that you use literal "Yes" and "No" to make it 
		 *       right on all platforms.
		 * 
		 * @param id
		 *         string - the unique id of the dialog, 
		 *                  you should set it to get notified with
		 *                  UI events (e.g. OnClick)
		 * 
         * @param title
		 *            string - dialog title
		 * 
		 * @param message
		 *            string - message to be shown on the dialog
		 * 
		 * @param icon 
		 *          DialogIconType - the icon of the dialog
		 * 
		 * @param positiveButtonCaption
		 *            string - positive button caption
		 * 
		 * @param neutralButtonCaption
		 *            string - neutral button caption
		 * 
		 * @param negativeButtonCaption
		 *            string - negative button caption
		 * 		
		 */
		public static void CreateMessageBox (string id, string title, string message, 
										DialogIconType icon,		                                
										string positiveButtonCaption, 
										string neutralButtonCaption, 
										string negativeButtonCaption)
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.DialogKit.CreateMessageBox(id, title, message, (int)icon, 
			                                                positiveButtonCaption, 
			                                                neutralButtonCaption, 
			                                                negativeButtonCaption);
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.DialogKit.CreateMessageBox(id, title, message, (int)icon, 
			                                                positiveButtonCaption, 
			                                                neutralButtonCaption, 
			                                                negativeButtonCaption);
#endif			
		}
		
		/**
		 * Create a text input dialog.
		 * 
		 * @param id
		 *         string - the unique id of the dialog
		 *		
		 * @param title
		 *            string - dialog title
		 * 
		 * @param message
		 *            string - message to be shown on the dialog
		 * 
		 * @param icon 
		 *           DialogIconType - the icon of the dialog
		 *  	
		 */
		public static void CreateTextInputDialog (string id, String title, string message, 
											DialogIconType icon)
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.DialogKit.CreateTextInputDialog(id, title, message, (int)icon);
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.DialogKit.CreateTextInputDialog(id, title, message, (int)icon);
#endif
		}	
		
		
		
		/**
		 * Create a login dialog.
		 * 
		 * @param id
		 *         string - the unique id of the dialog	 
		 * 
		 * @param title
		 *            string - Login dialog title
		 * 
		 * @param message
		 *            string - Login dialog message (optional)
		 * 
		 * @param usernameCaption
		 *                    string - the caption of the username label
		 * 
		 * @param passwordCaption
		 *                    string - the caption of the password label
		 * 
		 * @param loginButtonCaption
		 *                    string - the caption of the login button
		 * 
		 * @param cancelButtonCaption
		 *                    string - the caption of the cancel button	
		 * 
		 */	
		public static void CreateLoginDialog (string id, string title, string message,
									string usernameCaption, string passwordCaption,
									string loginButtonCaption, string cancelButtonCaption)
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.DialogKit.CreateLoginDialog(id, title, message, 
			                                             usernameCaption, passwordCaption,
			                                             loginButtonCaption, cancelButtonCaption);
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.DialogKit.CreateLoginDialog(id, title, message, 
			                                             usernameCaption, passwordCaption,
			                                             loginButtonCaption, cancelButtonCaption);
#endif			
		}
		
		/**
		 * Create an open web confirm dialog.
		 * 
		 * @param id
		 *         string - the unique id of the dialog	
		 * 
		 * @param title
		 *            string - dialog title
		 * 
		 * @param message
		 *            string - message to be shown on the dialog
		 * 
		 * @param url
		 *            string - the url to be opened
		 * 
		 *  @param icon 
		 *           DialogIconType - the icon of the dialog 
		 *  	
		 */
		public static void CreateOpenWebConfirmDialog (string id, String title, string message, string url,
											DialogIconType icon)
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.DialogKit.CreateOpenWebConfirmDialog(id, title, message, url, (int)icon);	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.DialogKit.CreateOpenWebConfirmDialog(id, title, message, url, (int)icon);
#endif			
		}
	
		
		/**
		 * Create a progress dialog.
		 * 
		 * @param id
		 *         string - the unique id of the dialog	
		 * 
		 * @param title
		 *            string - dialog title
		 * 
		 * @param message
		 *            string - message to be shown on the dialog		
		 *  	
		 */		
		public static void CreateProgressDialog (string id, String title, string message)
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.DialogKit.CreateProgressDialog(id, title, message);	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.DialogKit.CreateProgressDialog(id, title, message);
#endif			
		}
		
		/**
		 * Dismiss a progress dialog
		 *
	     * @param id
		 *       string - the unique id of the dialog
		 * 
		 */
		public static void DismissProgressDialog (string id)
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.DialogKit.DismissProgressDialog(id);	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.DialogKit.DismissProgressDialog(id);
#endif			
		} 
		
	};
	
	public static class GeoKit {
		
		static GeoKit()
		{
#if UNITY_ANDROID	
			if (!NativeToolboxAndroid.IsNativeHelperSet())
			{
				NativeToolboxAndroid.SetNativeHelper(new NativeToolboxNativeHelper());
			}
#endif			
		}		
		
		/**
		 * Spot the location on google map with location marker.
		 * 
		 * @param longitude
		 *             string - the longitude of the specified location
		 *            
		 * @param latitude
		 *             string - the latitude of the specified location 
		 * 
		 * @param label
		 *            string - the label of the location to be shown on the map
		 *           
		 */
		public static void SpotLocationOnMap(string latitude, string longitude, string label) {			
#if UNITY_ANDROID		
			NativeToolboxAndroid.GeoKit.SpotLocationOnMap(latitude, longitude, label);	                                                      
#endif	
#if UNITY_IPHONE
			NativeToolboxIOS.GeoKit.SpotLocationOnMap(latitude, longitude, label);
#endif			
		}
			
		/**
		 * Subscribe the device location updates.
		 * 
		 * This enables device GPS location updates receiver.
		 */
		public static void SubscribeLocationUpdates() {
#if UNITY_ANDROID		
			NativeToolboxAndroid.GeoKit.SubscribeLocationUpdates();	                                                      
#endif	
#if UNITY_IPHONE
			NativeToolboxIOS.GeoKit.SubscribeLocationUpdates();
#endif			
		}
		
		/**
		 * Unsubscribe the device location updates.
		 * 
		 * This disables device GPS location updates receiver.
		 */
		public static void UnsubscribeLocationUpdates() {
#if UNITY_ANDROID		
			NativeToolboxAndroid.GeoKit.UnsubscribeLocationUpdates();	                                                      
#endif	
#if UNITY_IPHONE
			NativeToolboxIOS.GeoKit.UnsubscribeLocationUpdates();
#endif			
		}	
		
	};	
	
	public static class TelephonyKit {
		
		static TelephonyKit()
		{
#if UNITY_ANDROID	
			if (!NativeToolboxAndroid.IsNativeHelperSet())
			{
				NativeToolboxAndroid.SetNativeHelper(new NativeToolboxNativeHelper());
			}
#endif	
		}		
		
		/**
		 * Show the dial keypad.
		 * 
		 * @param number
		 *            string - the phone number to dial	
		 *         
		 */
		public static void ShowDialKeypad(string number) {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.TelephonyKit.ShowDialKeypad(number);	                                                      
#endif	
#if UNITY_IPHONE
			NativeToolboxIOS.TelephonyKit.ShowDialKeypad(number);
#endif			
		}
		
		/**
		 * Call the phone number.
		 * 
		 * @param number
		 *            string - the phone number to call	
		 *            
		 */
		public static void CallPhone(string number) {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.TelephonyKit.CallPhone(number);	                                                      
#endif	
#if UNITY_IPHONE
			NativeToolboxIOS.TelephonyKit.CallPhone(number);
#endif			
			
		}	
		
	};
	
	public static class MessagingKit {
			
		static MessagingKit()
		{
#if UNITY_ANDROID	
			if (!NativeToolboxAndroid.IsNativeHelperSet())
			{
				NativeToolboxAndroid.SetNativeHelper(new NativeToolboxNativeHelper());
			}
#endif			
		}		
		
		/**
		 * Show SMS composer.	
		 * 
		 * @param number
		 *             string - the phone number to send SMS to
		 * @param text
		 *            string - SMS text to be sent
		 *
		 */
		public static void ShowSMSComposer(string number, string text) {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MessagingKit.ShowSMSComposer(number, text);	                                                      
#endif	
#if UNITY_IPHONE
			NativeToolboxIOS.MessagingKit.ShowSMSComposer(number, text);
#endif			
			
		}
		
		/**
		 * Send SMS in the background.
		 * 
		 * To receive the delivery status of the SMS, you need to
		 * drag & drop NativeToolboxAgent and NativeToolboxMessagingKitListener
		 * prefabs to your first scene.
		 *
		 * The SMS delivery result will be returned via event:
		 * NativeToolboxMessagingKitListener::OnSmsDelivered when succeeded or
		 * NativeToolboxMessagingKitListener::OnFailedToDeliverSms when failed.
		 * 
		 * @param number
		 *           string - the phone number to send SMS to	
		 * @param text
		 *           string  - SMS text to be sent
		 *         
		 */
		public static void SendSMS(string number, string text) {	
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MessagingKit.SendSMS(number, text);	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MessagingKit.SendSMS(number, text);
#endif			
			
		}
		
		/**
		 * Show MMS composer.
		 * 
		 * @param number
		 *            string - the phone number to send MMS message to	
		 * @param mediaFile
		 *            string - the media file to be attached (e.g. "/sdcard/somepic.png")
		 * @param subject
		 *            string - MMS subject
		 * @param text
		 *            string - text body            
		 *        
		 */
		public static void ShowMMSComposer(string number, string mediaFile, string subject, string text) {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MessagingKit.ShowMMSComposer(number, mediaFile, subject, text);	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MessagingKit.ShowMMSComposer(number, mediaFile, subject, text);
#endif			
			
		}
		
		/**
		 * Show email composer.
		 * 
		 * @param to
		 *          string - the recipient addresses to send email to (comma-saperated)
		 * @param cc
		 *          string - the cc addresses to send email to (comma-saperated)
		 * @param bcc
		 *          string - the bcc addresses to send email to (comma-saperated)
		 * @param subject
		 *          string - the email subject
		 * @param body
		 *          string - the email text body
		 * @param attachment
		 *          string - the email attachment path (e.g. "/sdcard/somepic.png")          
		 *         
		 */
		public static void ShowEmailComposer(string to, string cc, string bcc,
				string subject, string body, string attachment) {	
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MessagingKit.ShowEmailComposer(to, cc, bcc, subject, body, attachment);	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MessagingKit.ShowEmailComposer(to, cc, bcc, subject, body, attachment);
#endif			
		}
			
	};
	
	public static class MediaKit {
		
		static MediaKit()
		{
#if UNITY_ANDROID	
			if (!NativeToolboxAndroid.IsNativeHelperSet())
			{
				NativeToolboxAndroid.SetNativeHelper(new NativeToolboxNativeHelper());
			}
#endif			
		}		
		
		/**
		 * Play a media file (audio or video).
		 * 
		 * @param mediaFile
		 *             string - absolute path of the media file to play	
		 *                      ("/sdcard/somemedia.mp3")
		 *          
		 */
		public static void PlayMedia(string mediaFile) {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MediaKit.PlayMedia(mediaFile);	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MediaKit.PlayMedia(mediaFile);
#endif			
		}	
		
		/**
		 * Explore the media store on the device and get all audio files information.
		 *
		 * The query result will be sent to NativeToolboxMediaKitListener::OnAudioItemInfoArrived event handler. 
		 *
		 * You can play the audio file by calling NativeToolbox.MediaKit.PlayMedia(audioInfo.uri); 
		 * (audioInfo is an AudioFileInfo object which is passed to OnAudioItemInfoArrived() event handler).
		 *          
		 */
		public static void ExploreAudioFiles() {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MediaKit.ExploreAudioFiles();	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MediaKit.ExploreAudioFiles();
#endif			
			
		}
		
		/**
		 * Explore the media store on the device and get all video files information.
		 *
		 * The query result will be sent to NativeToolboxMediaKitListener::OnVideoItemInfoArrived event handler. 
		 *
		 * You can play the video file by calling NativeToolbox.MediaKit.PlayMedia(videoInfo.uri); 
		 * (videoInfo is a VideoFileInfo object which is passed to OnVideoItemInfoArrived() event handler).
		 *          
		 */
		public static void ExploreVideoFiles() {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MediaKit.ExploreVideoFiles();	                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MediaKit.ExploreVideoFiles();
#endif			
			
		}		
		
		/**
		 * Scan a newly added media file and add it to device gallery instantly.				
		 */
		public static void ScanSingleNewMedia() {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MediaKit.ScanSingleNewMedia(null);                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MediaKit.ScanSingleNewMedia(null);  
#endif	
			
		}		
		
		/**
		 * Scan a newly added media file and add it to device gallery instantly.
		 * 
		 * You app will get notified by NativeToolboxMediaKitListener::OnSingleNewMediaScanned()
         * when the scan completed.
		 * 
		 * @param string
		 *            mediaFile - the media file to scan.		
		 */
		public static void ScanSingleNewMedia(String mediaFile) {
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MediaKit.ScanSingleNewMedia(mediaFile);                                                      
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MediaKit.ScanSingleNewMedia(mediaFile);  
#endif	
			
		}
		
		/**
		 * Scan all newly added media files on pesistent storage and add them to media store.	
		 *
		 * NOTE: This method call is really expensive since all media files on the device
		 * storage will be explored. It is strongly recommended that you should always
		 * use ScanSingleNewMedia() instead.
		 */
		public static void ScanAllNewMedia() {			
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.MediaKit.ScanAllNewMedia();                                                    
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MediaKit.ScanAllNewMedia();  
#endif
			
		}
		
		/**
		 * Capture a screen shot and save it on device persistent data storage.
		 * 
		 * You app will get notified by NativeToolboxMediaKitListener::OnScreenShotCaptured()
         * when the capture completed.
		 * 
		 * @param string
		 *          fileName - The screen shot file name, if you leave this null,
		 *                     the plugin will create a random filename for you.
		 *          
		 */
		public static void CaptureScreenShot(string fileName)
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.MediaKit.CaptureScreenShot(fileName);                                                   
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MediaKit.CaptureScreenShot(fileName); 
#endif			
		}
		
		/**
		 * Capture a screen shot and share it.		 
		 */
		public static void ShareScreenShot() 
		{
#if UNITY_ANDROID		
			NativeToolboxAndroid.MediaKit.ShareScreenShot();                                                  
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.MediaKit.ShareScreenShot();
#endif	
		}
		
	};
	
	public static class WebKit {
		
		static WebKit()
		{
#if UNITY_ANDROID	
			if (!NativeToolboxAndroid.IsNativeHelperSet())
			{
				NativeToolboxAndroid.SetNativeHelper(new NativeToolboxNativeHelper());
			}
#endif			
		}
		
		/**
		 * Open a web view.
		 * 
		 * @param string
		 *           url - The url to be opened.		
		 */
		public static void OpenWebView(string url) {		
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.WebKit.OpenWebView(url);                                                    
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.WebKit.OpenWebView(url);
#endif	

		}
		
		/**
		 * Open an embedded web view.
		 * 
		 * @param string
		 *           url - The url to be opened.	
		 * 
		 * @param float
		 *           widthScale - The scale of the width. (0 - 1)
		 * 
		 * @param string
		 *           heightScale - The scale of the height. (0 - 1)
		 * 
		 * @param WebViewLayout
		 *           layout - The layout of the webview.
		 * 
		 * @param bool
		 *           shouldSavePassword - True for save, false for not.
		 * 
		 * @param bool
		 *           shoudSaveFormData - True for save, false for not.
		 * 
		 */
		public static void OpenEmbeddedWebView(string url, float widthScale, float heightScale, 
			WebViewLayout layout,	bool shouldSavePassword, bool shoudSaveFormData) {		
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.WebKit.OpenEmbeddedWebView(url, widthScale, heightScale, (int)layout, shouldSavePassword, shoudSaveFormData);                                                    
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.WebKit.OpenEmbeddedWebView(url, widthScale, heightScale, (int)layout, shouldSavePassword, shoudSaveFormData);
#endif	

		}	
		
		/**
		 * Close the embedded web view.				
		 */
		public static void CloseEmbeddedWebView() {		
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.WebKit.CloseEmbeddedWebView();                                                  
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.WebKit.CloseEmbeddedWebView(); 
#endif	

		}		
		
	};
	
	public static class SystemKit {
		
		static SystemKit()
		{
#if UNITY_ANDROID	
			if (!NativeToolboxAndroid.IsNativeHelperSet())
			{
				NativeToolboxAndroid.SetNativeHelper(new NativeToolboxNativeHelper());
			}
#endif		
		}
		
		/**
		 * Get System ID.
		 * 
		 * This will get Android ID for android platform,
		 * on iOS platform, this is just a placeholder.
		 *          
		 */
		public static string GetSystemId() {
			
			String result = string.Empty;
			
#if UNITY_ANDROID		
			result = NativeToolboxAndroid.SystemKit.GetSystemId();                                                      
#endif
#if UNITY_IPHONE
			result = NativeToolboxIOS.SystemKit.GetSystemId();
#endif			
			
			return result;
		}
		
		/**
		 * Get System Device ID		 
		 *          
		 */
		public static string GetDeviceId() {
			
			String result = string.Empty;
			
#if UNITY_ANDROID		
			result = NativeToolboxAndroid.SystemKit.GetDeviceId();                                                      
#endif
#if UNITY_IPHONE
			result = NativeToolboxIOS.SystemKit.GetDeviceId();
#endif			
			
			return result;
		}
		
		/**
		 * Get System Sim Serial Number.	 
		 *          
		 */
		public static string GetSimSerialNumber() {
			
			String result = string.Empty;
			
#if UNITY_ANDROID		
			result = NativeToolboxAndroid.SystemKit.GetSimSerialNumber();                                                     
#endif
#if UNITY_IPHONE
			result = NativeToolboxIOS.SystemKit.GetSimSerialNumber();
#endif			
			
			return result;
		}
		
		/**
		 * Get System Sim Serial Number.	 
		 *          
		 */
		public static string GetSimOperatorName() {
			
			String result = string.Empty;
			
#if UNITY_ANDROID		
			result = NativeToolboxAndroid.SystemKit.GetSimOperatorName();                                                      
#endif
#if UNITY_IPHONE
			result = NativeToolboxIOS.SystemKit.GetSimOperatorName();
#endif			
			
			return result;
		}
		
		/**
		 * Acquire a system wake lock, to keep the screen on.	 
		 *          
		 */
		public static void AcquireWakeLock() {			
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.SystemKit.AcquireWakeLock();                                                     
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.SystemKit.AcquireWakeLock();
#endif			
		}		
		
		/**
		 * Release the acquired system wake lock.	 
		 *          
		 */
		public static void ReleaseWakeLock() {			
			
#if UNITY_ANDROID		
			NativeToolboxAndroid.SystemKit.ReleaseWakeLock();                                                     
#endif
#if UNITY_IPHONE
			NativeToolboxIOS.SystemKit.ReleaseWakeLock();
#endif			
		}			
		
		
	};
}

