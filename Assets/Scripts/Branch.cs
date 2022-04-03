using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public enum Option
    {
        VERTICAL,
        HORIZONTAL,
    }
    public Option orientation;

    public bool canTraverseDirection(Vector2 dir)
    {
        switch (orientation)
        {
            case Option.VERTICAL:
                return dir == Vector2.up || dir == Vector2.down;
            case Option.HORIZONTAL:
                return dir == Vector2.left || dir == Vector2.right;
            default:
                throw new System.NotImplementedException($"Unknown option: {orientation}");
        }
    }
}
