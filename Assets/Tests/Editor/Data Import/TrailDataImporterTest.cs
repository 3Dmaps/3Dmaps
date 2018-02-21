using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TrailDataImporterTest {
    TrailData trailData;
    float precision;

    [OneTimeSetUp]
    public void Setup() {
        trailData = TrailDataImporter.ReadTrailData("Assets/Resources/testData/testTrailData.xml");
        precision = 0.0001F;        
    }
    
    [Test]
	public void TrailIdCorrect() {
        Assert.True(trailData.trails[0].id == 100000000297, "Trail id incorrect.");
    }

    [Test]
    public void TrailNodeIdCorrect() {
        Assert.True(trailData.trails[0].GetNodeList()[0].GetId() == 173886087, "TrailNode id incorrect.");
        Assert.True(trailData.trails[0].GetNodeList()[1].GetId() == 173895047, "TrailNode id incorrect.");
        Assert.True(trailData.trails[0].GetNodeList()[2].GetId() == 173910289, "TrailNode id incorrect.");
    }

    [Test]
    public void TrailNodeLatLonCorrect() {
        Assert.True(Mathf.Abs(trailData.trails[1].GetNodeList()[2].GetLat() - 37.0383775F) < precision, "TrailNode lat incorrect.");
        Assert.True(Mathf.Abs(trailData.trails[1].GetNodeList()[2].GetLon() - (-111.1872653F)) < precision, "TrailNode lon incorrect.");
    }

	[Test]
	public void TrailColorCorrect() {
		Assert.True(trailData.trails[0].colorName == "unnamedRouteColorName");
	}

}
