#define KAMCORD_ANDROID

#if UNITY_ANDROID && !(UNITY_2_6 || UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1)

using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KamcordJSON;

public class KamcordImplementationAndroid : Kamcord.Implementation
{
    private AndroidJavaClass kamcordJavaClass_;

    const string javaClassString = "com.kamcord.android.Kamcord";
    const string beginDrawString = "beginDraw";
    const string endDrawString = "endDraw";
    const string isEnabledString = "isEnabled";
    const string isRecordingString = "isRecording";
    const string beginDrawErrorString = "Kamcord: BeginDraw called with no kamcordJavaClass().  This error only prints once even if it happens more.";
    const string endDrawErrorString = "Kamcord: EndDraw called with no kamcordJavaClass().  This error only prints once even if it happens more.";
    const string setCurrentlyBoundFBString = "setCurrentlyBoundFramebufferToBeAppRenderingFramebuffer";

    private AndroidJavaClass kamcordJavaClass()
    {
        if ( kamcordJavaClass_ == null )
            kamcordJavaClass_ = new AndroidJavaClass (javaClassString);

        if ( kamcordJavaClass_ == null )
        {
            Debug.Log("Kamcord: Unable to find Kamcord java class.");
        }

        return kamcordJavaClass_;
    }

    public override void SetLoggingEnabled(bool value)
    {
        if (kamcordJavaClass() != null)
            kamcordJavaClass().CallStatic("setLoggingEnabled", value);
    }

    public override bool IsEnabled()
    {
        if (kamcordJavaClass() != null)
            return kamcordJavaClass().CallStatic<bool>(isEnabledString);

        return false;
    }

    public override String GetDisabledReason()
    {
        if (kamcordJavaClass() != null)
            return kamcordJavaClass().CallStatic<string>("getDisabledReason");

        return "Kamcord java class object not accessible from Unity script.";
    }

