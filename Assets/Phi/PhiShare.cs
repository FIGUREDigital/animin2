using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Phi
{
	public class PhiShare
	{		
		[DllImport ("__Internal")]
		private static extern void ShareImageI(string image64, float x, float y, float w, float h);

		static public void Image(Texture2D texture, Rect from) 
		{
			#if (UNITY_IPHONE && !UNITY_EDITOR)			
			float x = (from.xMin / Screen.width) + 0.5f;
			float y = (from.yMin / Screen.height) +0.5f;
			float w = from.size.x / Screen.width;
			float h = from.size.y / Screen.height;
			byte[] imageBytes = texture.EncodeToPNG();
			string image64 = System.Convert.ToBase64String (imageBytes);
			ShareImageI(image64, x, y, w, h);
			#endif
		}

	}
}