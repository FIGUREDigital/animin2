using UnityEngine;
using System.Collections;

public static class Extensions
{
    public static int Clamp(
        this int value, int inclusiveMinimum, int inclusiveMaximum)
    {
        if (value <= inclusiveMinimum) { return inclusiveMinimum; }
        if (value >= inclusiveMaximum) { return inclusiveMaximum; }
        return value;
    }


    public static float Clamp(
        this float value, float inclusiveMinimum, float inclusiveMaximum)
    {
        if (value <= inclusiveMinimum) { return inclusiveMinimum; }
        if (value >= inclusiveMaximum) { return inclusiveMaximum; }
        return value;
    }

    public static Color ReplaceAlpha(this Color c, float a)
    {
        c.a = a;
        return c;
    }

    public static string Path(this GameObject obj, GameObject relativeTo)
    {
        return obj.transform.Path(relativeTo.transform);
    }

    public static string Path(this GameObject obj)
    {
        return obj.transform.Path(null);
    }

    public static string Path(this Component comp)
    {
        return comp.transform.Path(null);
    }

    public static string Path(this Transform t, Transform relativeTo)
    {
        if (t == relativeTo) return "";
        string path = t.name;
        while (t.parent != null && t.parent != relativeTo)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
