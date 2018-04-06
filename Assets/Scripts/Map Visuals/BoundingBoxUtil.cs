using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class for creating bounding box for rivers and terrains
/// </summary>

public static class BoundingBoxUtil{

    private static int maxX, minX, maxY, minY = 0;
     
    public static List<int> BoundingBox(List<DisplayNode> displaynode)   {

        List<int> BoundingBox = new List<int>();
        minX = displaynode.Max(p=> p.x);
        BoundingBox.Add(minX);
        minY = displaynode.Min(p => p.y);
        BoundingBox.Add(minY);
        maxX = displaynode.Max(p => p.x);
        BoundingBox.Add(maxX);
        maxY = displaynode.Max(p => p.y);       
        BoundingBox.Add(minY);

        return BoundingBox;

    }    
}