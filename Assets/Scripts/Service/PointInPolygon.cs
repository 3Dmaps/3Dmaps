using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>

/// </summary>

public static class PolygonService {

static bool PointInPolygon(DisplayNode p, List<DisplayNode> polygon)
{
//  int raw x and int raw y 

    DisplayNode p1, p2;
    bool inside = false;
    
    if (polygon.Count < 3)
    {
        return inside;
    }

    DisplayNode oldPoint = new DisplayNode(polygon[polygon.Count - 1].x, polygon[polygon.Count - 1].y);

    for (int i = 0; i < polygon.Count; i++)
    {
        DisplayNode newPoint = new DisplayNode(polygon[i].x, polygon[i].y);

        if (newPoint.x > oldPoint.x)
        {
            p1 = oldPoint;
            p2 = newPoint;
        }
        else
        {
            p1 = newPoint;
            p2 = oldPoint;
        }

        if ((newPoint.x < p.x) == (p.x <= oldPoint.x) && 
           (p.y - p1.y) * (p2.x - p1.x) < (p2.y - p1.y) * (p.x - p1.x))
        {
            inside = !inside;
        }

        oldPoint = newPoint;
    }

    return inside;
}
}

