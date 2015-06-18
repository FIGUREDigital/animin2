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
		public Gradient backgroundGradient;
		ShowHide showHide;
		bool hiddenTutorial = false;
	    //public RectTransform tick;
		
		public class DisplayData
		{
			public string desc;
			public string title;
	        public Sprite sprite;
			public Color color;
			public string audioID = "";		
		}
		
		private List<DisplayData> queue = new List<DisplayData>();
		private DisplayData showing = null;

		public override void Init()
		{
			showHide = GetComponentInChildren<ShowHide> ();
			showHide.Show(false, true);	
		}

		public void OnDisable()
		{
			if(hiddenTutorial) 
			{
				TutorialHandler.Hide (false);
				hiddenTutorial = false;
			}
			visibleFor = 0;
			showing = null;
		}

		public void OnEnable()
		{
			if(showing != null && queue.Count > 0)
			{				
				ShowHide(true,false);
			}
		}

		float visibleFor = 0;

		public void Update()
		{
			if(visibleFor > 0)
			{				
				visibleFor -= Time.deltaTime;
				if (visibleFor < 0)
				{
					ShowHide(false,false);
				}
			}
		}

		public void ShowHide(bool show, bool instant)
		{
			visibleFor = 0;
			if (show && !hiddenTutorial) 
			{
				TutorialHandler.Hide (true);
				hiddenTutorial = true;
			}
			if (show)
			{
				showHide.Show (show, instant, TweenCompleteShow);
			}
			else
			{
				showHide.Show (show, instant, TweenCompleteHide);
			} 
			if (show && queue.Count > 0)
			{
				showing = queue[0];
				queue.RemoveAt (0);

				title.Text = showing.title;
				description.Text = showing.desc;
				backgroundGradient.vertex2 = showing.color;
	            if (image)
	            {
					image.sprite = showing.sprite;
	            }
				if(!string.IsNullOrEmpty(showing.audioID))
				{
					AudioController.Play(showing.audioID);
				}
			}
		}
		
		private void TweenCompleteHide()
		{
			showing = null;
			// Check to see if there's another to show.
			if (queue.Count > 0)
			{
				ShowHide(true, false);
			}
			else
			{		
				if (hiddenTutorial) 
				{
					TutorialHandler.Hide(false);	
					hiddenTutorial = false;
				}
			}
		}
		private void TweenCompleteShow()
		{
			// Shown now wait for a while before hiding it again...
			visibleFor = displayForSeconds;
		}

	    public void OnHide()
	    {
	        ShowHide(false, false);
	    }
		
		public delegate void NotificationDisplayedDelegate(object userData);
		
		public DisplayData Display(string title, string desc, Sprite sprite, string audioID)
	    {
			return (Display(title, desc, sprite, backgroundGradient.vertex1, audioID));
		}

		public void Remove(DisplayData remove)
		{
			if(remove != null)
			{
				if (showing == remove)
				{
					if (showHide.CurTarget)
					{				
						ShowHide(false, false);
					}
				}
				if(queue.Contains (remove))
				{
					queue.Remove(remove);
				}
			}
		}

		public DisplayData Display(string title, string desc, Sprite sprite, Color color, string audioID)
		{
			DisplayData data = new DisplayData();
			data.title = title;
			data.desc = desc;
			data.sprite = sprite;
			data.audioID = audioID;
			data.color = color;

			Display(data);

			return data;
		}

		private void Display(DisplayData data)
		{
	        queue.Add(data);          // some achievements (such as the first one don't want to be shown at all)
			if (showing == null)
			{
				// Not already displaying so enable
				ShowHide(true,false);
			}
		}
		
		void OnRelease()
		{
			ShowHide(false, false);
		}
	}
}
