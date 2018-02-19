using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Holds a map point with x and y coordinates.
/// </summary>
public class MapPoint {
    public double x { set; get; }
    public double y { set; get; }

    public MapPoint(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}
