using UnityEngine;
using System.Collections;

public class ArCameraManager : MonoBehaviour {

	#region Singleton

	private GameObject Go;
	private bool mInitialized;
	private static ArCameraManager s_Instance;
	
	public static ArCameraManager Instance
	{
		get
		{
			if ( s_Instance == null )
			{
				s_Instance = new ArCameraManager();
			}
			return s_Instance;
		}
	}
	
	#endregion
	void Awake()
	{
		Init();
	}

	void OnLevelWasLoaded(int level) 
	{
		if(level == 0)
		{
		}
		UIGlobalVariablesScript.Singleton.ARCamera = Go;
		//UIGlobalVariablesScript.Singleton.DragableUI3DObject = Go.GetComponentInChildren<CameraModelScript>().gameObject;
	}

	void Init()
	{
		if(mInitialized)
		{
			return;
		}
		mInitialized = true;
		Go = (GameObject)Instantiate(Resources.Load("Prefabs/ARCamera"));
		this.transform.parent = Go.transform;
		if(UIGlobalVariablesScript.Singleton != null)
		{
			UIGlobalVariablesScript.Singleton.ARCamera = Go;
		}
	}
}
