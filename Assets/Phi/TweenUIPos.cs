using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TweenUIPos : MonoBehaviour {

    Vector2 fromOffset = Vector2.zero;
    //Vector2 toOffset = Vector2.zero;
	// Use this for initialization
	void OnEnable () 
    {
        RectTransform rt = transform as RectTransform;
        //rt.an
        Vector2 pos = rt.anchoredPosition;
        rt.anchoredPosition = pos + fromOffset;
        //DOT
        //rt.
        //rt.DOAnchorPos(pos + toOffset);
	}
}
