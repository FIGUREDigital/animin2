using UnityEngine;
using System.Collections;
using System;
public abstract class UITextRenderer 
{
    public abstract MonoBehaviour TextRendererComponent
    {
        get;
    }

    public abstract string Text
    {
        get;
        set;
    }

    public abstract Color Color
    {
        get;
        set;
    }
    public abstract Material Material
    {
        get;
        set;
    }

    public abstract UITextRenderer Create(GameObject go);

}