    public override void WhitelistBoard(string boardName)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: WhitelistBoard called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("whitelistBoard", boardName);
    }

    public override void BlacklistBoard(string boardName)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: BlacklistBoard called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("blacklistBoard", boardName);
    }

    public override void WhitelistDevice(string deviceName)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: WhitelistDevice called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("whitelistDevice", deviceName);
    }

    public override void BlacklistDevice(string deviceName)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: BlacklistDevice called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("blacklistDevice", deviceName);
    }

    public override void WhitelistBoard(string boardName, int sdkVersion)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: WhitelistBoard called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("whitelistBoard", boardName, sdkVersion);
    }

    public override void BlacklistBoard(string boardName, int sdkVersion)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: BlacklistBoard called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("blacklistBoard", boardName, sdkVersion);
    }

    public override void WhitelistDevice(string deviceName, int sdkVersion)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: WhitelistDevice called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("whitelistDevice", deviceName, sdkVersion);
    }

    public override void BlacklistDevice(string deviceName, int sdkVersion)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: BlacklistDevice called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("blacklistDevice", deviceName, sdkVersion);
    }

    public override void WhitelistAllBoards()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: WhitelistAllBoards called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("whitelistAllBoards");
    }

    public override void BlacklistAllBoards()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: BlacklistAllBoards called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("blacklistAllBoards");
    }

    public override void WhitelistAll()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: WhitelistAll called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("whitelistAll");
    }

    public override void BlacklistAll()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: BlacklistAll called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("blacklistAll");
    }

    public override string GetBoard()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: GetBoard called with no kamcordJavaClass");
            return "";
        }

        return kamcordJavaClass().CallStatic<string>("getBoard");
    }

    public override string GetDevice()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: GetDevice called with no kamcordJavaClass");
            return "";
        }

        return kamcordJavaClass().CallStatic<string>("getDevice");
    }

    // Deprecated.
    public override bool IsWhitelisted(string boardName)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: IsWhitelisted called with no kamcordJavaClass");
            return false;
        }

        return kamcordJavaClass().CallStatic<bool>("isWhitelisted", boardName);
    }

    public override bool IsWhitelisted()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: IsWhitelisted called with no kamcordJavaClass");
            return false;
        }

        return kamcordJavaClass().CallStatic<bool>("isWhitelisted");
    }

    public override void DoneChangingWhitelist()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: DoneChangingWhitelist called with no kamcordJavaClass");
            return;
        }

        kamcordJavaClass().CallStatic("doneChangingWhitelist");
    }

    //////////////////////////////////////////////////////////////////
    /// Share settings
    ///

    public override void SetVideoTitle(string title)
    {
        kamcordJavaClass().CallStatic("setDefaultVideoTitle", title);
    }

    public override void SetYouTubeSettings (string description, string tags)
    {
        kamcordJavaClass().CallStatic("setDefaultYoutubeDescription", description);
        kamcordJavaClass().CallStatic("setDefaultYoutubeKeywords", tags);
    }

    public override void SetFacebookDeveloperCredentials(string key, string secret)
    {
        kamcordJavaClass().CallStatic("setFacebookDeveloperCredentials", key, secret);
    }

    public override void SetNicoNicoDeveloperCredentials(string key, string secret)
    {
        kamcordJavaClass().CallStatic("setNicoNicoDeveloperCredentials", key, secret);
    }

    public override void SetDefaultNicoNicoDescription(string description)
    {
        kamcordJavaClass().CallStatic("setDefaultNicoNicoDescription", description);
    }

    public override void SetFacebookDescription(string facebookDescription)
    {
        kamcordJavaClass().CallStatic("setDefaultFacebookDescription", facebookDescription);
    }

    public override void SetDefaultTweet(string tweet)
    {
        kamcordJavaClass().CallStatic("setDefaultTweet", tweet);
    }

    public override void SetTwitterDescription(string twitterDescription)
    {
        kamcordJavaClass().CallStatic("setDefaultTwitterDescription", twitterDescription);
    }

    public override void SetDefaultEmailSubject(string subject)
    {
        kamcordJavaClass().CallStatic("setDefaultEmailSubject", subject);
    }

    public override void SetDefaultEmailBody(string body)
    {
        kamcordJavaClass().CallStatic("setDefaultEmailBody", body);
    }

    public override void SetShareTargets(
        Kamcord.ShareTarget target1,
        Kamcord.ShareTarget target2,
        Kamcord.ShareTarget target3,
        Kamcord.ShareTarget target4)
    {
        kamcordJavaClass().CallStatic("setShareTargets", new int[4] {(int) target1, (int) target2, (int) target3, (int) target4});
    }

    public override void AddMetadataToVideo(Kamcord.KCMetadata metadata)
    {
        AndroidJavaObject kamcordMetadata = null;
        if (metadata.type == Kamcord.KCMetadata.KCMetadataValueType.String)
        {
            kamcordMetadata = new AndroidJavaObject("com.kamcord.android.KamcordMetadata", metadata.key, metadata.stringValue);
        }
        else if (metadata.type == Kamcord.KCMetadata.KCMetadataValueType.Double)
        {
            kamcordMetadata = new AndroidJavaObject("com.kamcord.android.KamcordMetadata", metadata.key, metadata.doubleValue);
        }
        else if (metadata.type == Kamcord.KCMetadata.KCMetadataValueType.Integer)
        {
            kamcordMetadata = new AndroidJavaObject("com.kamcord.android.KamcordMetadata", metadata.key, metadata.integerValue);
        }
        if (kamcordMetadata != null && kamcordJavaClass() != null)
        {
            kamcordJavaClass().CallStatic("addMetadataToVideo", kamcordMetadata);
        }
    }

    public override void ShowFeedWithMetadataConstraintsAndTitle(IList<Kamcord.KCMetadata> metadata, string title, Kamcord.SortModel sortModel)
    {
        List<IDictionary> transformedMetadata = new List<IDictionary>();
        AndroidJavaObject list = new AndroidJavaObject("java.util.ArrayList");
        foreach (var obj in metadata)
        {
            AndroidJavaObject kamcordMetadata = null;
            if (obj.type == Kamcord.KCMetadata.KCMetadataValueType.String)
            {
                kamcordMetadata = new AndroidJavaObject("com.kamcord.android.KamcordMetadata", obj.key, obj.stringValue);
            }
            else if (obj.type == Kamcord.KCMetadata.KCMetadataValueType.Double)
            {
                kamcordMetadata = new AndroidJavaObject("com.kamcord.android.KamcordMetadata", obj.key, obj.doubleValue);
            }
            else if (obj.type == Kamcord.KCMetadata.KCMetadataValueType.Integer)
            {
                kamcordMetadata = new AndroidJavaObject("com.kamcord.android.KamcordMetadata", obj.key, obj.integerValue);
            }
            if (kamcordMetadata != null && list != null)
            {
                list.Call<Boolean>("add", kamcordMetadata);
            }
        }
        if (list != null && kamcordJavaClass() != null)
        {
            kamcordJavaClass().CallStatic("showMetadataView", list, title, (int)sortModel);
        }
    }

    public override void ShowFeedWithMetadataConstraintsAndTitle(IList<Kamcord.KCMetadata> metadata, string title)
    {
        ShowFeedWithMetadataConstraintsAndTitle(metadata, title, Kamcord.SortModel.Trending);
    }

    public override string Version()
    {
        return kamcordJavaClass().CallStatic<string>("version");
    }

    public override void SetApplicationLink (string link)
    {
        kamcordJavaClass().CallStatic("setApplicationLink", link);
    }

    public override void SetMode (int mode)
    {
        kamcordJavaClass().CallStatic("setMode", (long)mode);
    }

    //////////////////////////////////////////////////////////////////
    /// Video Recording
    ///

    private static bool beginDrawErrorOnce = false;

    public override void BeginDraw ()
    {
        if (kamcordJavaClass() == null)
        {
            if(!beginDrawErrorOnce)
            {
                Debug.Log (beginDrawErrorString);
                beginDrawErrorOnce = true;
            }
            return;
        }

        kamcordJavaClass().CallStatic(beginDrawString);
    }

    private static bool endDrawErrorOnce = false;

    private bool callEndDrawOnJavaClass()
    {
        if (kamcordJavaClass() == null)
        {
            if(!endDrawErrorOnce)
            {
                Debug.Log (endDrawErrorString);
                endDrawErrorOnce = true;
            }
            return false;
        }

        kamcordJavaClass().CallStatic(endDrawString);
        return true;
    }

    public override void EndDraw ()
    {
        bool success = callEndDrawOnJavaClass();

        // In some versions of Unity, the "default" framebuffer
        // (what the Unity treats as the screen) may be different
        // than 0. In those scenarios, Kamcord needs to know about
        // the default framebuffer. It is assumed that when end draw is called
        // that the currently bound framebuffer is the framebuffer that is the
        // "default", so pass that information to Kamcord here
        if ( success )
        {
            // Yes, call this every frame in case Unity changes
            // the rendering framebuffer.
            kamcordJavaClass().CallStatic<bool>(setCurrentlyBoundFBString);
        }
    }

    public override void StartRecording ()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: StartRecording called with no kamcordJavaClass().");
            return;
        }

        kamcordJavaClass().CallStatic("startRecording");
    }

    public override void StopRecording ()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: StopRecording called with no kamcordJavaClass().");
            return;
        }

        kamcordJavaClass().CallStatic("stopRecording");
    }

    public override void Pause ()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: Pause called with no kamcordJavaClass().");
            return;
        }

        kamcordJavaClass().CallStatic("pauseRecording");
    }

    public override void Resume ()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: Resume called with no kamcordJavaClass().");
            return;
        }

        kamcordJavaClass().CallStatic("resumeRecording");
    }

    public override bool IsRecording ()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: IsRecording called with no kamcordJavaClass().");
            return false;
        }

        return kamcordJavaClass().CallStatic<bool>(isRecordingString);
    }

    public override bool IsPaused ()
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: isPaused called with no kamcordJavaClass().");
            return false;
        }

        return kamcordJavaClass().CallStatic<bool>("isPaused");
    }

    public override void SetVideoQuality (Kamcord.VideoQuality quality)
    {
        kamcordJavaClass().CallStatic("setVideoQuality", quality);
    }

    public override bool VoiceOverlayEnabled ()
    {
        return kamcordJavaClass().CallStatic<bool>("voiceOverlayEnabled");
    }

    public override bool VoiceOverlayActivated ()
    {
        return kamcordJavaClass().CallStatic<bool>("voiceOverlayActivated");
    }

    public override void SetVoiceOverlayEnabled (bool enabled)
    {
        kamcordJavaClass().CallStatic("setVoiceOverlayEnabled", enabled);
    }

    public override void ActivateVoiceOverlay (bool activate)
    {
        kamcordJavaClass().CallStatic("setVoiceOverlayActivated", activate);
    }

    public override void SetVideoIncompleteWarningEnabled (bool enabled)
    {
        kamcordJavaClass().CallStatic("setVideoIncompleteWarningEnabled", enabled);
    }

    public override void TurnOffAutomaticAudioRecording (bool state)
    {
        kamcordJavaClass().CallStatic("setAutomaticAudioRecording", !state);
    }

    public override bool IsVideoComplete()
    {
        return kamcordJavaClass().CallStatic<bool>("isVideoComplete");
    }

    public override void CaptureFrame ()
    {
        callEndDrawOnJavaClass();
    }

    public override void ShowView ()
    {
        kamcordJavaClass().CallStatic("showView");
    }

    public override void ShowWatchView ()
    {
        kamcordJavaClass().CallStatic("showWatchView");
    }

    public override void Init (
        string devKey,
        string devSecret,
        string appName,
        Kamcord.VideoQuality videoQuality)
    {
        if (kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: Init called with no kamcordJavaClass().");
            return;
        }

        int n = 1;
        switch(videoQuality)
        {
            default:
            case Kamcord.VideoQuality.Standard:
                n = 0;
                break;
            case Kamcord.VideoQuality.Trailer:
                n = 1;
                break;
        }

        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activityJavaObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        kamcordJavaClass().CallStatic("initActivity", activityJavaObject);
        kamcordJavaClass().CallStatic("initKeyAndSecret", devKey, devSecret, appName, n);
    }

    public override void Awake(Kamcord kamcordInstance)
    {
        if(kamcordJavaClass() == null)
        {
            Debug.Log ("Kamcord: Java class not accessible from C#.");
        }
    }

    public override void Start(Kamcord kamcordInstance)
    {
        Kamcord.CallAdjustAndroidWhitelist ();

        SetLoggingEnabled(Kamcord.loggingEnabled_);

        if ( Kamcord.getUseCompatibilityMode() )
        {
            kamcordJavaClass().CallStatic("enterCompatibilityMode");
        }

        if ( kamcordJavaClass() == null )
        {
            Debug.Log ("Kamcord: Java class not accessible from C#.");
        }
        else
        {
            Init(
                kamcordInstance.developerKey,
                kamcordInstance.developerSecret,
                kamcordInstance.appName,
                kamcordInstance.videoQuality);

            if ( Kamcord.getUseCompatibilityMode() )
            {
                InitializeRenderCamera("Pre");
                InitializeRenderCamera("Post");
            }
        }
    }

    public override void SetAudioSettings(int sampleRate, int numChannels)
    {
        kamcordJavaClass().CallStatic("setAudioSettings", sampleRate, numChannels);
    }

    public override void WriteAudioData(float [] data, int length)
    {
        kamcordJavaClass().CallStatic("writeAudioData", data, length);
    }

    private void InitializeRenderCamera(string type)
    {
        if( type.Equals("Pre") || type.Equals("Post") )
        {
            // Set up the render camera, if it doesn't already exist.
            if( GameObject.Find("kamcord" + type + "Camera") == null )
            {
                GameObject cameraObject = new GameObject();
                Camera camera = (Camera) cameraObject.AddComponent<Camera>();
                camera.name = "kamcord" + type + "Camera";
                camera.clearFlags = CameraClearFlags.Nothing;
                camera.cullingMask = 0;

                if( type.Equals("Pre") )
                {
                    camera.depth = Single.MinValue;
                    camera.gameObject.AddComponent<KamcordAndroidPreRender>();
                }
                else // Post
                {
                    camera.depth = Single.MaxValue;
                    camera.gameObject.AddComponent<KamcordAndroidPostRender>();
                }

                // Keep this fella around.
                cameraObject.SetActive(true);
                UnityEngine.Object.DontDestroyOnLoad(cameraObject);
            }
        }
    }

    public static void SetRenderCameraEnabled(string type, bool flag)
    {
        if( type.Equals("Pre") || type.Equals("Post") )
        {
            GameObject renderCamera = GameObject.Find ("kamcord"+type+"Camera");
            if( renderCamera != null )
            {
                renderCamera.SetActive(flag);
            }
        }
    }

    public static int getSDKVersion()
    {
        AndroidJavaClass vJavaClass = new AndroidJavaClass ("android.os.Build$VERSION");
        return vJavaClass.GetStatic<int>("SDK_INT");
    }
}

#endif
