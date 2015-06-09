using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MagnifyBehaviour : MonoBehaviour 
{
	public TextMeshProUGUI plusMinus;

    void Start()
    {
        TutorialHandler.ShouldSkipLesson += ShouldSkipZoomOutLesson;
    }

    void OnDestroy()
    {
        TutorialHandler.ShouldSkipLesson -= ShouldSkipZoomOutLesson;
    }

    void ShouldSkipZoomOutLesson(string skipID)
    {
        if (skipID == "ShouldSkipZoomOut")
        {
            if (ZoomBehaviour.tapZoom)
            {
                TutorialHandler.ShouldSkip = true;
            }
        }
    }

	public void OnClick()
	{
		ZoomBehaviour.tapZoom = !ZoomBehaviour.tapZoom;
		ZoomBehaviour.canTapZoom = true;
	}

	void Update()
	{
		if(plusMinus != null)
		{
			string pm = "+";
			if(!ZoomBehaviour.tapZoom)
			{
				pm = "-";
			}
			if (plusMinus.text != pm)
			{
				plusMinus.text = pm;
			}
		}
	}

}
