using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CoordinateConverterTest {
    double precision = 0.00001;

	[Test]
	public void RawCoordinateConversionWorks() {
        MapPoint pointInWebMercator = CoordinateConverter.ProjectPointToWebMercator(new MapPoint(-111.276018518560, 35.449999999998));
        //Debug.Log("Printing x,y: ");
        //Debug.Log(pointInWebMercator.x);
        //Debug.Log(pointInWebMercator.y);

        Assert.True(pointInWebMercator.x - (-12387189.7189888) < precision);
        Assert.True(pointInWebMercator.y - 4225203.75218275 < precision);
    }

    // Temporary test.
    [Test]
    public void temp() {
        MapMetadata metadata = MapDataImporter.ReadMetadata("Assets/Resources/grandcanyon.txt");
        MapData mapdata = MapDataImporter.ReadMapData("Assets/Resources/grandcanyon.txt", metadata);
        MapPoint pointInWebMercator = CoordinateConverter.ProjectPointToWebMercator(new MapPoint(metadata.xllcorner, metadata.yllcorner));
        Debug.Log("Printing in temp x,y: ");
        Debug.Log(pointInWebMercator.x);
        Debug.Log(pointInWebMercator.y);
    }

    
}
