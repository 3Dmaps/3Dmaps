using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 
///  
/// </summary>

public class FindDistanceToSegment {
    private List<DisplayNode> displaynodes = new List<DisplayNode>();
    DisplayNode Closest;

        // Calculate the distance between
        // point pt and the segment p1 --> p2.
        // PointF pt, 
    
    // public void FindDistanceRiver(List<DisplayNode> dpoint, DisplayNode point) {
    //     foreach(DisplayNode points in dpoint) {
    //         FindDistanceToSegmentRiver(points.x,points.y,points.x,points.y,point);
    //     }

    // }
    public double FindDistanceToSegmentRiver(DisplayNode point, DisplayNode dpnode, DisplayNode dpnode2, out DisplayNode closest){
            
        float dx = dpnode2.x - dpnode.x;
        float dy = dpnode2.y - dpnode.y;
        if ((dx == 0) && (dy == 0)) {
                // It's a point not a line segment.
        closest = dpnode;
                dx = point.x - dpnode.x;
                dy = point.y - dpnode.y;
                return Mathf.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }
}
