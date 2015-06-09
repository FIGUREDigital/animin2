using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[ExecuteInEditMode]
// Simple class to forward when the game object it is on has it's transform change.
public class UIForwardTransformChanges : UIBehaviour
{
    public Action onTransformChanged;
    public RectTransform rectT;

    Vector3 prevPosition;   // Used to spot change in position.
    Vector2 center;
    Vector2 size;
//    Rect prevRect;
    protected override void Awake()
    {
        rectT = transform as RectTransform;
//        prevRect = rectT.rect;
        center = rectT.rect.center;
        size = rectT.rect.size;
        //hideFlags = HideFlags.HideAndDontSave;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        if (pos != prevPosition || center != rectT.rect.size || size != rectT.rect.size)
        {
            prevPosition = pos;
            center = rectT.rect.center;
            size = rectT.rect.size;
            if(onTransformChanged != null)
            {
                onTransformChanged();
            }
        }
    }
    /*
    protected override void OnRectTransformDimensionsChange()
    {
//        Debug.Log("OnRectTransformDimensionsChange");
        if (onTransformChanged != null)
        {
            onTransformChanged();
        }
    }

    protected override void OnTransformParentChanged()
    {
//        Debug.Log("OnTransformParentChanged");
        if (onTransformChanged != null)
        {
            onTransformChanged();
        }
    }*/
}
