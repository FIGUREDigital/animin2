using UnityEngine;
using System.Collections;
using Prime31;

public class Picture : MonoBehaviour {

	public GameObject customPicture;
	public Material customMaterial;
	// Use this for initialization
	void Start () {
		Debug.Log ("Picture Start");
		#if UNITY_IOS
		EtceteraManager.imagePickerChoseImageEvent += ImagePicked;
		#endif
	}

	void OnDestroy()
	{		
		Debug.Log ("Picture OnDestroy");
		#if UNITY_IOS
		EtceteraManager.imagePickerChoseImageEvent -= ImagePicked;
		#endif
	}

	public void PromptForPhoto()
	{		
		#if UNITY_IOS
		EtceteraBinding.promptForPhoto(0.25f, PhotoPromptType.Album);
		#endif
	}
	
	#if UNITY_IPHONE
	void ImagePicked(string path)
	{
		Debug.Log ("ImagePicked ");	
		Debug.Log ("Picture "+path);		
		StartCoroutine( EtceteraManager.textureFromFileAtPath( path, textureLoaded, textureLoadFailed ) );
	}
	
	public void textureLoaded( Texture2D texture )
	{
		Debug.Log ("textureLoaded "+texture.texelSize);	
		customMaterial.mainTexture = texture;
		customPicture.gameObject.SetActive (true);
	}
	
	public void textureLoadFailed( string error )
	{
		Debug.Log ("textureLoadFailed "+error);	
		var buttons = new string[] { "OK" };
		EtceteraBinding.showAlertWithTitleMessageAndButtons( "Error Loading Texture.  Did you choose a photo first?", error, buttons );
		Debug.Log( "textureLoadFailed: " + error );
	}
	
	#endif
}
