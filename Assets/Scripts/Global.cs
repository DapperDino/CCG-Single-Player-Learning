using System;
using UnityEngine;

public static class Global
{
    public static int GenerateID<T>()
    {
        return GenerateID(typeof(T));
    }
    public static int GenerateID(Type type)
    {
        return Animator.StringToHash(type.Name);
    }
    public static string PrepareNotification<T>()
    {
        return PrepareNotification(typeof(T));
    }
    public static string PrepareNotification(Type type)
    {
        return string.Format("{0}.PrepareNotification", type.Name);
    }
    public static string PerformNotification<T>()
    {
        return PerformNotification(typeof(T));
    }
    public static string PerformNotification(Type type)
    {
        return string.Format("{0}.PerformNotification", type.Name);
    }
}
