using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Returns a list of child game objects that have the given string in its name
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<GameObject> getChildrenWithName(this GameObject go, string name)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform transform in go.transform)
        {
            if (transform.name.Contains(name))
            {
                children.Add(transform.gameObject);
            }
        }
        return children;
    }

    /// <summary>
    /// Return the first child with the given string in its name
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject getChildWithName(this GameObject go, string name)
    {
        List<GameObject> children = go.getChildrenWithName(name);
        if (children.Count > 0)
        {
            return children[0];
        }
        return null;
    }

    public static void DeleteChildrenWithName(this GameObject go, string[] names)
    {
        go.DeleteChildrenWithName(names.ToList());
    }

    public static void DeleteChildrenWithName(this GameObject go, List<string> names)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (string name in names)
        {
            children.AddRange(go.getChildrenWithName(name));
        }
        children.ForEach(child => Object.Destroy(child));
    }
}
