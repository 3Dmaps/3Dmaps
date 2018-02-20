using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class CoordinateConverterTest {
    CoordinateConverter converter;
    double precision = 0.00001;
    double meterInDegrees = 0.0000092592592593;

    [OneTimeSetUp]
    public void Setup() {
        this.converter = new CoordinateConverter(meterInDegrees, 10f);
    }

    [Test]
	public void RawCoordinateConversionWorks() {
        MapPoint pointInWebMercator = converter.ProjectPointToWebMercator(new MapPoint(-111.276018518560, 35.449999999998));
        Assert.True(pointInWebMercator.x - (-12387189.7189888) < precision, "Projection of x to Web Mercator incorrect.");
        Assert.True(pointInWebMercator.y - 4225203.75218275 < precision, "Projection of y to Web Mercator incorrect.");
    }

    [Test]
    public void transformationByOneCellWorks() {
        Assert.True(converter.TransformCoordinateByDistance(1, 0) - (10*meterInDegrees) < precision, 
            "Lat-lon coordinate transformation incorrect.");
    }

    [Test]
    public void transformationByManyCellsWorks() {
        Assert.True(converter.TransformCoordinateByDistance(10812, -112.0005556) - (-110.9994444) < precision, 
            "Lat-lon coordinate transformation incorrect.");
    }
}
