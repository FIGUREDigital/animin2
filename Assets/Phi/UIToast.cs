using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using DG.Tweening;
namespace Phi
{
public class UIToast : SingletonScene<UIToast>
{
	public UIText title;
    public UIText description;
    public UIImagePro image;
    public float displayForSeconds = 1.5f;
    public Vector3 hideVector = new Vector3(0, 300, 0);
    public GameObject normalToast;
	public Gradient backgroundGradient;
    private bool allowToast = false;    // Set to true once booted up
	ShowHide showHide;
	bool hiddenTutorial = false;
    //public RectTransform tick;
	
	public class DisplayData
	{
        public int challengeID = -1;
		public string desc;
		public string title;
        public Sprite sprite;
		public Color color;
		public Action<object> onDisplay = null;
		public object userData = null;
		public string audioID = "";
	}
	
	private List<DisplayData> display = new List<DisplayData>();
	
	private Vector3 visiblePos;
	private Tween tween = null;
	
	public override void Init()
	{
		showHide = GetComponentInChildren<ShowHide> ();
		visiblePos = transform.localPosition;
        ShowHide(false, true);
		
		StopCoroutine("HideInABit");
	}
	
    public void HideToasts(bool goalsOnly = false)
    {
        for(int i = 1; i < display.Count; i++)
        {
            if (!goalsOnly || display[i].challengeID < 0)
            {
                display.RemoveAt(i);
                i--;
            }
        }
        if (display.Count > 0)
        {
            if (!goalsOnly || display[0].challengeID < 0)
            {
                ShowHide(false, false);
            }
        }
    }

	public void ShowHide(bool show, bool instant)
	{		
		if (show && !hiddenTutorial) 
		{
			TutorialHandler.Hide (true);
			hiddenTutorial = true;
		}
		if (showHide != null) 
		{
			if (show)
			{
				showHide.Show (show, instant, TweenCompleteShow);
			}
			else
			{
				showHide.Show (show, instant, TweenCompleteHide);
			}
		} 
		else 
		{
			Vector3 dest = visiblePos;
			if (!show) {
				dest += hideVector;
			}
			if (tween != null) {
				tween.Kill ();
				tween = null;
			}
			if (instant) {
				transform.localPosition = dest;
				gameObject.SetActive (show);
			} else {
				if (show) {
					gameObject.SetActive (true);				
					tween = transform.DOLocalMove (dest, 0.5f).SetEase (Ease.OutQuart).OnComplete (TweenCompleteShow).SetUpdate (true);
				} else {
					tween = transform.DOLocalMove (dest, 0.5f).SetEase (Ease.InQuart).OnComplete (TweenCompleteHide).SetUpdate (true);
				}
			}
		}
		if (show)
		{
			DisplayData data = display[0];
            bool isGoal = data.challengeID != -1;
            normalToast.SetActive(!isGoal);

            title.Text = data.title;
            description.Text = data.desc;
			backgroundGradient.vertex1 = data.color;
            if (image)
            {
                image.sprite = data.sprite;
            }
			if(!string.IsNullOrEmpty(data.audioID))
			{
				AudioController.Play(data.audioID);
			}
		}
	}
	
	private void TweenCompleteHide()
	{
//		if (transform.localPosition.y > visiblePos.y)
		{
			//Hidden
			// Remove id
			display.RemoveAt(0);
			// Check to see if there's another to show.
			if (display.Count > 0)
			{
				ShowHide(true, false);
			}
			else
			{
				// No more so disable objects
				gameObject.SetActive(false);			
				if (hiddenTutorial) 
				{
					TutorialHandler.Hide(false);	
					hiddenTutorial = false;
				}
			}
		}
	}
	private void TweenCompleteShow()
	{
		// Shown now wait for a while before hiding it again...
		StartCoroutine("HideInABit");
	}
	
	private IEnumerator HideInABit()
	{
        float endTime = Time.realtimeSinceStartup + displayForSeconds;
        while (Time.realtimeSinceStartup < endTime)
        {
            yield return new WaitForEndOfFrame();
        }
		ShowHide(false, false);
	}

    public void OnHide()
    {
        ShowHide(false, false);
    }
	
	public delegate void NotificationDisplayedDelegate(object userData);
	
	public DisplayData Display(string title, string desc, Sprite sprite, string audioID, Action<object> onDisplay = null, object userData = null)
    {
		return (Display(title, desc, sprite, backgroundGradient.vertex2, audioID, onDisplay, userData));
	}

	public DisplayData Display(string title, string desc, Sprite sprite, Color color, string audioID, Action<object> onDisplay = null, object userData = null)
	{
		Debug.Log("Display " + title + ", " + desc);
		
		DisplayData data = new DisplayData();
		data.title = title;
		data.desc = desc;
		data.sprite = sprite;
		data.userData = userData;
		data.audioID = audioID;
		data.color = color;
		Display(data);
		return data;
	}

    public void DisplayChallenge(int challengeID)
    {
        DisplayData data = new DisplayData();
        data.challengeID = challengeID;
        Display(data);
    }
	
	public void AllowToast()
	{
		// Show first item if we have one
        allowToast = true;
		if(display.Count > 0)
		{			
			ShowHide(false, true);	// Place off screen
			ShowHide(true,false);
		}
	}
	
	private void Display(DisplayData data)
	{
		if (!allowToast)
		{
			return;	// Do not show yet!
		}

        display.Add(data);          // some achievements (such as the first one don't want to be shown at all)
		if (display.Count == 1)
		{
			// Not already displaying so enable
			ShowHide(false, true);	// Place off screen
			ShowHide(true,false);
		}
	}
	
	void OnRelease()
	{
		StopCoroutine("HideInABit");
		ShowHide(false, false);
	}
}
}
