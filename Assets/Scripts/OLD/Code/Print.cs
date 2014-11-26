using UnityEngine;
using System.Collections;
using System.IO;

public class Print : MonoBehaviour 
{
	
	[SerializeField]
	private Sprite PhotoSaved;
	void OnClick()
	{
		Texture2D screenshot = Resources.Load<Texture2D>("printOut");
		var bytes = screenshot.EncodeToPNG();
		string filepath = Application.persistentDataPath + "/printOut.png";
		Debug.Log("Photo saved to: " + filepath);
		File.WriteAllBytes(filepath, bytes);
#if UNITY_IOS
		EtceteraBinding.saveImageToPhotoAlbum(filepath);
#elif UNITY_ANDROID
		bool saved = EtceteraAndroid.saveImageToGallery(filepath,"printOut.png");
		if(saved)
		{
			Debug.Log("Image moved to gallery");
		}
		else
		{
			Debug.Log("Image not moved");
		}
#endif
		PopPhotoSaved();

	}
	void PopPhotoSaved()
	{
//		if (PhotoSaved != null && PhotoSaved.GetComponent<PhotoFadeOut> () != null) {
//			PhotoSaved.gameObject.SetActive(true);
//		}
	}
}
