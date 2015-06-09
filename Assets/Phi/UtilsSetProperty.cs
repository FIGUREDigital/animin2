using UnityEngine;
using System.Collections;
using System;
static class UtilsSetProperty
{
    // Methods
    public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
    {
        if (((((T)currentValue) != null) || (newValue != null)) && ((((T)currentValue) == null) || !currentValue.Equals(newValue)))
        {
            currentValue = newValue;
            return true;
        }
        return false;
    }

    public static bool SetColor(ref Color currentValue, Color newValue)
    {
        if (((currentValue.r != newValue.r) || (currentValue.g != newValue.g)) || ((currentValue.b != newValue.b) || (currentValue.a != newValue.a)))
        {
            currentValue = newValue;
            return true;
        }
        return false;
    }

    public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
    {
        if (!currentValue.Equals(newValue))
        {
            currentValue = newValue;
            return true;
        }
        return false;
    }

}
