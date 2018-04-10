using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class for creating bounding box for rivers and terrains
/// </summary>

public static class BoundingBoxUtil{

    public static List<int> BoundingBox(List<DisplayNode> displaynode)   {

        int maxX, minX, maxY, minY = 0;

        List<int> BoundingBox = new List<int>();
        minX = displaynode.Min(p=> p.x);
        BoundingBox.Add(minX);
        minY = displaynode.Min(p => p.y);
        BoundingBox.Add(minY);
        maxX = displaynode.Max(p => p.x);
        BoundingBox.Add(maxX);
        maxY = displaynode.Max(p => p.y);       
        BoundingBox.Add(maxY);

        return BoundingBox;

    }    
}