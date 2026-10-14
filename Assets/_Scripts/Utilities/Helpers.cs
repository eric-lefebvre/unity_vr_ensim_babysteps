using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers 
{
    public static void DestroyChildren(this Transform t) {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

    public static List<T> GetComponentsInDirectChildren<T>(Transform parent) where T : Component
    {
        List<T> components = new List<T>();
        foreach (Transform child in parent)
        {
            T component = child.GetComponent<T>();
            if (component != null)
                components.Add(component);
        }
        return components;
    }
}
