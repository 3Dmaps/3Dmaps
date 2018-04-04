using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class for creating bounding box for rivers and terrains
/// </summary>

public static class BoundingBoxUtil{

    private static int maxX, minX, maxY, minY = 0;

    public static void BoundingBox(List<DisplayNode> displaynode)   {

    minX = displaynode.Max(p=> p.x);
    minY = displaynode.Min(p => p.y);
    maxX = displaynode.Max(p => p.x);
    maxY = displaynode.Max(p => p.y);    
}    
}