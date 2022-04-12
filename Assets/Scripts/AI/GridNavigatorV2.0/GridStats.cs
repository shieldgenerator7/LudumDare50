using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStats : MonoBehaviour
{
    //visited is how many moves it takes to get here
    public float scale = 1;
    public int x;
    public int y;
    public int visited
    {
        get => throw new System.Exception("Use VisitData.visited instead");
        set => throw new System.Exception("Use VisitData.visited instead");
    }
}
