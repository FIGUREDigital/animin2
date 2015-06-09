using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class UIScreenTransitionCamera : MonoBehaviour
{
    public static List<UIScreenTransitionCamera> cameras = new List<UIScreenTransitionCamera>();
    public bool cropViewPort;
    public Vector3 position;
    public float perspDist;
    public bool contentsBeingScrolledToo = true;// If contents are moving we need to adjust our position with them when cropping...
    private bool added = false;
    [System.NonSerialized]
    public float fullAspect = 0;
    public float offset = 0;
    public Vector3 debugScrollOffset = new Vector3(0,2000,0);
    public bool debugScale = false;
    public float fullSize;
    public Camera TheCamera
    {
        get
        {
            return cam;
        }
    }
    private Camera cam;
    public void PreAdd()
    {
        Add();
    }

    void Add()
    {
        if(!added)
        {
//            Debug.Log("UIScreenTransitionCamera " + name + " add");
            added = true;
            cameras.Add(this);
        }
    }
    void Awake()
    {
        position = transform.localPosition;
        cam = GetComponent<Camera>();
        fullAspect = cam.aspect;
        if (fullAspect <1)
        {
            Debug.Log("Aspect not landscape! converting");
            fullAspect =1/fullAspect;
        }
        fullSize = cam.orthographicSize;
    }

    void OnEnable()
    {
        Add();
    }

    void OnDisable()
    {
 //       Debug.Log("UIScreenTransitionCamera " + name + " disable");
        added = false;
        cameras.Remove(this);
    }

    public static UIScreenTransitionCamera GetCamera(int layer)
    {
        int mask = 1 << layer;
        for(int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i].cam != null && (cameras[i].cam.cullingMask & mask) != 0)
            {
                return cameras[i];
            }
        }
        return null;
    }

    public void UpdateCrop(float ratio, Vector3 scrollOffset, bool scale, bool contentsAlsoTransformed, bool force)
    {
        float scaleBy = 1;
        if(scale)
        {
            scaleBy = 1 - Mathf.Abs(ratio);
        }
        offset = ratio;
        if (cropViewPort || force)
        {
            Rect r = cam.rect;
            if (ratio <= 0)
            {
                if (ratio < -1)
                {
                    ratio = -1;
                }
                if (scrollOffset.x != 0)
                {
                    r.xMin = 0;
                    r.width = 1 + ratio * scaleBy;
                }
                else
                {
                    r.xMin = (1 - scaleBy) / 2;
                    r.width = scaleBy;
                }
                if (scrollOffset.y != 0)
                {
                    r.yMin = 0;
                    r.height = 1 + ratio * scaleBy;
                }
                else
                {
                    r.yMin = (1 - scaleBy) / 2;
                    r.height = scaleBy;
                }
            }
            else
            {                
                if (ratio > 1)
                {
                    ratio = 1;
                }
                if (scrollOffset.x != 0)
                {
                    r.xMin = ratio;
                    r.width = 1 - ratio * scaleBy;
                }
                else
                {
                    r.xMin = (1 - scaleBy) / 2;
                    r.width = scaleBy;
                }
                if (scrollOffset.y != 0)
                {
                    r.yMin = ratio;
                    r.height = 1 - ratio * scaleBy;
                }
                else
                {
                    r.yMin = (1 - scaleBy) / 2;
                    r.height = scaleBy;
                }
            }
            cam.rect = r;


            float osize = fullSize;
            if (cam.orthographic)
            {
                if (scrollOffset.y != 0)
                {
                    cam.orthographicSize = fullSize * (1 - Mathf.Abs(ratio* scaleBy));
                }
                else
                {
                    if (!contentsAlsoTransformed)
                    {
                        cam.orthographicSize = fullSize;// *scaleBy * scaleBy;
                    }
                    else
                    {
                        cam.orthographicSize = fullSize * scaleBy;
                    }
                }
            }

            if (!cam.orthographic)
            {
                osize = Mathf.Tan(cam.fieldOfView * Mathf.PI / 360.0f) * (perspDist - cam.transform.position.z);
            }


            if (!contentsBeingScrolledToo)
            {
                osize = -osize;
            };
            Vector3 pos = position;

            if (scrollOffset.x != 0)
            {
                pos += cam.transform.right * (fullAspect * ratio * osize);
            }
            if (scrollOffset.y != 0)
            {
                pos += cam.transform.up * (ratio * osize);
            }
            cam.transform.localPosition = pos;
            bool e = (Mathf.Abs(offset) < 1);
            if(cam.enabled != e)
            {
                cam.enabled = e;
            }
        }
    }
	
    void OnValidate()
    {
        if (Application.isPlaying) return;
        if (cam != null)
        {
            UpdateCrop(offset, debugScrollOffset, debugScale, false, false);
        }
    }
}
