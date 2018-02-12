using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TrailDataImporterTest {
    //private string trailDataPath = "Assets/Resources/US-1.OsmMapGen.out";
    //private string trailDataPath = "Assets/Resources/note.xml";
    private string trailDataPath = "Assets/Resources/US1.xml";
    TrailDataImporter importer;

    [Test]
	public void TrailDataImporterTestSimplePasses() {
        this.importer = new TrailDataImporter();
        TrailData trailData = importer.ReadTrailData(trailDataPath);
        Debug.Log("Read sample trail data.");
        Debug.Log(trailData);

	}

}
