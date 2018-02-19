using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Converts a MapPoint in degrees to a MapPoint (x,y) in Web Mercator coordinates.
/// </summary>
public class CoordinateConverter {
    public static MapPoint ProjectPointToWebMercator(MapPoint PointToReproject) {
        double RadiansPerDegree = Math.PI / 180;
        double Rad = PointToReproject.y * RadiansPerDegree;
        double FSin = Math.Sin(Rad);
        double DegreeEqualsRadians = 0.017453292519943;
        double EarthsRadius = 6378137;

        double y = EarthsRadius / 2.0 * Math.Log((1.0 + FSin) / (1.0 - FSin));
        double x = PointToReproject.x * DegreeEqualsRadians * EarthsRadius;

        return new MapPoint(x, y);
    }
}

