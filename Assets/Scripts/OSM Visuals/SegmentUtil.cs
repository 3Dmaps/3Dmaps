using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Find the shortest distance between a point and a line segment 
/// </summary>

public static class SegmentUtil {
    public static double FindDistanceToSegmentRiver(DisplayNode point, DisplayNode dpnode, DisplayNode dpnode2){
            
        float dx = dpnode2.x - dpnode.x;
        float dy = dpnode2.y - dpnode.y;
        if ((dx == 0) && (dy == 0)) {
        // It's a point not a line segment.
            dx = point.x - dpnode.x;
            dy = point.y - dpnode.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        // Calculate the t that minimizes the distance.
        float t = ((point.x - dpnode.x) * dx + (point.y - dpnode.y) * dy) / (dx * dx + dy * dy);
        // See if this represents one of the segment's
        // end points or a point in the middle.
        if (t < 0)  {
            dx = point.x - dpnode.x;
            dy = point.y - dpnode.y;
        } else if (t > 1) {
            dx = point.x - dpnode2.x;
            dy = point.y - dpnode2.y;
        } else {        
            dx = point.x - (dpnode.x + t * dx);
            dy = point.y - (dpnode.y + t * dy);
        }
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
