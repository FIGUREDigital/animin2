using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("UI/Copy Rect", 10)]
[ExecuteInEditMode]
public class UICopyRect : MonoBehaviour
{
    public RectTransform copy;
    public bool copyScale = false;
    private RectTransform curSrc;    // Source we are currently linked to.
    private UIForwardTransformChanges curSrcRect;    // Source we are currently linked to.

    protected DrivenRectTransformTracker tracker;

    int added = 0;
    bool nowEnabled = false;
#if UNITY_EDITOR
    void OnValidate()
    {
        if(!EditorApplication.isPlaying)
        {
            Link();
        }
    }
#endif

    void Link()
    {
        if (copy != curSrc)
        {
            if (curSrc != null)
            {
                // Remove link from current source

                if (nowEnabled)
                {
                    curSrcRect.onTransformChanged -= CopyRect;
                    added--;
                }
#if UNITY_EDITOR
                Object.DestroyImmediate(curSrcRect);
#else
                Object.Destroy(curSrcRect);
#endif
                curSrc = null;
                curSrcRect = null;
            }
            if (copy)
            {
                curSrc = copy;
                curSrcRect = copy.gameObject.GetComponent<UIForwardTransformChanges>();
                if (curSrcRect == null)
                {
                    Debug.Log("Create UIForwardTransformChanges");
                    curSrcRect = copy.gameObject.AddComponent<UIForwardTransformChanges>();
                    Debug.Log("Finish Create UIForwardTransformChanges");
                }
                if (nowEnabled)
                {
                    added++;
                    curSrcRect.onTransformChanged += CopyRect;
                    CopyRect();
                }
            }
        }
    }
    
    void OnDestroy()
    {
        if(added != 0)
        {
            Debug.LogError("Leaked delegate");
        }
        if (curSrcRect)
        {
#if UNITY_EDITOR
            // Work around to avoid object getting destroyed twice if switching scene in editor.
            UIForwardTransformChanges temp = curSrcRect;
            EditorApplication.delayCall += () =>
            {
                if (temp)
                {
                    Object.DestroyImmediate(temp);
                }
            };
#else
                Object.Destroy(curSrcRect);
#endif
            curSrcRect = null;
            curSrc = null;
        }
    }

    void OnEnable()
    {
        Link();
        nowEnabled = true;
        if (curSrcRect != null)
        {
            curSrcRect.onTransformChanged += CopyRect;
            added++;
            CopyRect();
        }
    }
    
    void CopyRect()
    {
        tracker.Clear();
        RectTransform dst = transform as RectTransform;
        tracker.Add(this, dst as RectTransform, DrivenTransformProperties.SizeDelta | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.AnchorMax | DrivenTransformProperties.AnchorMin);

        Vector2 min = curSrc.rect.min;
        Vector2 max = curSrc.rect.max;
        // Move bounds to world space
        min = curSrc.TransformPoint(min);
        max = curSrc.TransformPoint(max);
        // Now into our local space
        if (transform.parent != null)
        {
            min = transform.parent.InverseTransformPoint(min);
            max = transform.parent.InverseTransformPoint(max);
        }
        Vector2 pos = (min+max)/2;
        RectTransform parent = transform.parent as RectTransform;
        if (parent != null)
        {
            Vector2 size = parent.rect.size;

            Vector2 pivot = parent.pivot;
            pivot.Scale(size);
            size /= 2;
            pos += pivot - size;


        }
        dst.anchoredPosition = pos;
        dst.anchorMax = dst.anchorMin = new Vector2(0.5f,0.5f);
        dst.sizeDelta = max - min;
        if (copyScale)
        {
            dst.localScale = curSrc.localScale;
        }
    }

    void OnDisable()
    {
        nowEnabled = false;
        if (curSrcRect != null)
        {
            curSrcRect.onTransformChanged -= CopyRect;
            added--;
        }
        tracker.Clear();
    }
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Copy Rect", false, 5000)]
    static void Create()
    {
        if (Selection.activeGameObject == null) return;
        RectTransform rectT = Selection.activeGameObject.transform as RectTransform;
        if(rectT != null)
        {            
            GameObject go = new GameObject("CopyRect");
            go.AddComponent<UICopyRect>();
            go.transform.parent = rectT;
        }            
    }
#endif
}
