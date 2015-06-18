using System;
using UnityEngine;
[ExecuteInEditMode]
public class UtilsDebug : MonoBehaviour
{
    [Serializable]
    public class Breakpoint
    {
        public bool pause = false;
        public bool log = false;
        public void Hit(string message)
        {
            if (pause)
            {
                Debug.Break();
            }
            if(log)
            {
                Debug.Log(message);
            }
        }
    }
    [Header("Breakpoints")]
    public Breakpoint start;
    public Breakpoint awake;
    public Breakpoint onEnable;
    public Breakpoint onDisable;
    public Breakpoint onDestroy;

#if UNITY_EDITOR
    void Start()
    {
        start.Hit("Start " + gameObject.Path());
    }
    void Awake()
    {
        if (awake!=null)
        {
            awake.Hit("Awake " + gameObject.Path());
        }
    }
    void OnEnable()
    {
        onEnable.Hit("OnEnable " + gameObject.Path());
    }
    void OnDestroy()
    {
        onDestroy.Hit("OnDestroy " + gameObject.Path());
    }

    void OnDisable()
    {
        onDisable.Hit("OnDisable " + gameObject.Path());
    }
#endif
	// http://answers.unity3d.com/questions/19122/assert-function.html

    [System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void Assert(bool condition, string message)
	{
		if (!condition)
		{
			throw new Exception(message);
//			UnityEngine.Debug.LogError(message);// Do this as well as throw an exception in case exceptions are switched off.
		}
	}
}
