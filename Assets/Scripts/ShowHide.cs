using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;


public class ShowHide : MonoBehaviour {

	public Transform showHideThis;
	public Vector3 offset;
	public bool startHidden = true;
	public float duration = 0.25f;
	public Ease easeIn = Ease.OutCubic;
	public Ease easeOut = Ease.InCubic;
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
	Vector3 visiblePosition;
	Vector3 hiddenPosition;

	void Awake()
	{
		visiblePosition = showHideThis.transform.localPosition;
	}

	void OnEnable()
	{
		showHideThis.gameObject.SetActive(showHideThis);
		Vector3 pos = visiblePosition + offset;
		showHideThis.transform.localPosition = pos;
		currentTarget = false;
		if (showHideThis) 
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
	public void Show(bool show, Action onFinish = null) 
	{
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
		Vector3 pos = visiblePosition;
		if(!show)
		{
			pos += offset;
		}
		if(show)
		{
			showHideThis.gameObject.SetActive(true);
		}
		showHideThis.DOLocalMove(pos, duration).SetEase(show ? easeIn : easeOut).OnComplete(OnComplete);
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
