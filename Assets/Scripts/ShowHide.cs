using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;


public class ShowHide : MonoBehaviour {

	public Transform showHideThis;
	public Vector3 offset;
	public bool scaleToZero = false;
	public bool startHidden = true;
	public float duration = 0.25f;
	public float durationHide = 0f;
	public Ease easeIn = Ease.OutCubic;
	public Ease easeOut = Ease.InCubic;
	public float showDelay = 0;
	public bool CurTarget
	{
		get
		{
			return currentTarget;
		}
		set
		{
			Show(value);
		}
	}

	Action onFinish = null;

	bool currentTarget = false;
	Tween tween = null;
	Vector3 defaultVec;
	Vector3 hiddenPosition;
	static Vector3 nearlyZero = new Vector3(0.001f, 0.001f, 0.001f);

	void Awake()
	{
		if (durationHide == 0) {
			durationHide = duration;
		}
		defaultVec = scaleToZero ? showHideThis.transform.localScale : showHideThis.transform.localPosition;
	}

	void OnEnable()
	{
		showHideThis.gameObject.SetActive(false);
		currentTarget = false;
		if (scaleToZero) 
		{
			showHideThis.transform.localScale = nearlyZero;
		} 
		else 
		{			
			Vector3 pos = defaultVec + offset;
			showHideThis.transform.localPosition = pos;
		}
		if (!startHidden) 
		{
			Show(true);
		}
	}

	void OnDisable()
	{
		if (tween != null)
		{
			tween.Kill ();
			if (onFinish != null)
			{
				Debug.LogError ("Warning "+name+" disabled while show hide tween in progress - onFinish delegates will not be called!");
			}
			onFinish = null;
		}
	}

	// Use this for initialization
	public void Show(bool show, bool instant = false, Action onFinish = null) 
	{
		Vector3 dest;
		if (scaleToZero) 
		{
			dest = show ? defaultVec : nearlyZero;
		}
		else 
		{
			dest = show ? defaultVec : defaultVec + offset;
		}

		if (instant) 
		{
			if (tween != null)
			{
				tween.Kill();
				tween = null;
			}
			showHideThis.gameObject.SetActive(show);
			if (scaleToZero) 
			{
				showHideThis.localScale = dest;
			}
			else 
			{
				showHideThis.localPosition = dest;
			}
			currentTarget = show;
		}
		if (currentTarget == show)
		{
			if(onFinish != null)
			{
				onFinish();
			}
			return;
		}
		if (onFinish != null) {
			this.onFinish += onFinish;
		}
		currentTarget = show;
		if (tween != null)
		{
			tween.Kill();
			tween = null;
		}
		if(show)
		{
			showHideThis.gameObject.SetActive(true);
		}
		if (scaleToZero) 
		{
			if(show)
			{
				tween = showHideThis.DOScale (dest , duration).SetEase (easeIn).OnComplete (OnComplete);
			}
			else
			{
				tween = showHideThis.DOScale (dest , durationHide).SetEase (easeOut).OnComplete (OnComplete);
			}
		}
		else 
		{
			if(!show)
			{
				tween = showHideThis.DOLocalMove (dest, duration).SetEase (easeIn).OnComplete (OnComplete);
			}
			else
			{				
				tween = showHideThis.DOLocalMove (dest, durationHide).SetEase (easeOut).OnComplete (OnComplete);
			}
		}
		if (tween != null && show) 
		{
			tween.SetDelay(showDelay);
		}
	}

	void OnComplete()
	{
		if (!currentTarget) 
		{			
			showHideThis.gameObject.SetActive(false);
		}
		if (onFinish != null) 
		{
			onFinish();
		}
		onFinish = null;
	}
}
