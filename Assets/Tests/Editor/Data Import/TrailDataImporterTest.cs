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
        Assert.True(trailData.trails[0].getNodeList()[0].getId() == 173886087, "TrailNode id incorrect.");
    }

    [Test]
    public void TrailNodeLatLonCorrect() {
        Assert.True(Mathf.Abs(trailData.trails[1].getNodeList()[2].getLat() - 37.0383775F) < precision, "TrailNode lat incorrect.");
        Assert.True(Mathf.Abs(trailData.trails[1].getNodeList()[2].getLon() - (-111.1872653F)) < precision, "TrailNode lon incorrect.");
    }
}
