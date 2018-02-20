using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Converts a MapPoint in degrees to a MapPoint (x,y) in Web Mercator coordinates.
/// </summary>
public class CoordinateConverter {
    double meterAsLatLonDegrees;
    float cellsize;

    /// <summary>
    /// Constructor with no parameters sets the default meter as lan/lon degrees 
    /// value and the cellsize to 10 meters. 
    /// </summary>
    public CoordinateConverter() {
        this.meterAsLatLonDegrees = 0.0000092592592593;
        this.cellsize = 10;
    }

    /// <summary>
    /// Constructor with cellsize parameter sets the default meter as lan/lon degrees 
    /// value and the cellsize to the given cellsize. 
    /// </summary>
    public CoordinateConverter(float cellsize) {
        this.meterAsLatLonDegrees = 0.0000092592592593;
        this.cellsize = cellsize;
    }

    public CoordinateConverter(double meterAsLatLonDegrees, float cellsize) {
        this.meterAsLatLonDegrees = meterAsLatLonDegrees;
        this.cellsize = cellsize;
    }

    /// <summary>
    /// Takes a MapPoint(lon,lat) and returns a MapPoint(x,y) with x and y coordinates in WebMercator.
    /// </summary>
    /// <param name="PointToReproject">MapPoint with x as lon and y as lat</param>
    /// <returns>MapPoint with x and y in WebMercator</returns>

    public MapPoint ProjectPointToWebMercator(MapPoint PointToReproject) {
        double RadiansPerDegree = Math.PI / 180;
        double Rad = PointToReproject.y * RadiansPerDegree;
        double FSin = Math.Sin(Rad);
        double DegreeEqualsRadians = 0.017453292519943;
        double EarthsRadius = 6378137;

        double y = EarthsRadius / 2.0 * Math.Log((1.0 + FSin) / (1.0 - FSin));
        double x = PointToReproject.x * DegreeEqualsRadians * EarthsRadius;

        return new MapPoint(x, y);
    }

    /// <summary>
    /// Takes a lat or lon coordinate and transforms it by a number of cells using 
    /// the cellsize and meter value specified in the constructor. Returns the 
    /// transformed lat or lon coordinate.
    /// </summary>
    /// <param name="cells">The number of map cells to transform the coordinate by</param>
    /// <param name="startCoordinate">The start coordinate in lat or lon</param>
    /// <returns>The transformed coordinate in lat or lon</returns>
    public double TransformCoordinateByDistance(int cells, double startCoordinate) {
        return startCoordinate + (meterAsLatLonDegrees * cells * cellsize);
    }

    /// <summary>
    /// Takes a lat or lon coordinate and transforms it by a number of cells using 
    /// the cellsize and meter value specified in the constructor. Returns the 
    /// transformed lat or lon coordinate.
    /// </summary>
    /// <param name="cells">The number of map cells to transform the coordinate by</param>
    /// <param name="startCoordinate">The start coordinate in lat or lon</param>
    /// <returns>The transformed coordinate in lat or lon</returns>
    public double TransformCoordinateByDistance(float cells, double startCoordinate) {
        return startCoordinate + (meterAsLatLonDegrees * cells * cellsize);
    }

    /// <summary>
    /// Takes a lat or lon coordinate and transforms it by a number of cells using 
    /// the cellsize and meter value specified in the constructor. Returns the 
    /// transformed lat or lon coordinate.
    /// </summary>
    /// <param name="cells">The number of map cells to transform the coordinate by</param>
    /// <param name="startCoordinate">The start coordinate in lat or lon</param>
    /// <returns>The transformed coordinate in lat or lon</returns>
    public double TransformCoordinateByDistanceInDouble(double cells, double startCoordinate) {
        return startCoordinate + (meterAsLatLonDegrees * cells * cellsize);
    }
}

