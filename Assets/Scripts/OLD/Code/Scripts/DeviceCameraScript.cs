using UnityEngine;
using System.Collections;

public class DeviceCameraScript : MonoBehaviour 
{

	//WebCamDevice[] devices;
	//WebCamTexture webcamTexture;

	//public bool IsFrontCameraFacing;


//	public WebCamDevice? FrontCamera
//	{
//		get
//		{
//			for(int i=0;i<devices.Length;++i)
//				if(devices[i].isFrontFacing)
//					return devices[i];
//
//			return null;
//		}
//	}
//
//	public WebCamDevice? BackCamera
//	{
//		get
//		{
//			for(int i=0;i<devices.Length;++i)
//				if(!devices[i].isFrontFacing)
//					return devices[i];
//
//			return null;
//		}
//	}

	public void ResetCamera()
	{
		/*webcamTexture.Stop();

		if(BackCamera.HasValue)
			webcamTexture.deviceName = BackCamera.Value.name;
		else if(FrontCamera.HasValue)
			webcamTexture.deviceName = FrontCamera.Value.name;
	
		webcamTexture.Play();
		IsFrontCameraFacing = false;*/
	}

	public void Stop()
	{
		/*if(webcamTexture.isPlaying)
			webcamTexture.Stop();*/
	}

	public void SwitchCamera()
	{
		/*if(!FrontCamera.HasValue || !BackCamera.HasValue) return;

		IsFrontCameraFacing = !IsFrontCameraFacing;
		webcamTexture.Stop();

		if(IsFrontCameraFacing) webcamTexture.name = FrontCamera.Value.name;
		else webcamTexture.name = BackCamera.Value.name;
		webcamTexture.Play();*/

	}

	public void SavePicture()
	{
		Application.CaptureScreenshot("Screenshot.png");
	}

	// Use this for initialization
	void Awake () 
	{
		//devices = WebCamTexture.devices;
		//webcamTexture = new WebCamTexture();
		//if(FrontCamera.HasValue) webcamTexture.name = FrontCamera.Value.name;
		//else if(BackCamera.HasValue) webcamTexture.name = BackCamera.Value.name;
		//webcamTexture.Play();
	}

	void OnGUI()
	{
		//Image img = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.RGBA8888);
	

		//UIGlobalVariablesScript.Singleton.Vuforia.camera

		/*if(webcamTexture != null && webcamTexture.isPlaying)
		{
			GUI.DrawTexture(new Rect(330, 0, 200, 200), webcamTexture);
		}*/
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
