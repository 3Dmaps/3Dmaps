using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class OSMDataImporterTest {
    public OSMData osmData;
    private float precision;

    [OneTimeSetUp]
    public void Setup() {
        osmData = OSMDataImporter.ReadOSMData("Assets/Resources/testData/testTrailData.xml");
        precision = 0.0001F;        
    }
    
    [Test]
	public void TrailIdCorrect() {
        Assert.True(osmData.trails[0].id == 100000000297, "Trail id incorrect.");
    }

    [Test]
    public void TrailNodeIdCorrect() {
        Assert.True(osmData.trails[0].GetNodeList()[0].id == 173886087, "TrailNode id incorrect.");
		Assert.True(osmData.trails[0].GetNodeList()[1].id == 173895047, "TrailNode id incorrect.");
        Assert.True(osmData.trails[0].GetNodeList()[2].id == 173910289, "TrailNode id incorrect.");
    }
    [Test]
    public void CorrectNumberOfPoiNodes() {
        Assert.True(osmData.poiNodes.Count == 2, "Wrong number of points of interest");
    }

    [Test]
    public void CorrectNumberOfAreas() {
        Assert.True(osmData.areas.Count == 1, "Wrong number of areas.");
    }

    [Test]
    public void AreaIdCorrect() {
        Assert.True(osmData.areas[0].id == 175607037, "Area id incorrect.");		
    }

     [Test]
    public void AreaNodeIdCorrect() {
        Assert.True(osmData.areas[0].GetNodeList()[0].id == 1861712094, "Area node id incorrect.");		
    }


    [Test]
    public void CorrentIconFoundOnPoi() {
        Assert.True(osmData.poiNodes[0].icon.Equals("city"), "Wrong icon name");        
    }
    [Test]
    public void CorrectLabelName() {
        bool hasCorrectLabelName = false;
        foreach (Trail trail in osmData.trails) {
            
            if(trail.trailName.Equals("Hiking Route")) {
                hasCorrectLabelName=true;
            }
            Assert.True(hasCorrectLabelName, "Name not found");

        } 
        
        
    }

    [Test]
    public void TrailNodeLatLonCorrect() {
        Assert.True(Mathf.Abs(osmData.trails[1].GetNodeList()[2].lat - 37.0383775F) < precision, "TrailNode lat incorrect.");
        Assert.True(Mathf.Abs(osmData.trails[1].GetNodeList()[2].lon - (-111.1872653F)) < precision, "TrailNode lon incorrect.");
    }

	[Test]
	public void TrailColorCorrect() {
		Assert.True(osmData.trails[0].colorName == "unnamedRouteColorName");
	}

}
