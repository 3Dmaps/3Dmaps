using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Converts coordinates between degrees in latitude-longitude and Web Mercator. Also
/// calculates distances between coordinates in lat-lon.
/// </summary>
public class CoordinateConverter {
    public const double radiansPerDegree = Math.PI / 180;
    public const double degreeEqualsRadians = 0.017453292519943;
    public const double earthsRadius = 6378137;
    
    public const double defaultMeterAsLatLonDegrees = 0.0000092592592593;
    public const float defaultCellsize = 10f;
    double meterAsLatLonDegrees;
    float cellsize;

    /// <summary>
    /// Constructor with no parameters sets the default meter as lan/lon degrees 
    /// value and the cellsize to default value. 
    /// </summary>
    public CoordinateConverter() : this(defaultCellsize) {
    }

    /// <summary>
    /// Constructor with cellsize parameter sets the default meter as lan/lon degrees 
    /// value and the cellsize to the given cellsize. 
    /// </summary>
    public CoordinateConverter(float cellsize) : this(defaultMeterAsLatLonDegrees, cellsize) {
    }

    /// <summary>
    /// User defines meter as lan/lon degrees and the cellsize as parameters. 
    /// </summary>
    public CoordinateConverter(double meterAsLatLonDegrees, float cellsize) {
        this.meterAsLatLonDegrees = meterAsLatLonDegrees;
        this.cellsize = cellsize;
    }

    /// <summary>
    /// Takes a MapPoint(lon,lat) and returns a MapPoint(x,y) with x and y coordinates in WebMercator.
    /// </summary>
    /// <param name="pointToReproject">MapPoint with x as lon and y as lat</param>
    /// <returns>MapPoint with x and y in WebMercator</returns>

    public MapPoint ProjectPointToWebMercator(MapPoint pointToReproject) {
        double rad = pointToReproject.y * radiansPerDegree;
        double fSin = Math.Sin(rad);

        double y = earthsRadius / 2.0 * Math.Log((1.0 + fSin) / (1.0 - fSin));
        double x = pointToReproject.x * degreeEqualsRadians * earthsRadius;

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
    public double TransformCoordinateByDistance(double cells, double startCoordinate) {
        return startCoordinate + (meterAsLatLonDegrees * cells * cellsize);
    }

    /// <summary>
    /// Takes two lat or lon coordinates and calculates the distance in map cells between
    /// the coordinates. Returns the distance in cells.
    /// </summary>
    /// <param name="startCoordinate">The start coordinate in lat or lon</param>
    /// <param name="endCoordinate">The end coordinate in lat or lon</param>
    /// <returns>The distance in map cells</returns>
    public float DistanceBetweenCoordinates(double startCoordinate, double endCoordinate) {
        double coordinateChange = endCoordinate - startCoordinate;
        double distance = coordinateChange * (1 / meterAsLatLonDegrees);
        return (float) (distance / cellsize);
    }

    public static float LatLonDegreesToDefaultMeters(double degrees) {
        return (float)(degrees / defaultMeterAsLatLonDegrees);
    }
}