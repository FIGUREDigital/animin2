using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RadioUI : MonoBehaviour {

	[SerializeField]
	float[] angles;
	
	[SerializeField]
	Transform knob;

	// Use this for initialization
	void OnEnable()
	{
		knob.localEulerAngles = new Vector3(0,0,angles[CurSetting]);
	}

	int CurSetting
	{
		get
		{
			switch(Music.Instance.CurentList)
			{
			case Music.Playlists.Radio1:
				return 1;				
			case Music.Playlists.Radio2:
				return 2;
			}
			return 0;
		}
	}

	public void OnTap()
	{
		if (BoomBoxUI.IsPlaying)
		{
			BoomBoxUI.DoStop();
		}
		int setting = (CurSetting + 1)%angles.Length;
		//float angle = angles[setting];
		knob.DOLocalRotate(new Vector3(0,0,angles[setting]),0.25f).SetEase(Ease.InOutCubic);
		switch(setting)
		{
			case 0:
				Music.Instance.Play(Music.Playlists.Caring, true);
				break;
			case 1:
				Music.Instance.Play(Music.Playlists.Radio1, true);
				break;
			case 2:
				Music.Instance.Play(Music.Playlists.Radio2, true);
				break;
		}
	}
}
