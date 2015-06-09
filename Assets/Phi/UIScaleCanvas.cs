using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Canvas))]
[ExecuteInEditMode]
public class UIScaleCanvas : MonoBehaviour 
{
    Canvas c;
	// Use this for initialization
    void OnEnable()
    {
        c = GetComponent<Canvas>();
        UpdateAspect();
    }

	// Update is called once per frame
	public void UpdateAspect() 
    {
        if (c != null && c.worldCamera)
        {
            float curAspect = c.worldCamera.aspect;
            if (Application.isPlaying)
            {
                UIScreenTransitionCamera tc = c.worldCamera.GetComponent<UIScreenTransitionCamera>();
                if (tc != null && tc.fullAspect != 0)
                {
                    curAspect = tc.fullAspect;
                }
            }
            RectTransform rt = (c.transform as RectTransform);
            Vector2 size = rt.sizeDelta;
//            Vector2 orig = size;
            size.x = rt.sizeDelta.y * curAspect;
            rt.sizeDelta = size;
//            Debug.Log("Scale from " + orig + " to " + size);
        }
	}
}
